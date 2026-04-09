using System;
using System.IO;
using UnityEngine;

public class PEXVariable
{
    public UInt16 name;
    public UInt16 typeName;
    public UInt32 userFlags;
    public byte type;
    public UInt16 indexData;
    public Int32 intData;
    public float floatData;
    public byte boolData;

    public void ReadFromFile(BinaryReader file)
    {
        name = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        typeName = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        userFlags = BinaryFileUtil.ReadUInt32FromFileBigEndian(file);
        type = file.ReadByte();

        if((type == 1) || (type == 2))
        {
            indexData = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        }
        else if(type == 3)
        {
            intData = BinaryFileUtil.ReadInt32FromFileBigEndian(file);
        }
        else if(type == 4)
        {
            floatData = BinaryFileUtil.ReadFloat32FromFileBigEndian(file);
        }
        else if(type == 5)
        {
            boolData = file.ReadByte();
        }
    }
}
