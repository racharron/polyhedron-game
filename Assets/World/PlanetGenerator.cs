using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    public uint Subdivisions = 0;
    public Color Color = Color.white;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        meshRenderer.sharedMaterial.color = Color;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Icosphere icosphere = new Icosphere();

        for (int i = 0; i < Subdivisions; i++) icosphere.Subdivide();

        meshFilter.mesh = icosphere.ToDualMesh();
        meshFilter.mesh.name = "Test Goldenberg Polyhedron (" + Subdivisions + ")";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
