using NUnit.Framework;
using System.Linq;
using UnityEngine;

public class TriangleVertex
{
    readonly static int DEFAULT_PRE_A = 0;
    readonly static int DEFAULT_A = 1;
    readonly static int DEFAULT_POST_A = 2;
    readonly static int DEFAULT_PRE_B = 3;
    readonly static int DEFAULT_B = 4;
    readonly static int DEFAULT_POST_B = 5;
    public readonly Vector3 Position;
    public int[] Neighbors;
    public TriangleVertex(Vector3 Position)
    {
        this.Position = Position;
    } 
    public TriangleVertex(Vector3 Position, int a, int b)
    {
        this.Position = Position;
        Neighbors = Enumerable.Repeat(-1, 6).ToArray();
        Neighbors[DEFAULT_A] = a; Neighbors[DEFAULT_B] = b;
    }
    public TriangleVertex(Vector3 Position, int[] neighbors)
    {
        Assert.AreEqual(neighbors.Length, 5);
        this.Position = Position;
        Neighbors = neighbors;
    }
    public void AddAround(int mid, int pre, int post)
    {
        Assert.AreEqual(Neighbors.Length, 6);
        if (Neighbors[DEFAULT_A] == mid) {
            Neighbors[DEFAULT_PRE_A] = pre; Neighbors[DEFAULT_POST_A] = post;
        } else if (Neighbors[DEFAULT_B] == mid) {
            Neighbors[DEFAULT_PRE_B] = pre; Neighbors[DEFAULT_POST_B] = post;
        }
    }
}
