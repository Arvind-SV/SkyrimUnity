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

    public override UInt32 ReadFromFile(BinaryReader file)
    {
        UInt32 processedBytes = 0;

        UInt32 numBytes = size;

        string fieldType;
        UInt16 fieldSize;


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
                VMAD = new();
                VMAD.ReadFromFile(file);
            }
            else
            {
                file.BaseStream.Position += fieldSize;
            }

            processedBytes += fieldSize;
        }

        return processedBytes;
    }

    public Int32 GetStartUpStage()
    {
        Int32 startUpStage = -1;

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
