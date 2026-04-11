using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PEXInstruction
{
    public byte op;
    public UInt32 numArguments;
    public byte hasVariableArguments;
    public List<PEXVariableData> arguments = new();

    public void ReadFromFile(BinaryReader file, string[] stringTable)
    {
        op = file.ReadByte();
        ReadArgsBasedOnOpCode(file, stringTable);
    }

    public void ReadArgsBasedOnOpCode(BinaryReader file, string[] stringTable)
    {
        hasVariableArguments = 0;

        // Determine number of arguments based on opcode
        switch(op)
        {
            case (byte)CommonPEXDefines.InstructionOpcodes.NOP: numArguments = 0; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.IADD: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.FADD: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.ISUB: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.FSUB: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.IMUL: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.FMUL: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.IDIV: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.FDIV: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.IMOD: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.NOT: numArguments = 2; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.INEG: numArguments = 2; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.FNEG: numArguments = 2; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.ASSIGN: numArguments = 2; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.CAST: numArguments = 2; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.CMP_EQ: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.CMP_LT: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.CMP_LE: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.CMP_GT: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.CMP_GE: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.JMP: numArguments = 1; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.JMPT: numArguments = 2; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.JMPF: numArguments = 2; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.CALLMETHOD: numArguments = 3; hasVariableArguments = 1; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.CALLPARENT: numArguments = 2; hasVariableArguments = 1; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.CALLSTATIC: numArguments = 3; hasVariableArguments = 1; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.RETURN: numArguments = 1; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.STRCAT: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.PROPGET: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.PROPSET: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.ARRAY_CREATE: numArguments = 2; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.ARRAY_LENGTH: numArguments = 2; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.ARRAY_GETELEMENT: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.ARRAY_SETELEMENT: numArguments = 3; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.ARRAY_FINDELEMENT: numArguments = 4; break;
            case (byte)CommonPEXDefines.InstructionOpcodes.ARRAY_RFINDELEMENT: numArguments = 4; break;
            default: numArguments = 0; break;
        }

        ReadArgsFromFile(file, stringTable);
    }

    public void ReadArgsFromFile(BinaryReader file, string[] stringTable)
    {
        for(int i = 0; i < numArguments; i++)
        {
            PEXVariableData arg = new();
            arg.ReadFromFile(file, stringTable);
            arguments.Add(arg);
        }

        if(hasVariableArguments > 0)
        {
            PEXVariableData numVarArgs = new();
            numVarArgs.ReadFromFile(file, stringTable);

            for(int i = 0; i < numVarArgs.intData; i++)
            {
                PEXVariableData arg = new();
                arg.ReadFromFile(file, stringTable);
                arguments.Add(arg);
            }
        }
    }
}
