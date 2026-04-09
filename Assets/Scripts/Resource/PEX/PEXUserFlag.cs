using System;
using System.IO;
using UnityEngine;

public class PEXUserFlag
{
    public UInt16 nameIndex;
    public byte flagIndex;

    public void ReadFromFile(BinaryReader file)
    {
        nameIndex = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        flagIndex = file.ReadByte();
    }
}
