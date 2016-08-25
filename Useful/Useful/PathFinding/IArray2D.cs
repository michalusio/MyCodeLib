namespace Useful.PathFinding
{
    public interface IArray2D
    {
        bool Bounds { get; }
        Manhattan2DNode this[int x, int y] { get; set; }
        int GetLength(int n);
    }
}