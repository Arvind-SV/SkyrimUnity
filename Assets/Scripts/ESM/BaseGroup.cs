using System;
using System.IO;
using UnityEngine;

public class BaseGroup
{
    public char[] type = new char[4];
    public UInt32 size;
    public byte[] label = new byte[4];
    public Int32 groupLabelType;
    public UInt16 timestamp;
    public UInt16 versionControlInfo;
    public UInt32 unknown;

    public BaseGroup()
    {

    }

    public BaseGroup(BaseGroup baseGroup)
    {
        type = baseGroup.type;
        size = baseGroup.size;
        label = baseGroup.label;
        groupLabelType = baseGroup.groupLabelType;
        timestamp = baseGroup.timestamp;
        versionControlInfo = baseGroup.versionControlInfo;
        unknown = baseGroup.unknown;
    }

    public virtual void ReadFromFile(BinaryReader file)
    {
        type = file.ReadChars(type.Length);
        size = file.ReadUInt32();
        label = file.ReadBytes(label.Length);
        groupLabelType = file.ReadInt32();
        timestamp = file.ReadUInt16();
        versionControlInfo = file.ReadUInt16();
        unknown = file.ReadUInt32();
    }
}
