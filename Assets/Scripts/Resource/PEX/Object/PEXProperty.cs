using System;
using System.IO;
using UnityEngine;

public class PEXProperty
{
    public string name;
    public UInt16 type;
    public UInt16 docString;
    public UInt32 userFlags;
    public byte flags;
    public UInt16 autoVarName;
    public PEXFunction readHandler;
    public PEXFunction writeHandler;

    public void ReadFromFile(BinaryReader file, string[] stringTable)
    {
        name = PEXStringTableUtil.ReadFromStringTableUsingStringIndex(file, stringTable);
        type = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        docString = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        userFlags = BinaryFileUtil.ReadUInt32FromFileBigEndian(file);
        flags = file.ReadByte();

        if((flags & 4) != 0)
        {
            autoVarName = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        }

        if((flags & 5) == 1)
        {
            readHandler = new();
            readHandler.ReadFromFile(file);
        }

        if ((flags & 6) == 2)
        {
            writeHandler = new();
            writeHandler.ReadFromFile(file);
        }
    }
}
