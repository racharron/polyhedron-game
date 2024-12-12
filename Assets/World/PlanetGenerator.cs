using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
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

        List<GameObject> tiles = new(icosphere.vertices.Length);
        foreach (var mesh in icosphere.ToDualMeshTiles())
        {

            GameObject childObject = new();
            childObject.name = "World Tile";
            MeshRenderer meshRenderer = childObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            meshRenderer.sharedMaterial.color = Random.ColorHSV();

            MeshFilter meshFilter = childObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            childObject.AddComponent<Tile>();
            childObject.transform.SetParent(gameObject.transform, false);
            tiles.Add(childObject);
        }
        Assert.AreEqual(tiles.Count, icosphere.vertices.Length);
        for (int i = 0; i < icosphere.vertices.Length; i++)
        {
            tiles[i].GetComponent<Tile>().neighbors = icosphere.vertices[i].Neighbors.Select(n => tiles[n]).ToArray();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
