using System;
using System.IO;
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

    public bool IsConditionFulfilled()
    {
        bool passed = false;

        float comparisonValue = 0.0f;

        if ((conditionFlags & (byte)CommonESMDefines.ConditionOperatorFlags.UseGlobal) == 0)
        {
            comparisonValue = this.comparisonValue;
        }

        float currentValue = -1.0f;

        if(functionIndex == (UInt16)CommonESMDefines.FunctionIndex.GetGlobalValue)
        {
            // param1 is formID of global variable
            currentValue = SkyrimUnity.engine.GetGlobalValue(param1);
        }

        if(conditionType == 0)
        {
            // Equal to
            if(currentValue == comparisonValue)
            {
                passed = true;
            }
        }
        else if (conditionType == 1)
        {
            // Not Equal to
            if (currentValue != comparisonValue)
            {
                passed = true;
            }
        }
        else if (conditionType == 2)
        {
            // Greater than
            if (currentValue > comparisonValue)
            {
                passed = true;
            }
        }
        else if (conditionType == 3)
        {
            // Greater than or Equal to
            if (currentValue >= comparisonValue)
            {
                passed = true;
            }
        }
        else if (conditionType == 4)
        {
            // Less than
            if (currentValue < comparisonValue)
            {
                passed = true;
            }
        }
        else if (conditionType == 5)
        {
            // Less than or Equal to
            if (currentValue <= comparisonValue)
            {
                passed = true;
            }
        }

        return passed;
    }
}
