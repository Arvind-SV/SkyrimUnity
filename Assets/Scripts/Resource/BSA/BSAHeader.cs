using System;
using System.IO;
using UnityEngine;

public class BSAHeader
{
    public char[] fileID = new char[4];
    public UInt32 version;
    public UInt32 offset;
    public UInt32 archiveFlags;
    public UInt32 folderCount;
    public UInt32 fileCount;
    public UInt32 totalFolderNameLength;
    public UInt32 totalFileNameLength;
    public UInt16 fileFlags;
    public UInt16 padding;

    public void ReadFromFile(BinaryReader file)
    {
        fileID = file.ReadChars(fileID.Length);
        version = file.ReadUInt32();
        offset = file.ReadUInt32();
        archiveFlags = file.ReadUInt32();
        folderCount = file.ReadUInt32();
        fileCount = file.ReadUInt32();
        totalFolderNameLength = file.ReadUInt32();
        totalFileNameLength = file.ReadUInt32();
        fileFlags = file.ReadUInt16();
        padding = file.ReadUInt16();
    }

    public bool HasDirectoryNames()
    {
        return ((archiveFlags & 0x1) > 0);
    }

    public bool IsFileCompressed(UInt32 fileDataSize)
    {
        bool isCompressed = false;

        if((archiveFlags & 0x4) > 0)
        {
            // File is compressed by default(unless bit 30 is set in fileDataSize)
            if((fileDataSize & 0x40000000) == 0)
            {
                isCompressed = true;
            }
        }
        else
        {
            // File is not compressed by default(unless bit 30 is set in fileDataSize)
            if ((fileDataSize & 0x40000000) > 0)
            {
                isCompressed = true;
            }
        }

        return isCompressed;
    }

    public bool AreFileNamesEmbedded()
    {
        return ((archiveFlags & 0x100) > 0);
    }
}
