using System;
using System.IO;
using UnityEngine;

public class PEXVariableData
{
    public byte type;
    public string stringData;
    public Int32 intData;
    public float floatData;
    public byte boolData;

    public void ReadFromFile(BinaryReader file, string[] stringTable)
    {
        type = file.ReadByte();

        if ((type == 1) || (type == 2))
        {
            stringData = PEXStringTableUtil.ReadFromStringTableUsingStringIndex(file, stringTable);
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

    public float GetValue()
    {
        float value = 0.0f;

        if(type == 3)
        {
            value = (float)intData;
        }

        return value;
    }
}
