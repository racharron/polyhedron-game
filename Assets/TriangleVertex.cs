using System.Linq;
using UnityEngine;

public class TriangleVertex
{
    public readonly Vector3 Position;
    public int[] Neighbors;
    public TriangleVertex(Vector3 Position)
    {
        this.Position = Position;
    }
    public TriangleVertex(Vector3 Position, int a, int b)
    {
        this.Position = Position;
        Neighbors = Enumerable.Repeat(-1, 5).ToArray();
        Neighbors[0] = a; Neighbors[3] = b;
    }
    public TriangleVertex(Vector3 Position, int[] neighbors)
    {
        this.Position = Position;
        Neighbors = neighbors;
    }
    public void AddAfter(int predecessor, int a, int b)
    {
        if (Neighbors[0] == predecessor)
        {
            Neighbors[1] = a; Neighbors[2] = b;
        }
        else if (Neighbors[3] == predecessor)
        {
            Neighbors[4] = a; Neighbors[5] = b;
        }
    }
}