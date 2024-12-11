using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    public uint Subdivisions = 0;
    public Color Color = Color.white;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Icosphere icosphere = new Icosphere();

        for (int i = 0; i < Subdivisions; i++) icosphere.Subdivide();

        foreach (var mesh in icosphere.ToDualMeshTiles())
        {

            GameObject tile = new();
            MeshRenderer meshRenderer = tile.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            meshRenderer.sharedMaterial.color = Random.ColorHSV();

            MeshFilter meshFilter = tile.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            tile.transform.SetParent(gameObject.transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
