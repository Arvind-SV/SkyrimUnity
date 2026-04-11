using System;
using System.IO;
using UnityEngine;

public class PEXNamedFunction
{
    public string functionName;
    public PEXFunction function = new();

    public void ReadFromFile(BinaryReader file, string[] stringTable)
    {
        functionName = PEXStringTableUtil.ReadFromStringTableUsingStringIndex(file, stringTable);
        function.ReadFromFile(file, stringTable);
    }
}
