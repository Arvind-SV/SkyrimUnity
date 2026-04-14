using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ESMParser
{
    public TES4Record tes4Record;
    public QuestGroup quests;
    public GlobalGroup globalVariables;
    public ActorGroup actors;
    public StaticGroup staticObjects;
    public CellGroup interiorCells;

    public Dictionary<UInt32, string> localizedStringTable = new();

    public void ParseESMFile(string esmFilePath)
    {
        Debug.Log("Reading ESM file: " + esmFilePath + "\n");

        LoadAttachedBSA(esmFilePath);

        using BinaryReader esmFile = new(File.Open(esmFilePath, FileMode.Open));

        if (esmFile != null)
        {
            ParseESMFile(esmFile, esmFilePath);
        }
        else
        {
            Debug.LogError("Failed to read ESM file: " + esmFilePath + "\n");
        }

        esmFile.Close();
    }

    public void ParseESMFile(BinaryReader file, string esmFilePath)
    {
        // Each ESM file starts with a TES4 header record
        tes4Record = new();
        tes4Record.ReadFromFile(file);

        if(ContainsLocalizedStrings())
        {
            LoadLocalizedStringTable(esmFilePath);
        }

        while(file.BaseStream.Position < file.BaseStream.Length)
        {
            // Apart from TES4 record, every record is part of a toplevel group
            BaseGroup group = new();
            group.ReadFromFile(file);

            if (group.groupLabelType == (Int32)CommonESMDefines.GroupLabelType.Top)
            {
                string groupLabel = new(System.Text.Encoding.UTF8.GetChars(group.label));
                
                // Process the group and its records
                if(groupLabel == "QUST")
                {
                    // Group contains quest records
                    quests = new(group);
                    quests.ReadFromFile(file);
                }
                else if(groupLabel == "GLOB")
                {
                    // Group contains global variables
                    globalVariables = new(group);
                    globalVariables.ReadFromFile(file);
                }
                else if(groupLabel == "NPC_")
                {
                    // Group contains actor records(eg NPCs, or player character)
                    actors = new(group);
                    actors.ReadFromFile(file);
                }
                else if(groupLabel == "STAT")
                {
                    // Group contains static objects
                    staticObjects = new(group);
                    staticObjects.ReadFromFile(file);
                }
                else if(groupLabel == "CELL")
                {
                    // Group contains interior cells(along with references)
                    interiorCells = new(group);
                    interiorCells.ReadFromFile(file);
                }
                else
                {
                    file.BaseStream.Position += (group.size - 24);
                }
            }
            else
            {
                Debug.Log("Invalid group label type found " + group.groupLabelType + "\n");
                file.BaseStream.Position += (group.size - 24);
            }
        }
    }

    public bool ContainsLocalizedStrings()
    {
        return tes4Record.isLocalized;
    }

    public void LoadAttachedBSA(string esmFilePath)
    {
        string esmFileName = Path.GetFileNameWithoutExtension(esmFilePath);
        string bsaFileName = esmFileName + ".bsa";

        if (File.Exists(SkyrimUnity.skyrimDataFilesPath + bsaFileName))
        {
            BSAManager.RegisterBSAs(new string[] { bsaFileName });
        }
    }

    public void LoadLocalizedStringTable(string esmFilePath)
    {
        string esmFileName = Path.GetFileNameWithoutExtension(esmFilePath) + "_English";

        string[] extensions = new string[] { ".strings", ".dlstrings", ".ilstrings" };

        foreach(string ext in extensions)
        {
            string filePath = "Strings/" + esmFileName + ext;

            BinaryReader stringTableFile = ResourceManager.LoadFile(filePath);

            if(stringTableFile != null)
            {
                StringTable.UpdateStringTable(stringTableFile, localizedStringTable, ext == ".strings");
                stringTableFile.Close();
            }
        }
    }
}