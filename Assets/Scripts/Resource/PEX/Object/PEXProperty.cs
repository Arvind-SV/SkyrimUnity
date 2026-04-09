using System;
using System.IO;
using UnityEngine;

public class PEXProperty
{
    public UInt16 name;
    public UInt16 type;
    public UInt16 docString;
    public UInt32 userFlags;
    public byte flags;
    public UInt16 autoVarName;

    public void ReadFromFile(BinaryReader file)
    {
        name = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
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

        }
    }
}
