using System;
using System.IO;
using UnityEngine;

public class PapyrusScriptPropertyObject
{
    public UInt32 formID;
    public Int16 alias;
    public UInt16 unused;

    public UInt32 ReadFromFile(BinaryReader file, Int16 objFormat)
    {
        UInt32 processedBytes = 0;

        if(objFormat == 1)
        {
            formID = file.ReadUInt32();
            alias = file.ReadInt16();
            unused = file.ReadUInt16();
        }
        else
        {
            unused = file.ReadUInt16();
            alias = file.ReadInt16();
            formID = file.ReadUInt32();
        }

        processedBytes += 8;

        return processedBytes;
    }
}
