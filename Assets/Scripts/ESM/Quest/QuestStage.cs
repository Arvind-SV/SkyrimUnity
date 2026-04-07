using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class QuestStage
{
    public Int16 index;
    public byte flags;
    public byte unknown;
    public List<QuestLogEntry> questLogEntries = new();

    public UInt32 ReadFromFile(BinaryReader file)
    {
        UInt32 processedBytes = 0;

        index = file.ReadInt16();
        flags = file.ReadByte();
        unknown = file.ReadByte();

        processedBytes += 4;

        string fieldType;
        UInt16 fieldSize;

        // Extract all attached quest log entries
        while (true)
        {
            fieldType = new(file.ReadChars(4));
            fieldSize = file.ReadUInt16();
            processedBytes += 6;

            // This is no longer a quest log entry. Break from loop and move file pointer back
            if(fieldType != "QSDT")
            {
                processedBytes -= 6;
                file.BaseStream.Position -= 6;
                break;
            }

            QuestLogEntry entry = new();
            processedBytes += entry.ReadFromFile(file);

            questLogEntries.Add(entry);
        }

        return processedBytes;
    }
}
