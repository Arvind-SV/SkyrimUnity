using System;
using System.IO;
using UnityEngine;

public class PEXFileData
{
    public UInt32 magic;
    public byte majorVersion;
    public byte minorVersion;
    public UInt16 gameID;
    public UInt64 compilationTime;
    public string sourceFileName;
    public string username;
    public string machinename;
    public UInt16 stringTableCount;
    public string[] stringTable;
    public byte hasDebugInfo;
    public PEXDebugInfo debugInfo;
    public UInt16 userFlagCount;
    public PEXUserFlag[] userFlags;
    public UInt16 objectCount;
    public PEXObject[] objects;

    public void ReadFromFile(BinaryReader file)
    {
        magic = BinaryFileUtil.ReadUInt32FromFileBigEndian(file);
        majorVersion = file.ReadByte();
        minorVersion = file.ReadByte();
        gameID = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        compilationTime = BinaryFileUtil.ReadUInt64FromFileBigEndian(file);
        sourceFileName = StringUtil.ReadWStringFromFileBigEndian(file);
        username = StringUtil.ReadWStringFromFileBigEndian(file);
        machinename = StringUtil.ReadWStringFromFileBigEndian(file);
        stringTableCount = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);

        if(stringTableCount > 0)
        {
            stringTable = new string[stringTableCount];

            for(int i = 0; i < stringTableCount; i++)
            {
                stringTable[i] = StringUtil.ReadWStringFromFileBigEndian(file);
            }
        }

        hasDebugInfo = file.ReadByte();
        if(hasDebugInfo > 0)
        {
            debugInfo = new();
            debugInfo.ReadFromFile(file);
        }

        userFlagCount = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);

        if(userFlagCount > 0)
        {
            userFlags = new PEXUserFlag[userFlagCount];

            for(int i = 0; i < userFlagCount; i++)
            {
                userFlags[i] = new();
                userFlags[i].ReadFromFile(file);
            }
        }

        objectCount = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);

        if(objectCount > 0)
        {
            if(objectCount > 1)
            {
                Debug.LogError("More than one object found in script file. Currently unhandled\n");
            }

            objects = new PEXObject[objectCount];

            for(int i = 0; i < objectCount ; i++)
            {
                objects[i] = new();
                objects[i].ReadFromFile(file, stringTable);
            }
        }
    }

    public PEXFunction GetFunctionByName(string functionName)
    {
        PEXFunction function = null;

        for(int i = 0; i < objectCount; i++)
        {
            PEXObject obj = objects[i];
            function = obj.GetFunctionByName(functionName);

            if(function != null)
            {
                break;
            }
        }

        return function;
    }

    public PEXVariable GetVariableByName(string name)
    {
        PEXVariable var = null;

        for (int i = 0; i < objectCount; i++)
        {
            PEXObject obj = objects[i];

            if(obj.variables.ContainsKey(name))
            {
                var = obj.variables[name];
                break;
            }
        }
            
        return var;
    }

    public bool IsProperty(string name)
    {
        bool isProperty = false;

        // all variables start with ::, and end with _var
        string actualName = name.Substring(2, name.Length - 6);
        Debug.Log("Actual name of argument " + actualName + "\n");

        for (int i = 0; i < objectCount; i++)
        {
            PEXObject obj = objects[i];

            if (obj.properties.ContainsKey(actualName))
            {
                isProperty = true;
                break;
            }
        }

        return isProperty;
    }
}
