using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class QuestRecord : BaseRecord
{
    public string EDID;
    public string FULL;
    public PapyrusScriptVMAD VMAD;
    public Dictionary<Int16, QuestStage> questStages = new();

    public QuestRecord(BaseRecord baseRecord) : base(baseRecord)
    {

    }

    public override PapyrusScriptProperty GetScriptProperty(string scriptName, string propertyName)
    {
        PapyrusScriptProperty property;

        PapyrusScriptData scriptData = VMAD.scripts[scriptName];

        property = scriptData.GetProperty(propertyName);

        return property;
    }

    public override string GetObjectID()
    {
        return EDID;
    }

    public PapyrusScriptFragment GetScriptFragment(Int16 questStageIdx, Int32 logEntryIdx)
    {
        PapyrusScriptFragment fragment = null;

        List<PapyrusScriptFragment> scriptFragments = VMAD.fragments;

        foreach(PapyrusScriptFragment scriptFragment in scriptFragments)
        {
            if((questStageIdx == scriptFragment.index) && (logEntryIdx == scriptFragment.logEntry))
            {
                fragment = scriptFragment;
                break;
            }
        }

        return fragment;
    }

    public void ReadFromFile()
    {
        UInt32 processedBytes = 0;

        UInt32 numBytes = size;

        string fieldType;
        UInt16 fieldSize;

        BinaryReader file = recordData;

        while (processedBytes < numBytes)
        {
            // Each field starts with a 4 byte field type and 2 byte field size
            fieldType = new(file.ReadChars(4));
            fieldSize = file.ReadUInt16();
            processedBytes += 6;

            if(fieldType == "EDID")
            {
                EDID = StringUtil.ReadZStringFromFile(file, fieldSize);
            }
            else if(fieldType == "FULL")
            {
                FULL = StringUtil.ReadLStringFromFile(file, fieldSize);
            }
            else if(fieldType == "INDX")
            {
                // Quest stage entry
                QuestStage stage = new();
                processedBytes += stage.ReadFromFile(file);

                questStages[stage.index] = stage;

                // As it gets incremented by fieldSize at the end of while loop iteration
                processedBytes -= fieldSize;
            }
            else if(fieldType == "VMAD")
            {
                // Script data
                Int64 currentPos = file.BaseStream.Position;

                VMAD = new();
                VMAD.ReadFromFile(file, "QUST", fieldSize);

                // Moving file pointer to expected position(incase reading script fails)
                file.BaseStream.Position = currentPos + fieldSize;
            }
            else
            {
                file.BaseStream.Position += fieldSize;
            }

            processedBytes += fieldSize;
        }

        recordData.Close();
        file.Close();
        recordData = null;
    }

    public Int16 GetStartUpStage()
    {
        Int16 startUpStage = -1;

        foreach(QuestStage stage in questStages.Values)
        {
            if((stage.flags & (byte)CommonESMDefines.QuestStageFlags.StartUpStage) > 0)
            {
                startUpStage = stage.index;
                break;
            }
        }

        return startUpStage;
    }
}
