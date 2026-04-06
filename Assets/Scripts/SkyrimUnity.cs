using UnityEngine;

public static class SkyrimUnity
{
    public static string skyrimDataFilesPath = new("Assets/Data/");

    // Taken from Skyrim.ini
    public static string[] registeredBSAList = new string[]
    {
        "Skyrim - Misc.bsa",
        "Skyrim - Shaders.bsa",
        "Skyrim - Interface.bsa",
        "Skyrim - Animations.bsa",
        "Skyrim - Meshes0.bsa",
        "Skyrim - Meshes1.bsa",
        "Skyrim - Sounds.bsa",
        "Skyrim - Voices_en0.bsa",
        "Skyrim - Textures0.bsa",
        "Skyrim - Textures1.bsa",
        "Skyrim - Textures2.bsa",
        "Skyrim - Textures3.bsa",
        "Skyrim - Textures4.bsa",
        "Skyrim - Textures5.bsa",
        "Skyrim - Textures6.bsa",
        "Skyrim - Textures7.bsa",
        "Skyrim - Textures8.bsa",
    };

    // Currently only working with Skyrim.esm, but can be expanded to support more files in the future
    public static string skyrimESM = new("Skyrim.esm");

    public static SkyrimEngine engine;

    [RuntimeInitializeOnLoadMethod]
    public static void StartSkyrim()
    {
        Debug.Log("Skyrim Unity initialized.\n");

        // Registering BSAs from Skyrim.ini
        BSAManager.RegisterBSAs(registeredBSAList);

        // Initializing Skyrim engine
        engine = new();
        engine.Initialize();
    }

    
}
