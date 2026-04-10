using System;
using System.IO;
using UnityEngine;

public class PEXVariableData
{
    public byte type;
    public UInt16 indexData;
    public Int32 intData;
    public float floatData;
    public byte boolData;

    public void ReadFromFile(BinaryReader file)
    {
        type = file.ReadByte();

        if ((type == 1) || (type == 2))
        {
            indexData = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        }
        else if (type == 3)
        {
            intData = BinaryFileUtil.ReadInt32FromFileBigEndian(file);
        }
        else if (type == 4)
        {
            floatData = BinaryFileUtil.ReadFloat32FromFileBigEndian(file);
        }
        else if (type == 5)
        {
            boolData = file.ReadByte();
        }
    }
}
