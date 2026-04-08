using System;
using System.IO;
using UnityEngine;

public class PapyrusScriptFragment
{
    public UInt16 index;
    public Int16 unknown;
    public Int32 logEntry;
    public byte unknown1;
    public string scriptName;
    public string fragmentName;
    public void ReadFromFile(BinaryReader file)
    {
        index = file.ReadUInt16();
        unknown = file.ReadInt16();
        logEntry = file.ReadInt32();
        unknown1 = file.ReadByte();
        scriptName = StringUtil.ReadWStringFromFile(file, out UInt32 temp);
        fragmentName = StringUtil.ReadWStringFromFile(file, out temp);
    }
}
