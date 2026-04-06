using System;
using System.IO;
using UnityEngine;

public static class StringUtil
{
    public static string ReadBZStringFromFile(BinaryReader file)
    {
        byte strSize = file.ReadByte();

        string str = new(file.ReadChars((int)strSize - 1)); // Ignoring the null terminator
        file.BaseStream.Position += 1; // Move past the null terminator
        return str;
    }

    public static string ReadBStringFromFile(BinaryReader file)
    {
        byte strSize = file.ReadByte();

        string str = new(file.ReadChars((int)strSize));
        return str;
    }

    // String Size given
    public static string ReadZStringFromFile(BinaryReader file, UInt32 strSize)
    {
        string str = new(file.ReadChars((int)strSize - 1)); // Ignoring the null terminator
        file.BaseStream.Position += 1; // Move past the null terminator
        return str;
    }

    // String Size not given
    public static string ReadZStringFromFile(BinaryReader file)
    {
        string str = new("");

        // Keep reading characters(and adding to string) until null terminator is found
        while(true)
        {
            char chr = file.ReadChar();

            if(chr == '\0')
            {
                break;
            }
            str += chr;
        }

        return str;
    }

    public static string ReadLStringFromFile(BinaryReader file, UInt32 strSize)
    {
        string str = null;

        if(!SkyrimUnity.engine.esmData.ContainsLocalizedStrings())
        {
            str = ReadZStringFromFile(file, strSize);
        }
        else
        {
            UInt32 strId = file.ReadUInt32();

            if(SkyrimUnity.engine.esmData.localizedStringTable.ContainsKey(strId))
            {
                str = SkyrimUnity.engine.esmData.localizedStringTable[strId];
            }
            else
            {
                Debug.LogError("String with id " + strId + " not found in string table!\n");
            }
        }

        return str;
    }
}
