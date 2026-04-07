using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class QuestLogEntry
{
    public byte QSDT;
    public List<Condition> CTDA = new();

    public UInt32 ReadFromFile(BinaryReader file)
    {
        UInt32 processedBytes = 0;

        QSDT = file.ReadByte();
        processedBytes++;

        string fieldType;
        UInt16 fieldSize;

        while (true)
        {
            fieldType = new(file.ReadChars(4));
            fieldSize = file.ReadUInt16();
            processedBytes += 6;

            if(!Array.Exists(CommonESMDefines.questLogEntryFields, element => element == fieldType))
            {
                // Start of a new log entry, or end of last log entry
                processedBytes -= 6;
                file.BaseStream.Position -= 6;
                break;
            }

            if(fieldType == "CTDA")
            {
                Condition condition = new();
                condition.ReadFromFile(file, fieldSize);

                CTDA.Add(condition);
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
