using System;
using System.IO;
using UnityEngine;

public class PEXVariableType
{
    public UInt16 name;
    public UInt16 type;

    public void ReadFromFile(BinaryReader file)
    {
        name = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        type = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
    }
}
