using UnityEngine;

public class SkyrimEngine
{
    public ESMParser esmData;

    public SkyrimEngine()
    {
        // Setting pointer in SkyrimUnity to use in other classes
        SkyrimUnity.engine = this;
    }

    public void Initialize()
    {
        // Read ESM files
        esmData = new();
        esmData.ParseESMFile(SkyrimUnity.skyrimDataFilesPath + SkyrimUnity.skyrimESM);
    }
}
