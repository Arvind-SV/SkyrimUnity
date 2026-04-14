using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GlobalGroup : BaseGroup
{
    public Dictionary<UInt32, GlobalRecord> globalRecords = new();

    public GlobalGroup(BaseGroup baseGroup) : base(baseGroup)
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
            recordSize = record.compressedDataSize;

            if (recordType == "GLOB")
            {
                GlobalRecord globalRecord = new(record);
                globalRecord.ReadFromFile();
                globalRecords[globalRecord.recordFormID] = globalRecord;
            }
            else
            {
                Debug.Log("Unexpected record type found in global group: " + recordType + "\n");
            }

            processedBytes += recordSize;
        }
    }
}
