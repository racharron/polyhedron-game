using System.Linq;
using UnityEngine;

public class IcosphereGenerator : MonoBehaviour
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
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[12]
        {
            new Vector3(-SHORT, LONG, 0),
            new Vector3(SHORT, LONG, 0),
            new Vector3(-SHORT, -LONG, 0),
            new Vector3(SHORT, -LONG, 0),

            new Vector3(0, -SHORT, LONG),
            new Vector3(0, SHORT, LONG),
            new Vector3(0, -SHORT, -LONG),
            new Vector3(0, SHORT, -LONG),

            new Vector3(LONG,0,  -SHORT),
            new Vector3(LONG, 0, SHORT),
            new Vector3(-LONG,0,  -SHORT),
            new Vector3(-LONG, 0, SHORT),
        };

        int[] indices = new int[3 * 20] {
            0, 11, 5,
            0, 5, 1,
            0, 1, 7,
            0, 7, 10,
            0, 10, 11,

            1, 5, 9,
            5, 11, 4,
            11, 10, 2,
            10, 7, 6,
            7, 1, 8,

            3, 9, 4,
            3, 4, 2,
            3, 2, 6,
            3, 6, 8,
            3, 8, 9,

            4, 9, 5,
            2, 4, 11,
            6, 2, 10,
            8, 6, 7,
            9, 8, 1,
        };

        Vector3[] normals = vertices;

        //Vector2[] uv = Enumerable.Range(0, vertices.Length).Select(_ => new Vector2(Random.Range(0, 1), Random.Range(0, 1))).ToArray();

        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.normals = normals;
        //mesh.uv = uv;

        meshFilter.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
