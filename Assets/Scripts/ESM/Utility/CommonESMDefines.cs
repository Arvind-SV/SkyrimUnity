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

    public enum ScriptPropertyType
    {
        Object = 1,
        WString = 2,
        Int = 3,
        Float = 4,
        Bool = 5,
        ObjectArray = 11,
        WStringArray = 12,
        IntArray = 13,
        FloatArray = 14,
        BoolArray = 15
    };
    
    public static string[] questLogEntryFields = new string[]
    {
        "CTDA",
        "CNAM",
        "NAM0",
        "SCHR"
    };

    
}
