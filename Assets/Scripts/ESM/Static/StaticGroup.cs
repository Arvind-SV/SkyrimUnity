using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class StaticGroup : BaseGroup
{
    public Dictionary<UInt32, StaticRecord> staticRecords = new();

    public StaticGroup(BaseGroup baseGroup) : base(baseGroup)
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

            if (recordType == "STAT")
            {
                StaticRecord staticRecord = new(record);
                staticRecord.ReadFromFile();
                staticRecords[staticRecord.recordFormID] = staticRecord;
            }
            else
            {
                Debug.Log("Unexpected record type found in static group: " + recordType + "\n");
                file.BaseStream.Position += recordSize;
            }

            processedBytes += recordSize;
        }
    }
}
