using UnityEngine;

public static class CommonESMDefines
{
    public enum RecordFlags
    {
        Localized = 0x000080
    };

    public enum GroupLabelType
    {
        Top = 0
    };

    public enum QuestStageFlags
    {
        StartUpStage = 0x02
    };

    public enum ConditionOperatorFlags
    {
        UseGlobal = 0x04
    };

    public enum FunctionIndex
    {
        GetGlobalValue = 74,
        GetEventData = 576
    };

    public static string[] questLogEntryFields = new string[]
    {
        "CTDA",
        "CNAM",
        "NAM0",
        "SCHR"
    };

    
}
