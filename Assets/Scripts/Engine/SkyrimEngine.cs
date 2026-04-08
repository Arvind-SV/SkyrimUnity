using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyrimEngine
{
    public CommonDefines.GameState gameState;

    public ESMParser esmData;
    
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

    public float GetGlobalValue(UInt32 formID)
    {
        float value = -12345678.0f; // Random invalid value

        if(esmData.globalVariables.globalRecords.ContainsKey(formID))
        {
            value = esmData.globalVariables.globalRecords[formID].FLTV;
        }

        return value;
    }
}
