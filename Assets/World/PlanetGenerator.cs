using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    static readonly float PHI = (1 + Mathf.Sqrt(5)) / 2;
    static readonly float NORM_FACTOR = Mathf.Sqrt(1 + PHI * PHI);
    static readonly float LONG = PHI / NORM_FACTOR;
    static readonly float SHORT = 1 / NORM_FACTOR;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Icosphere icosphere = new Icosphere();

        meshFilter.mesh = icosphere.ToMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
