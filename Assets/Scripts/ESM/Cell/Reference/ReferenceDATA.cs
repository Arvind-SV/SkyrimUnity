using System.IO;
using UnityEngine;

public class ReferenceDATA
{
    public float[] position = new float[3];
    public float[] rotation = new float[3];

    public void ReadFromFile(BinaryReader file)
    {
        for(int i = 0; i < position.Length; i++)
        {
            position[i] = file.ReadSingle();
        }

        for (int i = 0; i < rotation.Length; i++)
        {
            rotation[i] = file.ReadSingle();
        }
    }
}
