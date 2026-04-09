using System;
using System.IO;
using UnityEngine;

public class PEXDebugFunction
{
    public UInt16 objectNameIndex;
    public UInt16 stateNameIndex;
    public UInt16 functionNameIndex;
    public byte functionType;
    public UInt16 instructionCount;
    public UInt16[] lineNumbers;

    public void ReadFromFile(BinaryReader file)
    {
        objectNameIndex = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        stateNameIndex = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        functionNameIndex = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        functionType = file.ReadByte();

        instructionCount = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        if(instructionCount > 0)
        {
            lineNumbers = new UInt16[instructionCount];

            for(int i = 0; i < instructionCount; i++)
            {
                lineNumbers[i] = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
            }
        }
    }
}
