using System;
using System.Collections.Generic;
using UnityEngine;

public class PapyrusScriptStack
{
    public string scriptName;

    // Pointer to ESM object to which this script is attached
    public BaseRecord esmRecord;

    public UInt32 nextInstruction;
    // "Stack" for holding current value of function local variables
    public Dictionary<string, float> localVariablesValues = new();
    public Dictionary<string, float> parameterValues = new();

    public PapyrusScriptStack(string scriptName, BaseRecord esmRecord)
    {
        nextInstruction = 0; // Initialize to first instruction
        this.scriptName = scriptName;
        this.esmRecord = esmRecord;
    }

    public void RunInstructions(PEXFileData script, PEXFunction function, List<PapyrusScriptFunctionArgument> parameters)
    {
        // Initialize local variables to 0
        foreach(string name in function.locals.Keys)
        {
            localVariablesValues[name] = 0.0f;
        }

        int idx = 0;

        // Extract parameter values
        foreach (string name in function.parameters.Keys)
        {
            parameterValues[name] = parameters[idx].floatData;
            idx++;
        }

        while(nextInstruction < function.numInstructions)
        {
            PEXInstruction instruction = function.instructions[nextInstruction];
            DecodeAndExecuteInstruction(function, script, instruction);

            nextInstruction++;
        }
    }

    public PapyrusScriptFunctionArgument ConvertArgumentForOtherScripts(PEXFunction functionData, PEXFileData script, PEXVariableData arg)
    {
        PapyrusScriptFunctionArgument data = new();

        PEXProperty property = script.GetProperty(arg);

        if(property != null)
        {
            string propertyName = property.name;

            // Get value of property from script's ESM record
            PapyrusScriptProperty propertyData = esmRecord.GetScriptProperty(scriptName, propertyName);

            if(propertyData != null)
            {
                if(propertyData.propertyType == (byte)CommonESMDefines.ScriptPropertyType.Object)
                {
                    // Non array properties only have one data item
                    PapyrusScriptPropertyObject propertyObject = propertyData.propertyObjectValues[0];

                    data.type = typeof(UInt32);
                    data.uint32Data = propertyObject.formID;
                }
                else
                {
                    Debug.LogError("Unsupported property type " + propertyData.propertyType + "\n");
                }
            }
            else
            {
                Debug.LogError("Property name " + propertyName + "not found in script esm data\n");
            }
        }
        else
        {
            // Variable is not a property

            if(functionData.IsLocalVariable(arg))
            {
                // Get current local variable value from stack
                float varValue = localVariablesValues[arg.stringData];

                data.type = typeof(float);
                data.floatData = varValue;
            }
            else if(IsParameter(arg))
            {
                // Get parameter value
                float paramValue = parameterValues[arg.stringData];

                data.type = typeof(float);
                data.floatData = paramValue;
            }
            else
            {
                // Variable contains direct value
                float varValue = arg.GetValue();

                data.type = typeof(float);
                data.floatData = varValue;
            }
        }

        return data;
    }

    public bool IsParameter(PEXVariableData variable)
    {
        bool isParameter = false;

        if ((variable.type == 1) || (variable.type == 2))
        {
            if (parameterValues.ContainsKey(variable.stringData))
            {
                isParameter = true;
            }
        }

        return isParameter;
    }

    public void DecodeAndExecuteInstruction(PEXFunction functionData, PEXFileData script, PEXInstruction instruction)
    {
        byte opcode = instruction.op;
        List<PEXVariableData> arguments = instruction.arguments;

        if (opcode == (byte)CommonPEXDefines.InstructionOpcodes.CAST)
        {
            // Assignment operation
            Debug.Log("Assignment Operation\n");

            string varName = "";

            // 1st argument : Name of local variable
            if (functionData.IsLocalVariable(arguments[0]))
            {
                varName = arguments[0].stringData;    
            }
            
            // 2nd argument : Value
            PEXVariableData valueData = arguments[1];
            float value = valueData.GetValue();

            localVariablesValues[varName] = value;
        }
        else if(opcode == (byte)CommonPEXDefines.InstructionOpcodes.CALLMETHOD)
        {
            // 1st argument. Function name
            PEXVariableData functionNameData = arguments[0];
            string functionName = functionNameData.stringData;
            Debug.Log("Executing function  " + functionName + "\n");

            // 2nd argument : ESM object to which this function is applied(self = parent script object)
            PEXVariableData parentObjectData = arguments[1];
            string parentObjectName = parentObjectData.stringData;

            // Get type of parent object
            PEXVariable var = script.GetVariableByName(parentObjectName);

            string parentType;

            List<PapyrusScriptFunctionArgument> functionArgs = new();

            if (var != null)
            {
                // called function is part of script belonging to type of parent object
                parentType = var.typeName;

                functionArgs.Add(ConvertArgumentForOtherScripts(functionData, script, parentObjectData));
            }
            else
            {
                if(parentObjectName == "self")
                {
                    // Called function is either part of same script, or part of inherited script

                    PEXFunction function = script.GetFunctionByName(functionName);
                    if(function != null)
                    {
                        // Called function is part of same script
                        parentType = scriptName;
                    }
                    else
                    {
                        // Called function is part of inherited script object
                        parentType = script.objects[0].parentClassName;
                    }

                    string objectID = esmRecord.GetObjectID();

                    Debug.Log("Object ID : " + objectID + "\n");

                    // Create script object argument containing object id.
                    PapyrusScriptFunctionArgument objectArg = new();
                    objectArg.type = typeof(UInt32);
                    objectArg.uint32Data = esmRecord.recordFormID;
                    functionArgs.Add(objectArg);
                }
                else
                {
                    // Unknown scenario
                    parentType = null;
                    Debug.LogError("Parent type for instruction not found\n");
                }
            }

            if(parentType != null)
            {
                // Prepare argument list for imported function

                for (int i = 2; i < arguments.Count; i++)
                {
                    PEXVariableData arg = arguments[i];
                    PapyrusScriptFunctionArgument processedArg = ConvertArgumentForOtherScripts(functionData, script, arg);

                    functionArgs.Add(processedArg);
                }

                // Call function on parent object script
                PapyrusScriptManager.ProcessScript(esmRecord, parentType, functionName, functionArgs);
            }
        }
        else
        {
            Debug.Log("Unimplemented opcode " + opcode + "\n");
        }
    }
}
