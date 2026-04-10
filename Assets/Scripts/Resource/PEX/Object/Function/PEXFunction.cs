using System;
using System.IO;
using UnityEngine;

public class PEXFunction
{
    public UInt16 returnType;
    public UInt16 docString;
    public UInt32 userFlags;
    public byte flags;
    public UInt16 numParams;
    public PEXVariableType[] parameters;
    public UInt16 numLocals;
    public PEXVariableType[] locals;
    public UInt16 numInstructions;
    public PEXInstruction[] instructions;

    public void ReadFromFile(BinaryReader file)
    {
        returnType = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        docString = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        userFlags = BinaryFileUtil.ReadUInt32FromFileBigEndian(file);
        flags = file.ReadByte();

        numParams = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        if(numParams > 0)
        {
            parameters = new PEXVariableType[numParams];

            for(int i = 0; i < numParams; i++)
            {
                parameters[i] = new();
                parameters[i].ReadFromFile(file);
            }
        }

        numLocals = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        if (numLocals > 0)
        {
            locals = new PEXVariableType[numLocals];

            for (int i = 0; i < numLocals; i++)
            {
                locals[i] = new();
                locals[i].ReadFromFile(file);
            }
        }

        numInstructions = BinaryFileUtil.ReadUInt16FromFileBigEndian(file);
        if(numInstructions > 0)
        {
            instructions = new PEXInstruction[numInstructions];

            for(int i = 0; i < numInstructions; i++)
            {
                instructions[i] = new();
                instructions[i].ReadFromFile(file);
            }
        }
    }
}
