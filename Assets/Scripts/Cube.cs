using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
public static class Cube
{
    /// <summary>
    /// This enum can be cast to int to access values such as vertices for a given Face in class Cube
    /// </summary>
    public enum Face
    {
        Front = 0,
        Back = 1,
        Top = 2,
        Bottom = 3,
        Left = 4,
        Right = 5
    }

    #region Cube constants

    public const int VerticesPerCubeCount = 24;
    public const int UVsPerCubeCount = 24;
    public const int NormalsPerCubeCount = 24;
    public const int TrianglesPerCubeCount = 12;
    public const int TrianglePointsPerCubeCount = 36;

    #endregion Cube constants

    #region Per-face data

    private static readonly Vector3[][] VerticesPerFace = {
        new[] {new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0)},
        new[] {new Vector3(1, 0, 1), new Vector3(1, 1, 1), new Vector3(0, 1, 1), new Vector3(0, 0, 1)},
        new[] {new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 0)},
        new[] {new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 1)},
        new[] {new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(0, 1, 0), new Vector3(0, 0, 0)},
        new[] {new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 1, 1), new Vector3(1, 0, 1)}
    };

    private static readonly Vector3[] NormalPerFace = new Vector3[]
    {
        Vector3.back,
        Vector3.forward,
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right
    };

    private static readonly int[] Triangles = new int[] {0, 1, 2, 0, 2, 3};

    private static readonly Vector2[][] UvsPerFace = new Vector2[][]
    {
        new[]
        {
            new Vector2(0 * 1.0f / 6.0f, 0), new Vector2(0 * 1.0f / 6.0f, 1), new Vector2(1 * 1.0f / 6.0f, 1),
            new Vector2(1 * 1.0f / 6.0f, 0)
        },
        new[]
        {
            new Vector2(1 * 1.0f / 6.0f, 0), new Vector2(1 * 1.0f / 6.0f, 1), new Vector2(2 * 1.0f / 6.0f, 1),
            new Vector2(2 * 1.0f / 6.0f, 0)
        },
        new[]
        {
            new Vector2(2 * 1.0f / 6.0f, 0), new Vector2(2 * 1.0f / 6.0f, 1), new Vector2(3 * 1.0f / 6.0f, 1),
            new Vector2(3 * 1.0f / 6.0f, 0)
        },
        new[]
        {
            new Vector2(3 * 1.0f / 6.0f, 0), new Vector2(3 * 1.0f / 6.0f, 1), new Vector2(4 * 1.0f / 6.0f, 1),
            new Vector2(4 * 1.0f / 6.0f, 0)
        },
        new[]
        {
            new Vector2(4 * 1.0f / 6.0f, 0), new Vector2(4 * 1.0f / 6.0f, 1), new Vector2(5 * 1.0f / 6.0f, 1),
            new Vector2(5 * 1.0f / 6.0f, 0)
        },
        new[]
        {
            new Vector2(5 * 1.0f / 6.0f, 0), new Vector2(5 * 1.0f / 6.0f, 1), new Vector2(6 * 1.0f / 6.0f, 1),
            new Vector2(6 * 1.0f / 6.0f, 0)
        },
    };

    #endregion Per-face data

    public static Vector3[] GetVertices(Face face, Vector3 offset = default)
    {
        return new[]
        {
            VerticesPerFace[(int) face][0] + offset,
            VerticesPerFace[(int) face][1] + offset,
            VerticesPerFace[(int) face][2] + offset,
            VerticesPerFace[(int) face][3] + offset,
        };
    }

    public static Vector3 GetNormal(Face face)
    {
        return NormalPerFace[(int) face];
    }

    public static int[] GetTriangles(int offset)
    {
        return new[]
        {
            Triangles[0] + offset,
            Triangles[1] + offset,
            Triangles[2] + offset,
            Triangles[3] + offset,
            Triangles[4] + offset,
            Triangles[5] + offset
        };
    }

    public static Vector2[] GetUVs(Face face)
    {
        return UvsPerFace[(int) face];
    }
}