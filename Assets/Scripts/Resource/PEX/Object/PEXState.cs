using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PEXState
{
    public UInt16 name;
    public UInt16 numFunctions;
    public Dictionary<string, PEXFunction> functions = new();

    public void ReadFromFile(BinaryReader file, string[] stringTable)
    {
        name = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        
        numFunctions = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        if(numFunctions > 0)
        {
            for(int i = 0; i < numFunctions; i++)
            {
                PEXNamedFunction function = new();
                function.ReadFromFile(file, stringTable);

                functions[function.functionName] = function.function;
            }
        }
    }
}
