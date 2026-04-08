using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class PapyrusScriptProperty
{
    public string propertyName;
    public byte propertyType;
    public byte status;
    public UInt32 itemCount;
    public List<PapyrusScriptPropertyObject> propertyObjectValues = new();
    public List<string> propertyWStringValues = new();
    public List<Int32> propertyIntValues = new();
    public List<float> propertyFloatValues = new();
    public List<byte> propertyBoolValues = new();

    public UInt32 ReadFromFile(BinaryReader file, Int16 objFormat)
    {
        propertyName = StringUtil.ReadWStringFromFile(file, out UInt32 processedBytes);
        propertyType = file.ReadByte();
        status = file.ReadByte();

        processedBytes += 2;

        // Property data depends on type of property
        if(propertyType == (byte)CommonESMDefines.ScriptPropertyType.Object)
        {
            itemCount = 1;

            for (int i = 0; i < itemCount; i++)
            {
                PapyrusScriptPropertyObject propertyObject = new();
                processedBytes += propertyObject.ReadFromFile(file, objFormat);
                propertyObjectValues.Add(propertyObject);
            }
        }
        else if (propertyType == (byte)CommonESMDefines.ScriptPropertyType.ObjectArray)
        {
            itemCount = file.ReadUInt32();
            processedBytes += 4;

            for (int i = 0; i < itemCount; i++)
            {
                PapyrusScriptPropertyObject propertyObject = new();
                processedBytes += propertyObject.ReadFromFile(file, objFormat);
                propertyObjectValues.Add(propertyObject);
            }
        }
        else if (propertyType == (byte)CommonESMDefines.ScriptPropertyType.WString)
        {
            itemCount = 1;

            for (int i = 0; i < itemCount; i++)
            {
                string propertyWString = StringUtil.ReadWStringFromFile(file, out UInt32 temp);
                processedBytes += temp;
                propertyWStringValues.Add(propertyWString);
            }
        }
        else if (propertyType == (byte)CommonESMDefines.ScriptPropertyType.WStringArray)
        {
            itemCount = file.ReadUInt32();
            processedBytes += 4;

            for (int i = 0; i < itemCount; i++)
            {
                string propertyWString = StringUtil.ReadWStringFromFile(file, out UInt32 temp);
                processedBytes += temp;
                propertyWStringValues.Add(propertyWString);
            }
        }
        else if (propertyType == (byte)CommonESMDefines.ScriptPropertyType.Int)
        {
            itemCount = 1;

            for (int i = 0; i < itemCount; i++)
            {
                Int32 propertyInt = file.ReadInt32();
                processedBytes += 4;
                propertyIntValues.Add(propertyInt);
            }
        }
        else if (propertyType == (byte)CommonESMDefines.ScriptPropertyType.IntArray)
        {
            itemCount = file.ReadUInt32();
            processedBytes += 4;

            for (int i = 0; i < itemCount; i++)
            {
                Int32 propertyInt = file.ReadInt32();
                processedBytes += 4;
                propertyIntValues.Add(propertyInt);
            }
        }
        else if (propertyType == (byte)CommonESMDefines.ScriptPropertyType.Float)
        {
            itemCount = 1;

            for (int i = 0; i < itemCount; i++)
            {
                float propertyFloat = file.ReadSingle();
                processedBytes += 4;
                propertyFloatValues.Add(propertyFloat);
            }
        }
        else if (propertyType == (byte)CommonESMDefines.ScriptPropertyType.FloatArray)
        {
            itemCount = file.ReadUInt32();
            processedBytes += 4;

            for (int i = 0; i < itemCount; i++)
            {
                float propertyFloat = file.ReadSingle();
                processedBytes += 4;
                propertyFloatValues.Add(propertyFloat);
            }
        }
        else if (propertyType == (byte)CommonESMDefines.ScriptPropertyType.Bool)
        {
            itemCount = 1;

            for (int i = 0; i < itemCount; i++)
            {
                byte propertyBool = file.ReadByte();
                processedBytes += 1;
                propertyBoolValues.Add(propertyBool);
            }
        }
        else if (propertyType == (byte)CommonESMDefines.ScriptPropertyType.BoolArray)
        {
            itemCount = file.ReadUInt32();
            processedBytes += 4;

            for (int i = 0; i < itemCount; i++)
            {
                byte propertyBool = file.ReadByte();
                processedBytes += 1;
                propertyBoolValues.Add(propertyBool);
            }
        }
        else
        {
            Debug.Log("Unrecognized property type found : " + propertyType + "\n");
        }

        return processedBytes;
    }
}
