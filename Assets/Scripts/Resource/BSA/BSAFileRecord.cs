using System;
using System.IO;
using UnityEngine;

public class BSAFileRecord
{
    public UInt64 nameHash;
    public UInt32 size;
    public UInt32 offset;

    public void ReadFromFile(BinaryReader file)
    {
        nameHash = file.ReadUInt64();
        size = file.ReadUInt32();
        offset = file.ReadUInt32();
    }
}
