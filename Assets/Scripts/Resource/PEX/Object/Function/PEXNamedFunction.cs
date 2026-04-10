using System;
using System.IO;
using UnityEngine;

public class PEXNamedFunction
{
    public UInt16 functionName;
    public PEXFunction function = new();

    public void ReadFromFile(BinaryReader file)
    {
        functionName = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        function.ReadFromFile(file);
    }
}
