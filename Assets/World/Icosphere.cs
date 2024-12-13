using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Icosphere
{
    static readonly float PHI = (1 + Mathf.Sqrt(5)) / 2;
    static readonly float NORM_FACTOR = Mathf.Sqrt(1 + PHI * PHI);
    static readonly float LONG = PHI / NORM_FACTOR;
    static readonly float SHORT = 1 / NORM_FACTOR;

    static readonly Vector3[] INITIAL_POSITIONS = {
        new(-SHORT, LONG, 0),
        new(SHORT, LONG, 0),
        new(-SHORT, -LONG, 0),
        new(SHORT, -LONG, 0),

        new(0, -SHORT, LONG),
        new(0, SHORT, LONG),
        new(0, -SHORT, -LONG),
        new(0, SHORT, -LONG),

        new(LONG, 0, -SHORT),
        new(LONG, 0, SHORT),
        new(-LONG, 0, -SHORT),
        new(-LONG, 0, SHORT),
    };
    static readonly Triangle[] INITIAL_TRIANGLES = {
        new(0, 11, 5),
        new(0, 5, 1),
        new(0, 1, 7),
        new(0, 7, 10),
        new(0, 10, 11),

        new(1, 5, 9),
        new(5, 11, 4),
        new(11, 10, 2),
        new(10, 7, 6),
        new( 7, 1,8),

        new(3, 9, 4),
        new(3, 4, 2),
        new(3, 2, 6),
        new(3, 6, 8),
        new(3, 8, 9),

        new(4, 9, 5),
        new(2, 4, 11),
        new(6, 2, 10),
        new(8, 6, 7),
        new(9, 8, 1),
    };

    public readonly TriangleVertex[] vertices;

    public Icosphere(uint bisections)
    {
        vertices = INITIAL_POSITIONS.Select(p => new TriangleVertex(p)).ToArray();
        for (int v = 0; v < vertices.Length; v++)
        {
            var surroundingEdges = new List<PolyhedronEdge>(5);
            foreach (var triangle in INITIAL_TRIANGLES)
            {
                var opposite = triangle.OppositeEdge(v);
                if (opposite != null) surroundingEdges.Add(opposite);
            }
            for (int i = 0; i < surroundingEdges.Count - 2; i++) {
                for (int j = i + 2; j < surroundingEdges.Count; j++)
                {
                    if (surroundingEdges[i].b == surroundingEdges[j].a)
                    {
                        (surroundingEdges[j], surroundingEdges[i + 1]) = (surroundingEdges[i + 1], surroundingEdges[j]);
                        break;
                    }
                }
            }
            vertices[v].Neighbors = surroundingEdges.Select(e => e.a).ToArray();
        }
        for (int i = 0; i < bisections; i++)
        {
            var nextVertices = new List<TriangleVertex>(4 * vertices.Length);
            nextVertices.AddRange(vertices);
            int[][] midpoints = new int[vertices.Length][];
            foreach (var v in Enumerable.Range(0, vertices.Length))
            {
                List<int> localMidpoints = new(vertices[v].Neighbors.Length);
                foreach (var neighbor in vertices[v].Neighbors)
                {
                    if (midpoints[neighbor] == null)
                    {
                        var midPos = (vertices[v].Position + vertices[neighbor].Position) / 2;
                        var midPoint = nextVertices.Count;
                        nextVertices.Add(new(midPos.normalized, v, neighbor));
                        localMidpoints.Add(midPoint);
                    }
                    else
                    {
                        localMidpoints.Add(midpoints[neighbor][Array.IndexOf(vertices[neighbor].Neighbors, v)]);
                    }
                }
                midpoints[v] = localMidpoints.ToArray();
            }
            foreach (var v in Enumerable.Range(0, vertices.Length))
            {
                for (int pre = 0; pre < vertices[v].Neighbors.Length; pre++)
                {
                    int mid = (pre + 1) % vertices[v].Neighbors.Length;
                    int post = (pre + 2) % vertices[v].Neighbors.Length;
                    int pre_midpoint = midpoints[v][pre];
                    int midpoint = midpoints[v][mid];
                    int post_midpoint = midpoints[v][post];
                    //  We need to swap these around to maintain the winding order.
                    nextVertices[midpoint].AddAround(v, post_midpoint, pre_midpoint);
                    nextVertices[v].Neighbors[mid] = midpoint;
                }
            }
            vertices = nextVertices.ToArray();
        }
    }
    public Mesh ToMesh()
    {
        Mesh mesh = new Mesh();
        List<Vector3> positions = new(3 * vertices.Length);
        List<Vector3> normals = new(3 * vertices.Length);

        foreach (var v in Enumerable.Range(0, vertices.Length))
        {
            foreach (var n in Enumerable.Range(0, vertices[v].Neighbors.Length))
            {
                var a = v;
                var b = vertices[v].Neighbors[n];
                var c = vertices[v].Neighbors[(n + 1) % vertices[v].Neighbors.Length];
                if (a < b && a < c)
                {
                    var aPos = vertices[a].Position;
                    var bPos = vertices[b].Position;
                    var cPos = vertices[c].Position;
                    positions.Add(aPos);
                    positions.Add(bPos);
                    positions.Add(cPos);
                    normals.AddRange(Enumerable.Repeat((aPos + bPos + cPos) / 3, 3));
                }
            }
        }
        mesh.vertices = positions.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = Enumerable.Range(0, positions.Count).ToArray();

        mesh.name = "Icosphere";

        return mesh;
    }
    public Polygon[] Dual()
    {
        List<Polygon> polygons = new(vertices.Length);
        foreach (var vertex in vertices)
        {
            List<Vector3> polygonVertices = new(vertices.Length);
            //ok here vertices[i] is a triangleVertex. We want to create the hexagon from it. first create new points from neighbors.
            for (int n = 0; n < vertex.Neighbors.Length; n++)
            {
                var a = vertex.Position;
                var b = vertices[vertex.Neighbors[n]].Position;
                var c = vertices[vertex.Neighbors[(n + 1) % vertex.Neighbors.Length]].Position;
                polygonVertices.Add((a + b + c) / 3);
            }
            polygons.Add(new(polygonVertices.ToArray(), polygonVertices.Aggregate((a, b) => a+b) / vertices.Length, vertex.Position));
        }
        return polygons.ToArray();
    }
}
