using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class CubeChunk : MonoBehaviour
{
    public int xSize = 100;
    public int ySize = 20;
    public int zSize = 100;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    private bool[,,] _isSet;
    private Vector3 _centerOffset;
    private Vector3 _blockMiddle;

    private List<Vector3> _vertices;
    private List<Vector2> _uvs;
    private List<Vector3> _normals;
    private List<int> _triangles;

    // Start is called before the first frame update
    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
        
        InitCubes();
        RebuildMesh();
    }

    private void OnMouseDown()
    {
        if (Camera.main != null)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(mouseRay, out RaycastHit hit);

            Vector3 localHitPoint = WorldCoordinatesToXyz(hit.point + mouseRay.direction.normalized * 0.01f);
            if (!IsInBounds((int) localHitPoint.x, (int) localHitPoint.y, (int) localHitPoint.z))
            {
                Debug.LogError("Clicked not in bounds");
                return;
            }

            if (!_isSet[(int) localHitPoint.x, (int) localHitPoint.y, (int) localHitPoint.z])
            {
                Debug.LogError("Clicked not a block");
                return;
            }

            _isSet[(int) localHitPoint.x, (int) localHitPoint.y, (int) localHitPoint.z] = false;
            RebuildMesh();

            Debug.DrawLine(mouseRay.origin, hit.point, Color.red, 10f);
        }
    }

    private void InitCubes()
    {
        _isSet = new bool[xSize, ySize, zSize];
        _centerOffset = new Vector3(-xSize / 2.0f, -ySize / 2.0f, -zSize / 2.0f);
        _blockMiddle = new Vector3(0.5f, 0.5f, 0.5f);

        for (int dz = 0; dz < zSize; dz++)
        {
            for (int dx = 0; dx < xSize; dx++)
            {
                int height = (int) (Mathf.PerlinNoise(dx * 0.025f, dz * 0.025f) * 1.0f * (ySize - 1));

                for (int dy = 0; dy < height; dy++)
                {
                    _isSet[dx, dy, dz] = true;
                }
            }
        }
    }

    private void RebuildMesh()
    {
        Mesh newMesh = new Mesh();

        _vertices = new List<Vector3>();
        _uvs = new List<Vector2>();
        _normals = new List<Vector3>();
        _triangles = new List<int>();

        for (int dy = 0; dy < ySize; dy++)
        {
            for (int dz = 0; dz < zSize; dz++)
            {
                for (int dx = 0; dx < xSize; dx++)
                {
                    if (_isSet[dx, dy, dz])
                    {
                        bool hasCubeBefore = IsInBounds(dx, dy, dz - 1) && _isSet[dx, dy, dz - 1];
                        bool hasCubeBehind = IsInBounds(dx, dy, dz + 1) && _isSet[dx, dy, dz + 1];
                        bool hasCubeAbove = IsInBounds(dx, dy + 1, dz) && _isSet[dx, dy + 1, dz];
                        bool hasCubeBelow = IsInBounds(dx, dy - 1, dz) && _isSet[dx, dy - 1, dz];
                        bool hasCubeLeft = IsInBounds(dx - 1, dy, dz) && _isSet[dx - 1, dy, dz];
                        bool hasCubeRight = IsInBounds(dx + 1, dy, dz) && _isSet[dx + 1, dy, dz];

                        Vector3 offset = new Vector3(dx, dy, dz) + _centerOffset;

                        if (!hasCubeBefore)
                        {
                            _vertices.AddRange(Cube.GetVertices(Cube.Face.Front, offset));
                            _uvs.AddRange(Cube.GetUVs(Cube.Face.Front));
                            for (int i = 0; i < 4; i++) _normals.Add(Cube.GetNormal(Cube.Face.Front));
                            _triangles.AddRange(Cube.GetTriangles(_vertices.Count - 4));
                        }

                        if (!hasCubeBehind)
                        {
                            _vertices.AddRange(Cube.GetVertices(Cube.Face.Back, offset));
                            _uvs.AddRange(Cube.GetUVs(Cube.Face.Back));
                            for (int i = 0; i < 4; i++) _normals.Add(Cube.GetNormal(Cube.Face.Back));
                            _triangles.AddRange(Cube.GetTriangles(_vertices.Count - 4));
                        }

                        if (!hasCubeAbove)
                        {
                            _vertices.AddRange(Cube.GetVertices(Cube.Face.Top, offset));
                            _uvs.AddRange(Cube.GetUVs(Cube.Face.Top));
                            for (int i = 0; i < 4; i++) _normals.Add(Cube.GetNormal(Cube.Face.Top));
                            _triangles.AddRange(Cube.GetTriangles(_vertices.Count - 4));
                        }

                        if (!hasCubeBelow)
                        {
                            _vertices.AddRange(Cube.GetVertices(Cube.Face.Bottom, offset));
                            _uvs.AddRange(Cube.GetUVs(Cube.Face.Bottom));
                            for (int i = 0; i < 4; i++) _normals.Add(Cube.GetNormal(Cube.Face.Bottom));
                            _triangles.AddRange(Cube.GetTriangles(_vertices.Count - 4));
                        }

                        if (!hasCubeLeft)
                        {
                            _vertices.AddRange(Cube.GetVertices(Cube.Face.Left, offset));
                            _uvs.AddRange(Cube.GetUVs(Cube.Face.Left));
                            for (int i = 0; i < 4; i++) _normals.Add(Cube.GetNormal(Cube.Face.Left));
                            _triangles.AddRange(Cube.GetTriangles(_vertices.Count - 4));
                        }

                        if (!hasCubeRight)
                        {
                            _vertices.AddRange(Cube.GetVertices(Cube.Face.Right, offset));
                            _uvs.AddRange(Cube.GetUVs(Cube.Face.Right));
                            for (int i = 0; i < 4; i++) _normals.Add(Cube.GetNormal(Cube.Face.Right));
                            _triangles.AddRange(Cube.GetTriangles(_vertices.Count - 4));
                        }
                    }
                }
            }
        }

        newMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        newMesh.SetVertices(_vertices.ToArray());
        newMesh.SetNormals(_normals.ToArray());
        newMesh.SetTriangles(_triangles.ToArray(), 0);
        newMesh.SetUVs(0, _uvs.ToArray());

        // newMesh.RecalculateNormals();
        // newMesh.Optimize();

        _meshFilter.sharedMesh = newMesh;
        _meshCollider.sharedMesh = newMesh;
    }

    public bool IsInBounds(int x, int y, int z)
    {
        return x >= 0 && y >= 0 && z >= 0 && x < xSize && y < ySize && z < zSize;
    }

    public bool IsSet(int x, int y, int z)
    {
        return IsInBounds(x, y, z) && _isSet[x, y, z];
    }

    public bool IsSet(Vector3 worldCoordinates)
    {
        Vector3 local = WorldCoordinatesToXyz(worldCoordinates);
        return IsSet((int) local.x, (int) local.y, (int) local.z);
    }

    public Vector3 WorldCoordinatesToXyz(Vector3 worldCoordinates)
    {
        Vector3 localCoordinate = transform.InverseTransformPoint(worldCoordinates) - _centerOffset;
        return new Vector3((int) localCoordinate.x, (int) localCoordinate.y, (int) localCoordinate.z);
    }

    public Vector3 LocalToWorldCoordinates(Vector3 localCoordinates)
    {
        return transform.TransformPoint(localCoordinates + _centerOffset + _blockMiddle);
    }
}