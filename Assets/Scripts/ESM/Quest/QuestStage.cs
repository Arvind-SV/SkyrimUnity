using System;
using System.IO;
using UnityEngine;

public class QuestStage
{
    public Int16 index;
    public byte flags;
    public byte unknown;

    public void ReadFromFile(BinaryReader file)
    {
        index = file.ReadInt16();
        flags = file.ReadByte();
        unknown = file.ReadByte();
    }
}
