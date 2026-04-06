using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class BSAManager
{
    public static List<string> bsaList = new();

    public static void RegisterBSAs(string[] registeredBSAList)
    {
        foreach(string bsa in registeredBSAList)
        {
            bsaList.Add(SkyrimUnity.skyrimDataFilesPath + bsa);
        }
    }

    public static BinaryReader LoadFileFromBSA(string filePath)
    {
        BinaryReader file = null;

        // Processing BSAs in reverse, as later loaded BSA has higher priority
        List<string> temp = bsaList;
        temp.Reverse();

        foreach(string bsa in temp)
        {
            file = BSAReader.LoadFile(bsa, filePath);

            if(file != null)
            {
                // File found, no need to go through rest of the BSAs
                break;
            }
        }
        return file;
    }
}
