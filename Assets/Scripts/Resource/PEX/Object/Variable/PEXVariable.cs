using System;
using System.IO;
using UnityEngine;

public class PEXVariable
{
    public string name;
    public string typeName;
    public UInt32 userFlags;
    public PEXVariableData data = new();
    public void ReadFromFile(BinaryReader file, string[] stringTable)
    {
        name = PEXStringTableUtil.ReadFromStringTableUsingStringIndex(file, stringTable);
        typeName = PEXStringTableUtil.ReadFromStringTableUsingStringIndex(file, stringTable);
        userFlags = BinaryFileUtil.ReadUInt32FromFileBigEndian(file);
        data.ReadFromFile(file, stringTable);
    }
}
