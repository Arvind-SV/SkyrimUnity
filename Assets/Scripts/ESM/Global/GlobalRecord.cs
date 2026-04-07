using System;
using System.IO;
using UnityEngine;

public class GlobalRecord : BaseRecord
{
    public string EDID;
    public byte FNAM;
    public float FLTV;

    public GlobalRecord(BaseRecord baseRecord) : base(baseRecord)
    {

    }

    public override UInt32 ReadFromFile(BinaryReader file)
    {
        UInt32 processedBytes = 0;

        UInt32 numBytes = size;

        string fieldType;
        UInt16 fieldSize;

        while (processedBytes < numBytes)
        {
            // Each field starts with a 4 byte field type and 2 byte field size
            fieldType = new(file.ReadChars(4));
            fieldSize = file.ReadUInt16();
            processedBytes += 6;

            if(fieldType == "EDID")
            {
                EDID = StringUtil.ReadZStringFromFile(file, fieldSize);
            }
            else if(fieldType == "FNAM")
            {
                FNAM = file.ReadByte();
            }
            else if(fieldType == "FLTV")
            {
                FLTV = file.ReadSingle();
            }
            else
            {
                file.BaseStream.Position += fieldSize;
            }

            processedBytes += fieldSize;
        }

        return processedBytes;
    }
}
