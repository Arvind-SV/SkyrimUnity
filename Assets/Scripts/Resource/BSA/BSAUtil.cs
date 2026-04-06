using System;
using System.IO;
using UnityEngine;

public static class BSAUtil
{
    // Reference : https://en.uesp.net/wiki/Oblivion_Mod:Hash_Calculation
    public static UInt64 GetHash(string name)
    {
        string newName = name.Replace('/', '\\');
        return GetHash(Path.ChangeExtension(newName, null), Path.GetExtension(newName));
    }

    public static UInt64 GetHash(string name, string ext)
    {
        string newName = name.ToLowerInvariant();
        string newExt = ext.ToLowerInvariant();

        byte[] hashBytes = new byte[]
        {
        (byte)(newName.Length == 0 ? '\0' : newName[^1]),
        (byte)(newName.Length < 3 ? '\0' : newName[^2]),
        (byte)newName.Length,
        (byte)newName[0]
        };

        UInt32 hash1 = BitConverter.ToUInt32(hashBytes, 0);
        switch (newExt)
        {
            case ".kf":
                hash1 |= 0x80;
                break;
            case ".nif":
                hash1 |= 0x8000;
                break;
            case ".dds":
                hash1 |= 0x8080;
                break;
            case ".wav":
                hash1 |= 0x80000000;
                break;
        }

        UInt32 hash2 = 0;
        for (int i = 1; i < newName.Length - 2; i++)
        {
            hash2 = hash2 * 0x1003f + (byte)newName[i];
        }

        UInt32 hash3 = 0;
        for (int i = 0; i < newExt.Length; i++)
        {
            hash3 = hash3 * 0x1003f + (byte)newExt[i];
        }

        return (((UInt64)(hash2 + hash3)) << 32) + hash1;
    }
}
