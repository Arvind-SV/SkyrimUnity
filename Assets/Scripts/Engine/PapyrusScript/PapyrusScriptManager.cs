using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public static class PapyrusScriptManager
{
    public static Dictionary<string, PEXFileData> loadedScriptsData = new();

    public static void ProcessScript(BaseRecord esmRecord, string scriptName, string functionName, List<PapyrusScriptFunctionArgument> args)
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
        RunScriptFunction(scriptName, esmRecord, scriptData, functionName, args);
    }

    public static void RunScriptFunction(string scriptName, BaseRecord esmRecord, PEXFileData script, string functionName, List<PapyrusScriptFunctionArgument> args)
    {
        // Get Function data for fragment
        PEXFunction function = script.GetFunctionByName(functionName);

        // Process all instructions sequentially
        if (function != null)
        {
            PapyrusScriptFunctionArgument objectData = null;
            PapyrusScriptFunctionArgument returnObjectData = null;
            List<PapyrusScriptFunctionArgument> functionParameters = null;

            if (args != null)
            {
                // Script function has arguments
                functionParameters = new();

                // Argument 1 is object to which function is applied
                objectData = args[0];

                // Argument 2 is object for holding return value. Currently ignored

                // Argument 3 onwards is the function parameters
                for(int i = 2; i < args.Count; i++)
                {
                    functionParameters.Add(args[i]);
                }
            }

            if ((function.flags & (byte)CommonPEXDefines.FunctionFlag.Native) > 0)
            {
                ExecuteNativeFunction(scriptName, functionName, objectData, returnObjectData, functionParameters);
            }
            else
            {
                PapyrusScriptStack stack = new(scriptName, esmRecord);
                stack.RunInstructions(script, function, functionParameters);
            }     
        }
        else
        {
            Debug.Log("Script function not found " + functionName + "\n");
        }
    }

    public static void ExecuteNativeFunction(string scriptName, string functionName, PapyrusScriptFunctionArgument functionObject, PapyrusScriptFunctionArgument returnObject,  List<PapyrusScriptFunctionArgument> parameters)
    {
        if(functionName == "SetValue")
        {
            if (scriptName == "globalvariable")
            {
                if (functionObject != null)
                {
                    UInt32 formID = functionObject.uint32Data;
                    GlobalRecord globalRecord = SkyrimUnity.engine.GetGlobalRecord(formID);

                    if (globalRecord != null)
                    {
                        float value = parameters[0].floatData;

                        SkyrimUnity.engine.SetGlobalValue(formID, value);
                    }
                }
            }
            else
            {
                Debug.LogError("Unsupported parent type for function " + functionName + " : " + scriptName + "\n");
            }
        }
        else if(functionName == "SetCurrentStageID")
        {
            if(scriptName == "Quest")
            {
                // Get ID of quest
                UInt32 formID = functionObject.uint32Data;

                // Get requested quest stage
                float stage = parameters[0].floatData;

                // Get quest state object
                QuestManager.SetQuestStage(formID, (Int16)stage);
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
