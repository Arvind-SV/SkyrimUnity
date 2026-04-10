using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.DedicatedServer;

public class PapyrusScriptStack
{
    public UInt32 nextInstruction;
    public Dictionary<string, float> localVariablesValues = new();

    public PapyrusScriptStack()
    {
        nextInstruction = 0; // Initialize to first instruction
    }

    public void RunInstructions(PapyrusScriptData scriptESMData, PEXFileData script, PEXFunction function)
    {
        while(nextInstruction < function.numInstructions)
        {
            PEXInstruction instruction = function.instructions[nextInstruction];
            DecodeAndExecuteInstruction(script, instruction);

            nextInstruction++;
        }
    }

    public void DecodeAndExecuteInstruction(PEXFileData script, PEXInstruction instruction)
    {
        byte opcode = instruction.op;
        List<PEXVariableData> arguments = instruction.arguments;

        if (opcode == (byte)CommonPEXDefines.InstructionOpcodes.CAST)
        {
            // Assignment operation
            Debug.Log("Assignment Operation\n");

            // 1st argument : Name of local variable
            PEXVariableData localVariableName = arguments[0];
            string varName = script.stringTable[localVariableName.indexData];

            // 2nd argument : Value
            PEXVariableData valueData = arguments[1];
            float value = valueData.GetValue();

            localVariablesValues[varName] = value;
        }
        else if(opcode == (byte)CommonPEXDefines.InstructionOpcodes.CALLMETHOD)
        {
            Debug.Log("Function call operation\n");

            // 1st argument. Function name
            PEXVariableData functionNameData = arguments[0];
            string functionName = script.stringTable[functionNameData.indexData];
            Debug.Log("Executing function  " + functionName + "\n");

            // 2nd argument : ESM object to which this function is applied(self = parent script object)
            PEXVariableData parentObjectData = arguments[1];
            string parentObjectName = script.stringTable[parentObjectData.indexData];
            Debug.Log("Parent object : " + parentObjectName + "\n");

            // Get type of parent object
            PEXVariable var = script.GetVariableByName(parentObjectName);

            if(var != null)
            {
                string parentType = script.stringTable[var.typeName];
                Debug.Log("Parent object type " + parentType + "\n");

                // Prepare argument list for imported function
                List<PEXVariableData> functionArgs = new();

                for(int i = 1; i < arguments.Count; i++)
                {
                    functionArgs.Add(arguments[i]);
                }

                // Call function on parent object script
                PapyrusScriptManager.ProcessScript(null, parentType, functionName, functionArgs);
            }
            else
            {
                if(parentObjectName == "self")
                {
                    Debug.Log("Self object\n");
                }
            }
        }
        else
        {
            Debug.Log("Unimplemented opcode " + opcode + "\n");
        }
    }
}
