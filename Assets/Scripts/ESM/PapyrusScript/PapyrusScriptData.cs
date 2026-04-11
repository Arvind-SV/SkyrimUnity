using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PapyrusScriptData
{
    public string scriptName;
    public byte status;
    public UInt16 propertyCount;
    public Dictionary<string, PapyrusScriptProperty> properties = new();

    public UInt32 ReadFromFile(BinaryReader file, Int16 objFormat)
    {
        scriptName = StringUtil.ReadWStringFromFile(file, out UInt32 processedBytes);
        status = file.ReadByte();
        propertyCount = file.ReadUInt16();

        processedBytes += 3;

        for(int i = 0; i < propertyCount; i++)
        {
            PapyrusScriptProperty property = new();
            processedBytes += property.ReadFromFile(file, objFormat);
            properties[property.propertyName] = property;
        }

        return processedBytes;
    }

    public PapyrusScriptProperty GetProperty(string propertyName)
    {
        PapyrusScriptProperty prop = null;

        if(properties.ContainsKey(propertyName))
        {
            prop = properties[propertyName];
        }

        return prop;
    }
}
