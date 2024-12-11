public class Triangle
{
    public readonly int a, b, c;

    public Triangle(int a, int b, int c)
    {
        this.a = a; this.b = b; this.c = c;
    }
    public bool Contains(int i)
    {
        return a == i || b == i || c == i;
    }
    public bool InSequence(int i, int j)
    {
        return (a == i && b == j) || (b == i && c == j) || (c == i && a == j);
    }
    public PolyhedronEdge[] Edges()
    {
        return new PolyhedronEdge[3] { new(a, b), new(b, c), new(c, a) };
    }
}