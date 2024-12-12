using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

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

    public TriangleVertex[] vertices {  get; private set; }

    public Icosphere()
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
            Assert.AreEqual(surroundingEdges.Count, 5);
            for (int i = 0; i < surroundingEdges.Count - 2; i++) {
                for (int j = i + 2; j < surroundingEdges.Count; j++)
                {
                    if (surroundingEdges[i].b == surroundingEdges[j].a)
                    {
                        (surroundingEdges[j], surroundingEdges[i+1]) = (surroundingEdges[i+1], surroundingEdges[j]);
                        break;
                    }
                }
            }
            vertices[v].Neighbors = surroundingEdges.Select(e => e.a).ToArray();
            Assert.AreEqual(vertices[v].Neighbors.Length, 5);
            Assert.AreEqual(vertices[v].Neighbors.Distinct().Count(), 5);
            //  Debug code
            for (int i = 0; i < surroundingEdges.Count; i++)
            {
                Assert.AreEqual(surroundingEdges[i].b, surroundingEdges[(i+1)%surroundingEdges.Count].a);
            }
        }
        //  Debug code
        
        for (int n = 0; n < vertices.Length; n++)
        {
            bool found = false;
            for (int v = 0; v < vertices.Length; v++)
            {
                if (vertices[v].Neighbors.Contains(n)) found = true;
            }
            Assert.IsTrue(found);
        }
        foreach (var triangle in INITIAL_TRIANGLES) {
            int count = 0;
            foreach (var v in Enumerable.Range(0, vertices.Length))
            {
                foreach (var i in Enumerable.Range(0, vertices[v].Neighbors.Length))
                {
                    var j = (i + 1) % vertices[v].Neighbors.Length;
                    var a = v;
                    var b = vertices[v].Neighbors[i];
                    var c = vertices[v].Neighbors[j];
                    if (
                        (triangle.a == a && triangle.b == b && triangle.c == c)
                        || (triangle.b == a && triangle.c == b && triangle.a == c)
                        || (triangle.c == a && triangle.a == b && triangle.b == c)
                        )
                    {
                        count++;
                    }
                }
            }
            Assert.AreEqual(count, 3);
        }
    }
    public void Subdivide()
    {
        var nextVertices= new List<TriangleVertex>(4 * vertices.Length);
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
                } else
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
                int mid = (pre+1) % vertices[v].Neighbors.Length;
                int post = (pre+2) % vertices[v].Neighbors.Length;
                int pre_midpoint = midpoints[v][pre];
                int midpoint = midpoints[v][mid];
                int post_midpoint = midpoints[v][post];
                //  We need to swap these around to maintain the winding order.
                nextVertices[midpoint].AddAround(v, post_midpoint, pre_midpoint);
                nextVertices[v].Neighbors[mid] = midpoint;
            }
        }
        vertices = nextVertices.ToArray();
        for (int v = 0; v < vertices.Length; v++)
        {
            Assert.IsFalse(vertices[v].Neighbors.Contains(-1));
            Assert.IsFalse(vertices[v].Neighbors.Contains(v));
            Assert.AreEqual(vertices[v].Neighbors.Distinct().Count(), vertices[v].Neighbors.Length);
            for (int n = 0; n < vertices[v].Neighbors.Length; n++)
            {
                Assert.IsTrue(vertices[vertices[v].Neighbors[n]].Neighbors.Contains(v));
            }
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

        mesh.name = "Debug Icosphere";

        return mesh;
    }
    public List<Mesh> ToDualMeshTiles()
    {
        //create new mesh from this
        List<Vector3> newNormals = new();
        List<Mesh> meshes = new();

        foreach (var vertex in vertices)
        {
            List<Vector3> positions = new();
            List<int> indices = new();
            Mesh mesh = new();
            //ok here vertices[i] is a triangleVertex. We want to create the hexagon from it. first create new points from neighbors.
            for (int n = 0; n < vertex.Neighbors.Length; n++)
            {
                var a = vertex.Position;
                var b = vertices[vertex.Neighbors[n]].Position;
                var c = vertices[vertex.Neighbors[(n+1)%vertex.Neighbors.Length]].Position;
                positions.Add((a + b + c) / 3);
            }
            //now add the indices that triangulate the hex/pent to the newIndices list
            for (int i = 1; i < vertex.Neighbors.Length - 1; i++)
            {
                indices.Add(0);
                indices.Add(i);
                indices.Add(i + 1);
            }

            //add normals to the new points we added.
            mesh.vertices = positions.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.normals = Enumerable.Repeat(vertex.Position, vertex.Neighbors.Length).ToArray();
            meshes.Add(mesh);
        }

        return meshes;
    }
}
