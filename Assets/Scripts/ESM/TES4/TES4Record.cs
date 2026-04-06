using System;
using System.IO;
using UnityEngine;

public class TES4Record : BaseRecord
{
    public bool isLocalized;

    public override UInt32 ReadFromFile(BinaryReader file)
    {
        base.ReadFromFile(file);

        if((flags & (UInt32)CommonESMDefines.RecordFlags.Localized) > 0)
        {
            isLocalized = true;
        }
        else
        {
            isLocalized = false;
        }

        // Nothing needed from TES4 record as of now
        file.BaseStream.Position += size;

        UInt32 processedBytes = size;

        return processedBytes;
    }
}
