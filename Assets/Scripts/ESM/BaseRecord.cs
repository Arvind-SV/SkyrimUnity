using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public class BaseRecord
{
    public string type;
    public UInt32 size; // Decompressed data size
    public UInt32 flags;
    public UInt32 recordFormID;
    public UInt16 timestamp;
    public UInt16 versionControlInfo;
    public UInt16 internalVersion;
    public UInt16 unknown;

    public UInt32 compressedDataSize; // same as decompressed data size if data is not compressed
    public BinaryReader recordData;

    public BaseRecord()
    {

    }

    public BaseRecord(BaseRecord baseRecord)
    {
        type = baseRecord.type;
        size = baseRecord.size;
        flags = baseRecord.flags;
        recordFormID = baseRecord.recordFormID;
        timestamp = baseRecord.timestamp;
        versionControlInfo = baseRecord.versionControlInfo;
        internalVersion = baseRecord.internalVersion;
        unknown = baseRecord.unknown;

        recordData = baseRecord.recordData;
    }

    public virtual UInt32 ReadFromFile(BinaryReader file)
    {
        UInt32 processedBytes;

        type = new(file.ReadChars(4));
        size = file.ReadUInt32();
        flags = file.ReadUInt32();
        recordFormID = file.ReadUInt32();
        timestamp = file.ReadUInt16();
        versionControlInfo = file.ReadUInt16();
        internalVersion = file.ReadUInt16();
        unknown = file.ReadUInt16();

        processedBytes = 24;

        if (!IsDataCompressed())
        {
            recordData = new(new MemoryStream(file.ReadBytes((int)size)));
            compressedDataSize = size;
        }
        else
        {
            // Compressed data has 4 bytes at the beginning that mentions decompressed data size
            UInt32 decompressedDataSize = file.ReadUInt32();
            
            processedBytes += 4;

            compressedDataSize = size - 4;

            // The first two bytes of compressed data is ZLIB header which is not allowed by DeflateStream
            file.BaseStream.Position += 2;

            // 4 bytes for decompressed data size and 2 bytes for ZLIB header
            size -= 6;

            DeflateStream decompressedData = new(new MemoryStream(file.ReadBytes((int)(size))), CompressionMode.Decompress);

            MemoryStream decompressedCopy = new();
            decompressedData.CopyTo(decompressedCopy);

            size = decompressedDataSize;

            recordData = new(decompressedCopy);
            recordData.BaseStream.Position = 0;

            decompressedData.Close();
        }

        return processedBytes;
    }

    public bool IsDataCompressed()
    {
        return ((flags & 0x00040000) > 0);
    }

    public virtual PapyrusScriptProperty GetScriptProperty(string scriptName, string propertyName)
    {
        PapyrusScriptProperty property = null;

        if(type == "QUST")
        {
            property = ((QuestRecord)this).GetScriptProperty(scriptName, propertyName);
        }

        return property;
    }

    public virtual string GetObjectID()
    {
        string id = "";

        if (type == "QUST")
        {
            id = ((QuestRecord)this).GetObjectID();
        }

        return id;
    }
}
