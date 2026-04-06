using System;
using System.IO;
using UnityEngine;

public class QuestRecord : BaseRecord
{
    public string EDID;
    public string FULL;

    public QuestRecord(BaseRecord baseRecord) : base(baseRecord)
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
            else if(fieldType == "FULL")
            {
                FULL = StringUtil.ReadLStringFromFile(file, fieldSize);
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
