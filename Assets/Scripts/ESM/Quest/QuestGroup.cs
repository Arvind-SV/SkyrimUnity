using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuestGroup : BaseGroup
{
    public Dictionary<UInt32, QuestRecord> questRecords = new();

    public QuestGroup(BaseGroup baseGroup) : base(baseGroup)
    {

    }

    public override void ReadFromFile(BinaryReader file)
    {
        UInt32 processedBytes = 0;

        UInt32 numBytes = size - 24; // Subtract the size of the group header

        string recordType;
        UInt32 recordSize;

        while (processedBytes < numBytes)
        {
            // Read each record in the group
            BaseRecord record = new();
            processedBytes += record.ReadFromFile(file);

            recordType = record.type;
            recordSize = record.size;

            if (recordType == "QUST")
            {
                QuestRecord questRecord = new(record);
                questRecord.ReadFromFile(file);
                questRecords[questRecord.recordFormID] = questRecord;
            }
            else
            {
                Debug.Log("Unexpected record type found in quest group: " + recordType + "\n");
                file.BaseStream.Position += recordSize;
            }

            processedBytes += recordSize;
        }
    }

    public QuestRecord GetQuestWithEDID(string edid)
    {
        QuestRecord questRecord = null;

        foreach(QuestRecord record in questRecords.Values)
        {
            if(record.EDID == edid)
            {
                questRecord = record;
                break;
            }
        }

        return questRecord;
    }
}
