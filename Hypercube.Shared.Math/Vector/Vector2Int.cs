namespace Hypercube.Shared.Math.Vector;

public readonly partial struct Vector2Int(int x, int y)
{
    public static readonly Vector2Int Zero = new(0, 0);
    public static readonly Vector2Int One = new(1, 1);
    public static readonly Vector2Int Up = new(0, 1);
    public static readonly Vector2Int Down = new(0, -1);
    public static readonly Vector2Int Right = new(1, 0);
    public static readonly Vector2Int Left = new(-1, 0);
    
    public readonly int X = x;
    public readonly int Y = y;
    public readonly float Ratio = x / (float)y;

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2Int operator +(Vector2Int a, int b)
    {
        return new Vector2Int(a.X + b, a.Y + b);
    }

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2Int operator -(Vector2Int a, int b)
    {
        return new Vector2Int(a.X - b, a.Y - b);
    }

    public static Vector2Int operator *(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X * b.X, a.Y * b.Y);
    }

    public static Vector2Int operator *(Vector2Int a, int b)
    {
        return new Vector2Int(a.X * b, a.Y * b);
    }

    public static Vector2 operator *(Vector2Int a, float b)
    {
        return new Vector2(a.X * b, a.Y * b);
    }

    public static Vector2Int operator /(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X / b.X, a.Y / b.Y);
    }

    public static Vector2Int operator /(Vector2Int a, int b)
    {
        return new Vector2Int(a.X / b, a.Y / b);
    }

    public static Vector2 operator /(Vector2Int a, float b)
    {
        return new Vector2(a.X / b, a.Y / b);
    }
    
    public static bool operator ==(Vector2Int a, Vector2Int b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector2Int a, Vector2Int b)
    {
        return !a.Equals(b);
    }

    public static implicit operator Vector2(Vector2Int a)
    {
        return new Vector2(a.X, a.Y);
    }

    public static implicit operator Vector2Int(Vector2 a)
    {
        return new Vector2Int((int)a.X, (int)a.Y);
    }

    public static implicit operator Vector2Int((int x, int y) a)
    {
        return new Vector2Int(a.x, a.y);
    }

    public readonly bool Equals(Vector2Int other)
    {
        return X == other.X && Y == other.Y;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Vector2Int vector && Equals(vector);
    }
    
    public override int GetHashCode()
    {
        return x + y;
    }

    public override string ToString()
    {
        return $"{x}, {y}";
    }
}