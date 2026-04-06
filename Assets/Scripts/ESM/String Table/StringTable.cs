using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class StringTable
{
    public static void UpdateStringTable(BinaryReader file, Dictionary<UInt32, string> stringTable, bool isExtStrings)
    {
        UInt32 numEntries = file.ReadUInt32();
        UInt32 strDataSize = file.ReadUInt32(); // Unused

        UInt32 directoryEntrySize = 8; // 4 bytes for string ID, 4 bytes for offset

        // Raw string data begins after header and all directory entries(header is numEntries + strDataSize)
        UInt32 stringDataBegin = 8 + (numEntries * directoryEntrySize);

        UInt32 stringID, offset;

        Int64 currentFilePosition;

        
        for(int i = 0; i < numEntries; i++)
        {
            stringID = file.ReadUInt32();
            offset = file.ReadUInt32();

            // Storing current position to come back later
            currentFilePosition = file.BaseStream.Position;

            // Moving pointer to position of string entry
            file.BaseStream.Position = stringDataBegin + offset;

            string str;

            if(!isExtStrings)
            {
                UInt32 strSize = file.ReadUInt32();
                try
                {
                    str = new(file.ReadChars((int)(strSize) - 1));
                    file.BaseStream.Position += 1;
                }
                catch(ArgumentException)
                {
                    // TODO : Handle in future properly
                    str = new("DEFAULT STRING");
                }
            }
            else
            {
                str = StringUtil.ReadZStringFromFile(file);
            }

            stringTable[stringID] = str;

            // Moving pointer back to stored position to read next string ID.
            file.BaseStream.Position = currentFilePosition;
        }
    }
}
