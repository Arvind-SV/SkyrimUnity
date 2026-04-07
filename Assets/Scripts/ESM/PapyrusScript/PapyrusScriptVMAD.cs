using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class PapyrusScriptVMAD
{
    public Int16 version;
    public Int16 objFormat;
    public UInt16 scriptCount;
    public List<PapyrusScriptData> scripts = new();

    public void ReadFromFile(BinaryReader file)
    {
        version = file.ReadInt16();
        objFormat = file.ReadInt16();
        scriptCount = file.ReadUInt16();

        for(int i = 0; i < scriptCount; i++)
        {
            PapyrusScriptData script = new();

            scripts.Add(script);
        }
    }
}
