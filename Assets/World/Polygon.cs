using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Polygon
{
    public readonly Vector3[] Vertices;
    public readonly Vector3 Centroid;
    public readonly Vector3 Normal;
    public readonly Polygon Centered
    {
        get
        {
            var centroid = Centroid;
            return new(Vertices.Select(v => v - centroid).ToArray(), Vector3.zero, Normal);
        }
    }

    public readonly Vector3 this[int i]
    {
        get => Vertices[i];
    }
    public Polygon(Vector3[] vertices, Vector3 centroid, Vector3 normal)
    {
        this.Vertices = vertices;
        this.Centroid = centroid;
        this.Normal = normal;
    }
    public readonly Mesh ToMesh()
    {
        List<int> indices = new();
        Mesh mesh = new();
        //now add the indices that triangulate the hex/pent to the newIndices list
        for (int i = 1; i < Vertices.Length - 1; i++)
        {
            indices.Add(0);
            indices.Add(i);
            indices.Add(i + 1);
        }

        //add normals to the new points we added.
        mesh.vertices = Vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.normals = Enumerable.Repeat(Normal, Vertices.Length).ToArray();
        return mesh;
    }
}
