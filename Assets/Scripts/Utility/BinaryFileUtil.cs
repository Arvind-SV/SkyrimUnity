using System;
using System.IO;
using System.Linq;
using UnityEngine;

public static class BinaryFileUtil
{
    public static UInt32 ReadUInt32FromFileBigEndian(BinaryReader file)
    {
        byte[] arr = file.ReadBytes(4);

        UInt32 result = (UInt32)((arr[0] << 24) | (arr[1] << 16) | (arr[2] << 8) | (arr[3]));

        return result;
    }

    public static Int32 ReadInt32FromFileBigEndian(BinaryReader file)
    {
        byte[] arr = file.ReadBytes(4);

        Int32 result = (Int32)((arr[0] << 24) | (arr[1] << 16) | (arr[2] << 8) | (arr[3]));

        return result;
    }

    public static float ReadFloat32FromFileBigEndian(BinaryReader file)
    {
        byte[] arr = file.ReadBytes(4);
        arr = (byte[])arr.Reverse();

        float result = BitConverter.ToSingle(arr);

        return result;
    }

    public static UInt16 ReadUInt16FromFileBigEndian(BinaryReader file)
    {
        byte[] arr = file.ReadBytes(2);

        UInt16 result = (UInt16)((arr[0] << 8) | (arr[1]));

        return result;
    }

    public static UInt64 ReadUInt64FromFileBigEndian(BinaryReader file)
    {
        byte[] arr = file.ReadBytes(8);

        UInt64 result = (UInt64)((arr[0] << 56) | (arr[1] << 48) | (arr[2] << 40) | (arr[3] << 32) | (arr[4] << 24) | (arr[5] << 16) | (arr[6] << 8) | (arr[7]));

        return result;
    }
}
