using System;
using System.IO;
using UnityEngine;

public class PEXVariableType
{
    public string name;
    public string type;

    public void ReadFromFile(BinaryReader file, string[] stringTable)
    {
        name = PEXStringTableUtil.ReadFromStringTableUsingStringIndex(file, stringTable);
        type = PEXStringTableUtil.ReadFromStringTableUsingStringIndex(file, stringTable);
    }
}
