using System;
using UnityEngine;

public class QuestStatus
{
    public QuestRecord record; // Pointer to ESM record for this quest

    public bool hasStarted;
    public Int32 currentQuestStage;

    public QuestStatus(QuestRecord record)
    {
        this.record = record;
        hasStarted = false;
        currentQuestStage = -1;
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

        Debug.Log("Current stage of quest " + record.EDID + " = " + currentQuestStage + "\n");
    }
}
