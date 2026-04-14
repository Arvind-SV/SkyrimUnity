using System;
using System.IO;
using UnityEngine;

public class ReferenceRecord : BaseRecord
{
    public UInt32 NAME;
    public float XSCL;
    public ReferenceDATA DATA;

    public ReferenceRecord(BaseRecord baseRecord) : base(baseRecord)
    {

    }

    public void ReadFromFile()
    {
        UInt32 processedBytes = 0;
        UInt32 numBytes = size;

        string fieldType;
        UInt16 fieldSize;

        BinaryReader file = recordData;

        while (processedBytes < numBytes)
        {
            // Each field starts with a 4 byte field type and 2 byte field size
            fieldType = new(file.ReadChars(4));
            fieldSize = file.ReadUInt16();
            processedBytes += 6;

            if (fieldType == "NAME")
            {
                NAME = file.ReadUInt32();
            }
            else if (fieldType == "XSCL")
            {
                XSCL = file.ReadSingle();
            }
            else if(fieldType == "DATA")
            {
                DATA = new();
                DATA.ReadFromFile(file);
            }
            else
            {
                file.BaseStream.Position += fieldSize;
            }

            processedBytes += fieldSize;
        }
    }
}
