using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public static class PapyrusScriptManager
{
    public static Dictionary<string, PEXFileData> loadedScriptsData = new();

    public static void ProcessScript(PapyrusScriptData scriptESMData, string scriptName, string functionName, List<PEXVariableData> args)
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
        RunScriptFunction(scriptESMData, scriptData, functionName, args);
    }

    public static void RunScriptFunction(PapyrusScriptData scriptESMData, PEXFileData script, string functionName, List<PEXVariableData> args)
    {
        // Get Function data for fragment
        PEXFunction function = script.GetFunctionByName(functionName);

        // Process all instructions sequentially
        if (function != null)
        {
            if ((function.flags & (byte)CommonPEXDefines.FunctionFlag.Native) > 0)
            {
                ExecuteNativeFunction(functionName, args);
            }
            else
            {
                PapyrusScriptStack stack = new();
                stack.RunInstructions(scriptESMData, script, function);
            }     
        }
        else
        {
            Debug.Log("Script function not found " + functionName + "\n");
        }
    }

    public static void ExecuteNativeFunction(string functionName, List<PEXVariableData> args)
    {
        if(functionName == "SetValue")
        {
            // 1st argument. Variable to set value to

        }
        else
        {
            Debug.Log("Unimplemented native function " + functionName + "\n");
        }
    }
}
