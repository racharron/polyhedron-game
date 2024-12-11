using System;
using System.Collections.Generic;
using System.Linq;
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

    public TriangleVertex[] vertices;

    public Icosphere()
    {
        vertices = INITIAL_POSITIONS.Select(p => new TriangleVertex(p)).ToArray();
        for (int tri = 0; tri < vertices.Length; tri++)
        {
            var triangles = new List<Triangle>(5);
            foreach (var triangle in INITIAL_TRIANGLES)
            {
                if (triangle.Contains(tri)) triangles.Add(triangle);
            }
            Assert.AreEqual(triangles.Count, 5);
            for (int i = 0; i < triangles.Count - 1; i++)
            {
                for (int j = i + 1; j < triangles.Count; j++)
                {
                    if (triangles[i].Edges().Any(e => triangles[j].InSequence(e.b, e.a)))
                    {
                        Triangle tmp = triangles[i];
                        triangles[i] = triangles[j];
                        triangles[j] = tmp;
                        break;
                    }
                }
            }
            vertices[tri].Neighbors = triangles
                .SelectMany(tri => new int[3] { tri.a, tri.b, tri.c })
                .Where(i => i != tri)
                .Distinct()
                .ToArray();
            Assert.AreEqual(vertices[tri].Neighbors.Length, 5);
        }
        //  Debug code
        foreach (var triangle in INITIAL_TRIANGLES)
        {
            int count = 0;
            for (int v = 0; v < vertices.Length; v++)
            {
                var len = vertices[v].Neighbors.Length;
                for (int i = 0; i < len; i++)
                {
                    var j = (i + 1) % len;
                    if (
                        (triangle.a == v && triangle.b == i && triangle.c == j)
                        || (triangle.b == v && triangle.c == i && triangle.a == j)
                        || (triangle.c == v && triangle.a == i && triangle.b == j)
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
        var buffer = new List<TriangleVertex>(4 * vertices.Length);
        buffer.AddRange(vertices);
        for (int tri = 0; tri < vertices.Length; tri++)
        {
            foreach (var neighbor in vertices[tri].Neighbors)
            {
                //  Avoid double counting.
                if (tri < neighbor)
                {
                    var midpoint = (vertices[tri].Position + vertices[neighbor].Position) / 2;
                    buffer.Add(new(midpoint.normalized, tri, neighbor));
                }
            }
        }
    }
}
