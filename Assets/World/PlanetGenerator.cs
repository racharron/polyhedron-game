using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlanetGenerator : MonoBehaviour
{
    public uint bisections = 0;
    public Material TileMaterial;

    public GameObject TileCanvas;

    static readonly float TILE_UI_OFFSET = 0.01f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Icosphere icosphere = new(bisections);

        List<GameObject> worldFacets = new(icosphere.vertices.Length);
        var dual = icosphere.Dual();
        for (int i = 0; i < dual.Length; i++)
        {

            GameObject worldFacet = new() { name = "World Tile " + i };
            
            worldFacet.AddComponent<MeshRenderer>().sharedMaterial = TileMaterial;
            MeshFilter meshFilter = worldFacet.AddComponent<MeshFilter>();
            Mesh mesh = dual[i].ToMesh();
            meshFilter.mesh = mesh;
            worldFacet.AddComponent<Tile>().baseColor = Random.ColorHSV(0, 1, 0.5f, 0.5f, 0.5f, 0.5f, 1, 1);
            worldFacet.AddComponent<MeshCollider>().sharedMesh = mesh;
            worldFacet.AddComponent<TileState>().state = new()
            {
                liquidity = Random.Range(1f, 10f),
                development = Random.Range(1f, 10f),
                infrastructure = Random.Range(1f, 10f),
                technology = Random.Range(1f, 10f)
            };
            worldFacet.transform.SetParent(gameObject.transform, false);
            var tileUi = Instantiate(TileCanvas, worldFacet.transform);
            tileUi.transform.localPosition = dual[i].Centroid + TILE_UI_OFFSET * dual[i].Centroid.normalized;
            tileUi.transform.localRotation = Quaternion.LookRotation(dual[i].Normal, Vector3.ProjectOnPlane(Vector3.up, dual[i].Normal));
            float scale = 0.75f * dual[i].Centered.Vertices.Select(Vector3.Magnitude).Min();
            tileUi.transform.localScale = new Vector3(scale, scale, scale);

            worldFacets.Add(worldFacet);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
