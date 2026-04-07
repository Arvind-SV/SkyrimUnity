using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Condition
{
    public byte conditionType;
    public byte conditionFlags;

    public byte[] unknown = new byte[3];
    public UInt32 comparisonFormID;
    public float comparisonValue;
    public UInt16 functionIndex;
    public byte[] padding = new byte[2];
    public UInt32 param1;
    public UInt32 param2;

    public void ReadFromFile(BinaryReader file, UInt32 numBytes)
    {
        byte operatorData = file.ReadByte();

        // Upper 3 bits, condition type
        conditionType = (byte)(operatorData >> 5);
        // Lower 5 bits, condition flags
        conditionFlags = (byte)(operatorData & 0x1F);

        unknown = file.ReadBytes(unknown.Length);

        if((conditionFlags & (byte)CommonESMDefines.ConditionOperatorFlags.UseGlobal) > 0)
        {
            comparisonFormID = file.ReadUInt32();
        }
        else
        {
            comparisonValue = file.ReadSingle();
        }

        functionIndex = file.ReadUInt16();
        padding = file.ReadBytes(padding.Length);

        UInt32 processedBytes = 12;

        if (functionIndex == (UInt16)CommonESMDefines.FunctionIndex.GetEventData)
        {
            // Currently not supported
        }
        else
        {
            param1 = file.ReadUInt32();
            param2 = file.ReadUInt32();

            processedBytes += 8;
        }

        file.BaseStream.Position += (numBytes - processedBytes);
    }
}
