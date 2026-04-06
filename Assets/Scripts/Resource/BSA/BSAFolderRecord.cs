using System;
using System.IO;
using UnityEngine;

public class BSAFolderRecord
{
    public UInt64 nameHash;
    public UInt32 count;
    public UInt32 padding;
    public UInt32 offset;

    public void ReadFromFile(BinaryReader file)
    {
        nameHash = file.ReadUInt64();
        count = file.ReadUInt32();
        padding = file.ReadUInt32();
        offset = file.ReadUInt32();
        // There are two separate paddings in folder record. Using same variable for both(as its not used anywhere)
        padding = file.ReadUInt32();
    }
}
