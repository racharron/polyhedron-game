using UnityEngine;
using System.Collections.Generic;
public class DualMeshTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Icosphere ico = new Icosphere();
        Mesh generate = generateDualMesh(ico.vertices);

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.mesh = generate;
    }

    static Mesh generateDualMesh(TriangleVertex[] vertices)
    {
        //create new mesh from this
        List<Vector3> newVertices = new List<Vector3>();
        List<int> newIndices = new List<int>();
        List<Vector3> newNormals = new List<Vector3>();

        for (int i = 0; i < vertices.Length; i++)
        {
            //ok here vertices[i] is a triangleVertex. We want to create the hexagon from it. first create new points from neighbors.
            int len = vertices[i].Neighbors.Length;
            for (int j = 0; j < len; j++)
            {
                //get three points to find the center
                Vector3 point1 = vertices[i].Position;
                Vector3 point2 = vertices[vertices[i].Neighbors[j]].Position;
                Vector3 point3 = vertices[vertices[i].Neighbors[(j + 1) % len]].Position;

                Vector3 newPoint = (point1 + point2 + point3) / 3;
                newVertices.Add(newPoint);
            }
            //now add the indices that triangulate the hex/pent to the newIndices list
            int pivotIndex = newVertices.Count - len;
            for (int j = 1; j < len - 1; j++)
            {
                newIndices.Add(pivotIndex);
                newIndices.Add(pivotIndex + j);
                newIndices.Add(pivotIndex + j + 1);
            }

            //add normals to the new points we added.
            for (int j = 0; j < len; j++)
            {
                newNormals.Add(vertices[i].Position);
            }
        }
        Mesh mesh = new Mesh();
        mesh.vertices = newVertices.ToArray();
        mesh.normals = newNormals.ToArray();
        mesh.triangles = newIndices.ToArray();

        return mesh;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
