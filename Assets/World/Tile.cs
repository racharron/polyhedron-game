using UnityEngine;

/// <summary>
/// A tile of the world (the rendering side).
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class Tile : MonoBehaviour
{
    public Color baseColor;
    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = baseColor;
    }
}
