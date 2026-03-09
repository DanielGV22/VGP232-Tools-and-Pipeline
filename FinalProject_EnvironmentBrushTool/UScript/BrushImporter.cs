using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BrushPoint
{
    public float X;
    public float Z;
    public float RotationY;
    public float Scale;
}

[System.Serializable]
public class BrushConfig
{
    public string BrushName;
    public float Radius;
    public int Density;
    public string DistributionType;
    public bool RandomRotation;
    public bool RandomScale;
    public float MinScale;
    public float MaxScale;
    public int Seed;
    public List<BrushPoint> Points;
}

public class BrushImporter : MonoBehaviour
{
    public TextAsset jsonFile;
    public GameObject prefabToPaint;
    public Transform parentRoot;
    public Vector3 originOffset;

    [ContextMenu("Import Brush")]
    public void ImportBrush()
    {
        if (jsonFile == null)
        {
            Debug.LogWarning("Missing JSON file.");
            return;
        }

        if (prefabToPaint == null)
        {
            Debug.LogWarning("Missing prefabToPaint.");
            return;
        }

        BrushConfig data = JsonUtility.FromJson<BrushConfig>(jsonFile.text);

        if (data == null || data.Points == null)
        {
            Debug.LogWarning("Invalid brush JSON.");
            return;
        }

        foreach (BrushPoint point in data.Points)
        {
            Vector3 spawnPosition = originOffset + new Vector3(point.X, 0f, point.Z);
            Quaternion rotation = Quaternion.Euler(0f, point.RotationY, 0f);

            GameObject obj = Instantiate(prefabToPaint, spawnPosition, rotation, parentRoot);

            obj.transform.localScale = Vector3.one * point.Scale;
        }
    }
}