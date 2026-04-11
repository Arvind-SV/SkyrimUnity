using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PEXObject
{
    public UInt16 nameIndex;
    public UInt32 size;
    public string parentClassName;
    public UInt16 docString;
    public UInt32 userFlags;
    public UInt16 autoStateName;
    public UInt16 numVariables;
    public Dictionary<string, PEXVariable> variables = new();
    public UInt16 numProperties;
    public Dictionary<string, PEXProperty> properties = new();
    public UInt16 numStates;
    public PEXState[] states;

    public void ReadFromFile(BinaryReader file, string[] stringTable)
    {
        nameIndex = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        size = BinaryFileUtil.ReadUInt32FromFileBigEndian(file);
        parentClassName = PEXStringTableUtil.ReadFromStringTableUsingStringIndex(file, stringTable);
        docString = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        userFlags = BinaryFileUtil.ReadUInt32FromFileBigEndian(file);
        autoStateName = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        numVariables = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);

        if(numVariables > 0)
        {
            for(int i = 0; i < numVariables; i++)
            {
                PEXVariable variable = new();
                variable.ReadFromFile(file, stringTable);
                variables[variable.name] = variable;
            }
        }

        numProperties = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);

        if(numProperties > 0)
        {
            for(int i = 0; i < numProperties; i++)
            {
                PEXProperty property = new();
                property.ReadFromFile(file, stringTable);
                properties[property.name] = property;
            }
        }

        numStates = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        if(numStates > 0)
        {
            states = new PEXState[numStates];

            for(int i = 0; i < numStates; i++)
            {
                states[i] = new();
                states[i].ReadFromFile(file, stringTable);
            }
        }
    }

    public PEXFunction GetFunctionByName(string functionName)
    {
        PEXFunction function = null;

        for(int i = 0; i < numStates; i++)
        {
            PEXState state = states[i];

            if(state.functions.ContainsKey(functionName))
            {
                function = state.functions[functionName];
                break;
            }
        }

        return function;
    }
}
