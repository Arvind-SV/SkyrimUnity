using System;
using System.IO;
using UnityEngine;

public class ActorRecord : BaseRecord
{
    public string EDID;
    public string FULL;

    public ActorRecord(BaseRecord baseRecord) : base(baseRecord)
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

            if (fieldType == "EDID")
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

        recordData.Close();
        file.Close();
        recordData = null;
    }
}
