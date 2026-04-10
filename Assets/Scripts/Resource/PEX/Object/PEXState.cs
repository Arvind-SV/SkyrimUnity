using System;
using System.IO;
using UnityEngine;

public class PEXState
{
    public UInt16 name;
    public UInt16 numFunctions;
    public PEXNamedFunction[] functions;

    public void ReadFromFile(BinaryReader file)
    {
        name = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        
        numFunctions = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        if(numFunctions > 0)
        {
            functions = new PEXNamedFunction[numFunctions];

            for(int i = 0; i < numFunctions; i++)
            {
                functions[i] = new();
                functions[i].ReadFromFile(file);
            }
        }
    }
}
