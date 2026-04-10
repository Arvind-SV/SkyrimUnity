using System;
using System.IO;
using UnityEngine;

public class PEXVariable
{
    public UInt16 name;
    public UInt16 typeName;
    public UInt32 userFlags;
    public PEXVariableData data = new();
    public void ReadFromFile(BinaryReader file)
    {
        name = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        typeName = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        userFlags = BinaryFileUtil.ReadUInt32FromFileBigEndian(file);
        data.ReadFromFile(file);
    }
}
