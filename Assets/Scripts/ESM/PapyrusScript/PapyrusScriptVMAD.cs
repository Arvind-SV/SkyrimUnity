using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class PapyrusScriptVMAD
{
    public Int16 version;
    public Int16 objFormat;
    public UInt16 scriptCount;
    public Dictionary<string, PapyrusScriptData> scripts = new();
    // Fragment Data
    public byte unknown;
    public UInt16 fragmentCount;
    public string fileName;
    public List<PapyrusScriptFragment> fragments = new();

    public void ReadFromFile(BinaryReader file, string recordType, UInt32 numBytes)
    {
        UInt32 processedBytes = 0;

        version = file.ReadInt16();
        objFormat = file.ReadInt16();
        scriptCount = file.ReadUInt16();

        processedBytes += 6;

        if(version < 4)
        {
            Debug.Log("Quest has scripts with versions less than 4. Unsupported!\n");
            file.BaseStream.Position += (numBytes - processedBytes);
        }
        else
        {
            for (int i = 0; i < scriptCount; i++)
            {
                PapyrusScriptData script = new();
                processedBytes += script.ReadFromFile(file, objFormat);
                scripts[script.scriptName] = script;
            }

            if(processedBytes < numBytes)
            {
                // Script record has fragments. Process each fragment
                // Fragment data format depends on the particular record
                if(recordType == "QUST")
                {
                    unknown = file.ReadByte();
                    fragmentCount = file.ReadUInt16();
                    fileName = StringUtil.ReadWStringFromFile(file, out UInt32 temp);

                    for(int i = 0; i < fragmentCount; i++)
                    {
                        PapyrusScriptFragment fragment = new();
                        fragment.ReadFromFile(file);
                        fragments.Add(fragment);
                    }
                }
                else
                {
                    Debug.LogError("Unsupported record type for fragment " + recordType + "\n");
                }
            }
        }
    }
}
