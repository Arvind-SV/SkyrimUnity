using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PapyrusScriptManager
{
    public static Dictionary<string, PEXFileData> loadedScriptsData = new();

    public static void ProcessScript(QuestRecord record, string scriptName, string fragmentName)
    {
        Debug.Log("Quest : " + record.EDID + " Script : " + scriptName + " Fragment : " + fragmentName + "\n");

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
    }
}
