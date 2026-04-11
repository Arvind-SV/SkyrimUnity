using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public static class PapyrusScriptManager
{
    public static Dictionary<string, PEXFileData> loadedScriptsData = new();

    public static void ProcessScript(PapyrusScriptData scriptESMData, string scriptName, string functionName, List<PapyrusScriptFunctionArgument> args)
    {
        Debug.Log("Script : " + scriptName + " Function : " + functionName + "\n");

        PEXFileData scriptData;

        if(loadedScriptsData.ContainsKey(scriptName))
        {
            scriptData = loadedScriptsData[scriptName];
        }
        else
        {
            // Reading script file
            BinaryReader pexFile = ResourceManager.LoadFile("Scripts/" + scriptName + ".pex");

            scriptData = new();
            scriptData.ReadFromFile(pexFile);

            loadedScriptsData[scriptName] = scriptData;

            pexFile.Close();
        }

        // Running instructions of fragment
        RunScriptFunction(scriptName, scriptESMData, scriptData, functionName, args);
    }

    public static void RunScriptFunction(string scriptName, PapyrusScriptData scriptESMData, PEXFileData script, string functionName, List<PapyrusScriptFunctionArgument> args)
    {
        // Get Function data for fragment
        PEXFunction function = script.GetFunctionByName(functionName);

        // Process all instructions sequentially
        if (function != null)
        {
            if ((function.flags & (byte)CommonPEXDefines.FunctionFlag.Native) > 0)
            {
                ExecuteNativeFunction(scriptName, scriptESMData, script, functionName, args);
            }
            else
            {
                PapyrusScriptStack stack = new();
                stack.RunInstructions(scriptName, scriptESMData, script, function);
            }     
        }
        else
        {
            Debug.Log("Script function not found " + functionName + "\n");
        }
    }

    public static void ExecuteNativeFunction(string scriptName, PapyrusScriptData scriptESMData, PEXFileData script, string functionName, List<PapyrusScriptFunctionArgument> args)
    {
        if(functionName == "SetValue")
        {
            if(scriptName == "globalvariable")
            {
                // Argument 1 : Variable to set value to
                UInt32 formID = args[0].uint32Data;
                GlobalRecord globalRecord = SkyrimUnity.engine.GetGlobalRecord(formID);

                if(globalRecord != null)
                {
                    // Argument 2 ignored

                    // Argument 3 : Value to be set
                    float value = args[2].floatData;
                    
                    SkyrimUnity.engine.SetGlobalValue(formID, value);
                }
            }
            else
            {
                Debug.LogError("Unsupported parent type for function " + functionName + " : " + scriptName + "\n");
            }
        }
        else
        {
            Debug.Log("Unimplemented native function " + functionName + "\n");
        }
    }
}
