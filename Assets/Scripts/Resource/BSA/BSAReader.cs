using System;
using System.IO;
using UnityEngine;

public static class BSAReader
{
    public static BinaryReader LoadFile(string bsaPath, string filePath)
    {
        BinaryReader file = null;

        using BinaryReader bsaFile = new(File.Open(bsaPath, FileMode.Open));

        BSAHeader header = new();
        header.ReadFromFile(bsaFile);

        UInt64 folderHash = BSAUtil.GetHash(Path.GetDirectoryName(filePath));

        BSAFolderRecord folderRecord = new();

        bool folderFound = false;

        for(int i = 0; i < header.folderCount; i++)
        {
            folderRecord = new();
            folderRecord.ReadFromFile(bsaFile);

            if(folderHash == folderRecord.nameHash)
            {
                // Folder found in BSA
                folderFound = true;
                break;
            }
        }

        if(folderFound)
        {
            // Moving BSA file pointer to position of file records for this folder
            bsaFile.BaseStream.Position = folderRecord.offset - header.totalFileNameLength;

            if(header.HasDirectoryNames())
            {
                // Folder name is present before file records(unused)
                string folderName = StringUtil.ReadBZStringFromFile(bsaFile);
            }

            UInt64 fileHash = BSAUtil.GetHash(Path.GetFileName(filePath));

            BSAFileRecord fileRecord = new();
            bool fileFound = false;

            for(int i = 0; i < folderRecord.count ; i++)
            {
                fileRecord = new();
                fileRecord.ReadFromFile(bsaFile);

                if(fileHash == fileRecord.nameHash)
                {
                    // File found in BSA.
                    fileFound = true;
                    break;
                }
            }

            if(fileFound)
            {
                // Setting BSA file pointer to location of raw file data
                bsaFile.BaseStream.Position = fileRecord.offset;

                UInt32 fileDataSize = fileRecord.size;

                if(header.IsFileCompressed(fileDataSize))
                {
                    Debug.LogError("File " + filePath + " is compressed! Currently not supported\n");
                }
                else
                {
                    if(header.AreFileNamesEmbedded())
                    {
                        // File name is present before file data(unused)
                        string fileName = StringUtil.ReadBStringFromFile(bsaFile);
                    }

                    // Read file data
                    file = new(new MemoryStream(bsaFile.ReadBytes((int)fileDataSize)));
                }
            }
        }

        bsaFile.Close();
        return file;
    }
}
