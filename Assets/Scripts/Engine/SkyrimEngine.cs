using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyrimEngine
{
    public CommonDefines.GameState gameState;

    public ESMParser esmData;

    // Holds values of global records, if modified
    public Dictionary<UInt32, float> currentGlobalRecordValues = new();

    public SkyrimEngine()
    {
        // Setting pointer in SkyrimUnity to use in other classes
        SkyrimUnity.engine = this;

        gameState = CommonDefines.GameState.None;
    }

    public void Initialize()
    {
        // Read ESM files
        esmData = new();
        esmData.ParseESMFile(SkyrimUnity.skyrimDataFilesPath + SkyrimUnity.skyrimESM);

        QuestManager.Initialize();

        gameState = CommonDefines.GameState.BGSLogo;

        // Create main scene where Skyrim is rendered
        SetUpMainRenderScene();
    }

    public void SetUpMainRenderScene()
    {
        Scene mainScene = SceneManager.CreateScene("SkyrimUnity");
        SceneManager.SetActiveScene(mainScene);
    }

    public void StartNewGame()
    {
        // At the beginning of the game, quest MQ101(Unbound) is started
        QuestManager.StartQuest("MQ101");
    }

    public GlobalRecord GetGlobalRecord(UInt32 formID)
    {
        GlobalRecord record = null;

        if(esmData.globalVariables.globalRecords.ContainsKey(formID))
        {
            record = esmData.globalVariables.globalRecords[formID];
        }

        if(record == null)
        {
            Debug.LogError("Global Variable record with formID " + formID + " not found\n");
        }
        
        return record;
    }
    public float GetGlobalValue(UInt32 formID)
    {
        float value = -12345678.0f; // Random invalid value

        if(currentGlobalRecordValues.ContainsKey(formID))
        {
            // Take latest modified value
            value = currentGlobalRecordValues[formID];
        }
        else if (esmData.globalVariables.globalRecords.ContainsKey(formID))
        {
            // Take value from ESM global record
            value = esmData.globalVariables.globalRecords[formID].FLTV;
        }
        else
        {
            // formID not found
        }

        return value;
    }

    public float SetGlobalValue(UInt32 formID, float value)
    {

        if (esmData.globalVariables.globalRecords.ContainsKey(formID))
        {
            currentGlobalRecordValues[formID] = value;
        }

        return value;
    }
}
