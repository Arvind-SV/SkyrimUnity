using System.Collections.Generic;
using UnityEngine;

public static class QuestManager
{
    public static QuestGroup quests;
    public static Dictionary<string, QuestRecord> activeQuests;

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
                Debug.Log("Starting quest " + questRecord.FULL + "\n");
                activeQuests[edid] = questRecord;
            }
            else
            {
                Debug.LogError("Quest with EDID " + edid + " not found!\n");
            }
        }
    }
}
