using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    public uint Bisections = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Icosphere icosphere = new(Bisections);

        List<GameObject> worldFacets = new(icosphere.vertices.Length);
        var dual = icosphere.Dual();
        for (int i = 0; i < dual.Length; i++)
        {

            GameObject worldFacet = new() { name = "World Tile " + i, layer = LayerMask.NameToLayer("UI") };
            MeshRenderer meshRenderer = worldFacet.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

            MeshFilter meshFilter = worldFacet.AddComponent<MeshFilter>();
            Mesh mesh = dual[i].ToMesh();
            meshFilter.mesh = mesh;
            worldFacet.AddComponent<Tile>();
            worldFacet.AddComponent<MeshCollider>().sharedMesh = mesh;
            worldFacet.transform.SetParent(gameObject.transform, false);
            GameObject tileBase = new() { name = "Tile Base " + i };
            tileBase.transform.SetPositionAndRotation(
                dual[i].Centroid,
                Quaternion.LookRotation(Vector3.Cross(dual[i].Normal, dual[i][0] - dual[i].Centroid), dual[i].Normal)
            );
            tileBase.transform.SetParent(worldFacet.transform, false);
            
            worldFacets.Add(worldFacet);
        }
        Assert.AreEqual(worldFacets.Count, icosphere.vertices.Length);
        for (int i = 0; i < icosphere.vertices.Length; i++)
        {
            worldFacets[i].GetComponent<Tile>().neighbors = icosphere.vertices[i].Neighbors.Select(n => worldFacets[n]).ToArray();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
