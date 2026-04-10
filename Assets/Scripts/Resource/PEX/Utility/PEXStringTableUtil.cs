using System;
using System.IO;
using UnityEngine;

public static class PEXStringTableUtil
{
    public static string ReadFromStringTableUsingStringIndex(BinaryReader file, string[] stringTable)
    {
        UInt16 idx = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        string str = stringTable[idx];

        return str;
    }
}
