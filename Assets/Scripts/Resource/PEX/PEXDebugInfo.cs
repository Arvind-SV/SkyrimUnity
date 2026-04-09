using System;
using System.IO;
using UnityEngine;

public class PEXDebugInfo
{
    public UInt64 modificationTime;
    public UInt16 functionCount;
    public PEXDebugFunction[] functions;

    public void ReadFromFile(BinaryReader file)
    {
        modificationTime = BinaryFileUtil.ReadUInt64FromFileBigEndian(file);
        functionCount = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);

        if(functionCount > 0)
        {
            functions = new PEXDebugFunction[functionCount];

            for(int i = 0; i < functionCount; i++)
            {
                PEXDebugFunction function = new();
                function.ReadFromFile(file);

                functions[i] = function;
            }
        }
    }
}
