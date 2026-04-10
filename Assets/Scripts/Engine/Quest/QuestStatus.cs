using System;
using UnityEngine;

public class QuestStatus
{
    public QuestRecord record; // Pointer to ESM record for this quest

    public bool hasStarted;
    public Int16 currentQuestStage;

    public QuestStatus(QuestRecord record)
    {
        this.record = record;
        hasStarted = false;
        currentQuestStage = -1;
    }

    public void ProcessStage(Int16 stageIdx)
    {
        QuestStage stageData = record.questStages[stageIdx];

        // Find log entries for which conditions are fulfilled
        foreach(UInt32 logIdx in stageData.questLogEntries.Keys)
        {
            QuestLogEntry entry = stageData.questLogEntries[logIdx];

            bool isConditionFulfilled = false;

            foreach(Condition cond in entry.CTDA)
            {
                if(cond.IsConditionFulfilled())
                {
                    isConditionFulfilled = true;
                }
            }

            if(isConditionFulfilled)
            {
                // Get script fragment corresponding to this stage and log entry
                PapyrusScriptFragment fragment = record.GetScriptFragment(stageIdx, (Int32)logIdx);

                if(fragment != null)
                {
                    // No arguments being provided
                    PapyrusScriptManager.ProcessScript(record.VMAD.scripts[fragment.scriptName], fragment.scriptName, fragment.fragmentName, new());
                }
                else
                {
                    Debug.Log("Script fragment with quest stage " + stageIdx + " and log entry " + logIdx + " not found\n");
                }
            }
        }
    }

    // Responsible for managing quest state(active stage, scripts, etc)
    public void ProcessQuest()
    {
        if(!hasStarted)
        {
            // Start the quest from startup stage
            hasStarted = true;

            currentQuestStage = record.GetStartUpStage();
        }

        // Run attached scripts for this quest stage
        ProcessStage(currentQuestStage);
    }
}
