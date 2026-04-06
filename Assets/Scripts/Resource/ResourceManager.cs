using System.IO;
using UnityEngine;

public static class ResourceManager
{
    // Loads files(either from loose files or from BSA). Doesnt handle closing of BSA files(caller should do it after using)
    public static BinaryReader LoadFile(string filePath)
    {
        BinaryReader file;

        string fullFilePath = SkyrimUnity.skyrimDataFilesPath + filePath;

        if(File.Exists(fullFilePath))
        {
            file = new(File.Open(fullFilePath, FileMode.Open));
        }
        else
        {
            file = BSAManager.LoadFileFromBSA(filePath);
        }

        if(file == null)
        {
            Debug.LogError("File " + filePath + " not found!\n");
        }

        return file;
    }
}
