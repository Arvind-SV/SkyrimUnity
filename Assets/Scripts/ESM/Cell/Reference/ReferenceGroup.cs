using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReferenceGroup : BaseGroup
{
    public Dictionary<UInt32, ReferenceRecord> cellReferences = new();

    public ReferenceGroup(BaseGroup baseGroup) : base(baseGroup)
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

            if (recordType == "REFR")
            {
                ReferenceRecord referenceRecord = new(record);
                referenceRecord.ReadFromFile();
                cellReferences[referenceRecord.recordFormID] = referenceRecord;
            }

            processedBytes += recordSize;
        }
    }
}
