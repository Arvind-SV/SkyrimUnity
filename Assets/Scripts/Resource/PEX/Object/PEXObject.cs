using System;
using System.IO;
using UnityEngine;

public class PEXObject
{
    public UInt16 nameIndex;
    public UInt32 size;
    public UInt16 parentClassName;
    public UInt16 docString;
    public UInt32 userFlags;
    public UInt16 autoStateName;
    public UInt16 numVariables;
    public PEXVariable[] variables;
    public UInt16 numProperties;
    public PEXProperty[] properties;
    public UInt16 numStates;
    public PEXState[] states;

    public void ReadFromFile(BinaryReader file)
    {
        nameIndex = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        size = BinaryFileUtil.ReadUInt32FromFileBigEndian(file);
        parentClassName = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        docString = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        userFlags = BinaryFileUtil.ReadUInt32FromFileBigEndian(file);
        autoStateName = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        numVariables = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);

        if(numVariables > 0)
        {
            variables = new PEXVariable[numVariables];

            for(int i = 0; i < numVariables; i++)
            {
                variables[i] = new();
                variables[i].ReadFromFile(file);
            }
        }

        numProperties = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);

        if(numProperties > 0)
        {
            properties = new PEXProperty[numProperties];

            for(int i = 0; i < numProperties; i++)
            {
                properties[i] = new();
                properties[i].ReadFromFile(file);
            }
        }

        numStates = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        if(numStates > 0)
        {
            states = new PEXState[numStates];

            for(int i = 0; i < numStates; i++)
            {
                states[i] = new();
                states[i].ReadFromFile(file);
            }
        }
    }
}
