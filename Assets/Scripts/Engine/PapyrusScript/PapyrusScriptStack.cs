using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.DedicatedServer;

public class PapyrusScriptStack
{
    public UInt32 nextInstruction;
    // "Stack" for holding current value of function local variables
    public Dictionary<string, float> localVariablesValues = new();

    public PapyrusScriptStack()
    {
        nextInstruction = 0; // Initialize to first instruction
    }

    public void RunInstructions(string scriptName, PapyrusScriptData scriptESMData, PEXFileData script, PEXFunction function)
    {
        while(nextInstruction < function.numInstructions)
        {
            PEXInstruction instruction = function.instructions[nextInstruction];
            DecodeAndExecuteInstruction(scriptName, scriptESMData, script, instruction);

            nextInstruction++;
        }
    }

    public PapyrusScriptFunctionArgument ConvertArgumentForOtherScripts(PapyrusScriptData scriptESMData, PEXFileData script, PEXVariableData arg)
    {
        PapyrusScriptFunctionArgument data = new();

        PEXProperty property = script.GetProperty(arg);

        if(property != null)
        {
            string propertyName = property.name;

            // Get value of property from script's ESM record
            PapyrusScriptProperty propertyData = scriptESMData.GetProperty(propertyName);

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

            if(IsLocalVariable(arg))
            {
                // Get current local variable value from stack
                float varValue = localVariablesValues[arg.stringData];

                data.type = typeof(float);
                data.floatData = varValue;
            }
        }

        return data;
    }

    public bool IsLocalVariable(PEXVariableData arg)
    {
        bool isLocalVariable = false;

        if((arg.type == 1) || (arg.type == 2))
        {
            if(localVariablesValues.ContainsKey(arg.stringData))
            {
                isLocalVariable = true;
            }
        }

        return isLocalVariable;
    }

    public void DecodeAndExecuteInstruction(string scriptName, PapyrusScriptData scriptESMData, PEXFileData script, PEXInstruction instruction)
    {
        byte opcode = instruction.op;
        List<PEXVariableData> arguments = instruction.arguments;

        if (opcode == (byte)CommonPEXDefines.InstructionOpcodes.CAST)
        {
            // Assignment operation
            Debug.Log("Assignment Operation\n");

            // 1st argument : Name of local variable
            PEXVariableData localVariableName = arguments[0];
            string varName = localVariableName.stringData;

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

            if(var != null)
            {
                // called function is part of script belonging to type of parent object
                parentType = var.typeName;
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
                List<PapyrusScriptFunctionArgument> functionArgs = new();

                for (int i = 1; i < arguments.Count; i++)
                {
                    PEXVariableData arg = arguments[i];
                    PapyrusScriptFunctionArgument processedArg = ConvertArgumentForOtherScripts(scriptESMData, script, arg);

                    functionArgs.Add(processedArg);
                }

                // Call function on parent object script
                PapyrusScriptManager.ProcessScript(scriptESMData, parentType, functionName, functionArgs);
            }
        }
        else
        {
            Debug.Log("Unimplemented opcode " + opcode + "\n");
        }
    }
}
