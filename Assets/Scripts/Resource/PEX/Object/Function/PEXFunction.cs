using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PEXFunction
{
    public UInt16 returnType;
    public UInt16 docString;
    public UInt32 userFlags;
    public byte flags;
    public UInt16 numParams;
    public Dictionary<string, PEXVariableType> parameters = new();
    public UInt16 numLocals;
    public Dictionary<string, PEXVariableType> locals = new();
    public UInt16 numInstructions;
    public PEXInstruction[] instructions;

    public void ReadFromFile(BinaryReader file, string[] stringTable)
    {
        returnType = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        docString = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        userFlags = BinaryFileUtil.ReadUInt32FromFileBigEndian(file);
        flags = file.ReadByte();

        numParams = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        if(numParams > 0)
        {
            for(int i = 0; i < numParams; i++)
            {
                PEXVariableType parameter = new();
                parameter.ReadFromFile(file, stringTable);

                parameters[parameter.name] = parameter;
            }
        }

        numLocals = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        if (numLocals > 0)
        {
            for (int i = 0; i < numLocals; i++)
            {
                PEXVariableType local = new();
                local.ReadFromFile(file, stringTable);

                locals[local.name] = local;
            }
        }

        numInstructions = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        if(numInstructions > 0)
        {
            instructions = new PEXInstruction[numInstructions];

            for(int i = 0; i < numInstructions; i++)
            {
                instructions[i] = new();
                instructions[i].ReadFromFile(file, stringTable);
            }
        }
    }

    public bool IsLocalVariable(PEXVariableData variable)
    {
        bool isLocalVariable = false;

        if ((variable.type == 1) || (variable.type == 2))
        {
            if (locals.ContainsKey(variable.stringData))
            {
                isLocalVariable = true;
            }
        }

        return isLocalVariable;
    }
}
