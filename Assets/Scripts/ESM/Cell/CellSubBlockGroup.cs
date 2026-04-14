using System;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CellSubBlockGroup : BaseGroup
{
    public CellSubBlockGroup(BaseGroup baseGroup) : base(baseGroup)
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

            if (recordType == "CELL")
            {
                CellRecord cellRecord = new(record);
                cellRecord.ReadFromFile();
                SkyrimUnity.engine.esmData.interiorCells.interiorCellRecords[cellRecord.recordFormID] = cellRecord;

                processedBytes += ReadCellReferencesFromFile(file, cellRecord);
            }
            else
            {
                Debug.Log("Unexpected record type found in cell group: " + recordType + "\n");
            }

            processedBytes += recordSize;
        }
    }

    public UInt32 ReadCellReferencesFromFile(BinaryReader file, CellRecord cellRecord)
    {
        BaseGroup referenceTopGroup = new();
        referenceTopGroup.ReadFromFile(file);

        UInt32 processedBytes = 24;

        UInt32 numBytes = referenceTopGroup.size;

        if(referenceTopGroup.groupLabelType == (Int32)CommonESMDefines.GroupLabelType.CellChildren)
        {
            while(processedBytes < numBytes)
            {
                BaseGroup referenceGroup = new();
                referenceGroup.ReadFromFile(file);

                processedBytes += 24;

                if((referenceGroup.groupLabelType == (Int32)CommonESMDefines.GroupLabelType.CellPersistentChildren)) 
                {
                    cellRecord.persistentReferences = new(referenceGroup);
                    cellRecord.persistentReferences.ReadFromFile(file);
                }
                else if (referenceGroup.groupLabelType == (Int32)CommonESMDefines.GroupLabelType.CellTemporaryChildren)
                {
                    cellRecord.temporaryReferences = new(referenceGroup);
                    cellRecord.temporaryReferences.ReadFromFile(file);
                }
                else
                {
                    Debug.LogError("Unsupported group type found inside cell references " + referenceGroup.groupLabelType + "\n");
                    file.BaseStream.Position += referenceGroup.size - 24;
                }

                processedBytes += referenceGroup.size - 24;
            }
        }
        else
        {
            Debug.LogError("Expected group label type cell children. Got " + referenceTopGroup.groupLabelType + "\n");
            file.BaseStream.Position += referenceTopGroup.size - 24;
        }

        processedBytes = referenceTopGroup.size;

        return processedBytes;
    }
}
