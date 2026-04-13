using System;
using System.IO;
using UnityEngine;

public class TES4Record : BaseRecord
{
    public bool isLocalized;

    public new void ReadFromFile(BinaryReader file)
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

        recordData.Close();
        recordData = null;

    }
}
