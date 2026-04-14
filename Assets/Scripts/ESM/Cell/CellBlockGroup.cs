using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CellBlockGroup : BaseGroup
{
    public CellBlockGroup(BaseGroup baseGroup) : base(baseGroup)
    {

    }

    public override void ReadFromFile(BinaryReader file)
    {
        UInt32 processedBytes = 0;

        UInt32 numBytes = size - 24; // Subtract the size of the group header

        while (processedBytes < numBytes)
        {
            // Read cell subblock group
            BaseGroup group = new();
            group.ReadFromFile(file);
            processedBytes += 24; // Group info is always 24 bytes

            if (group.groupLabelType == (Int32)CommonESMDefines.GroupLabelType.InteriorCellSubBlock)
            {
                CellSubBlockGroup cellSubBlock = new(group);
                cellSubBlock.ReadFromFile(file);

                processedBytes += (group.size - 24);
            }
            else
            {
                Debug.LogError("Expected Interior Cell subblock group. Got " + group.groupLabelType + "\n");
                file.BaseStream.Position += (group.size - 24);
                processedBytes += (group.size - 24);
            }
        }
    }
}
