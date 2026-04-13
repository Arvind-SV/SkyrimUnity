using System;
using System.Collections.Generic;
using UnityEngine;

public static class QuestManager
{
    public static QuestGroup quests;
    public static Dictionary<string, QuestStatus> activeQuests;

    public static void Initialize()
    {
        quests = SkyrimUnity.engine.esmData.quests;
        activeQuests = new();
    }

    public static void StartQuest(string edid)
    {
        if(activeQuests.ContainsKey(edid))
        {
            // Quest already active
        }
        else
        {
            QuestRecord questRecord = quests.GetQuestWithEDID(edid);

            if(questRecord != null)
            {
                QuestStatus questStatus = new(questRecord);
                activeQuests[edid] = questStatus;

                questStatus.ProcessQuest();
            }
            else
            {
                Debug.LogError("Quest with EDID " + edid + " not found!\n");
            }
        }
    }

    public static void SetQuestStage(UInt32 questFormID, Int16 stage)
    {
        QuestRecord questRecord = quests.questRecords[questFormID];

        string edid = questRecord.EDID;

        Debug.Log("Quest EDID : " + edid + " Stage : " + stage + "\n");

        QuestStatus questStatus = activeQuests[edid];
        questStatus.ProcessStage(stage);
    }
}
