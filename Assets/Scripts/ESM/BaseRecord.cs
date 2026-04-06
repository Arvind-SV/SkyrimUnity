using System;
using System.IO;
using UnityEngine;

public class BaseRecord
{
    public string type;
    public UInt32 size;
    public UInt32 flags;
    public UInt32 recordFormID;
    public UInt16 timestamp;
    public UInt16 versionControlInfo;
    public UInt16 internalVersion;
    public UInt16 unknown;

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

        return processedBytes;
    }
}
