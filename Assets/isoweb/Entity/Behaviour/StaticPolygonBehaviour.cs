using UnityEngine;
using System.Collections;
using System.Linq;
using SimpleJSON;

public class StaticPolygonBehaviour : MonoBehaviour
{
    private Mesh _mesh;
    private MeshRenderer _renderer;
    private MeshFilter _meshFilter;

    void Awake()
	{
	    _renderer = gameObject.AddComponent<MeshRenderer>();
	    _meshFilter = gameObject.AddComponent<MeshFilter>();
	}
	
    public void SetTextureUrl(string url)
    {
    }

    public void SetPoints(Vector2[] points)
    {
        var tri = new Triangulator(points);
        var indices = tri.Triangulate();

        if (_mesh == null)
            _mesh = new Mesh();

        _mesh.Clear();
        _mesh.vertices = points.Select(s => (Vector3) s).ToArray();
        _mesh.uv = points;
        _mesh.triangles = indices;
        _meshFilter.mesh = _mesh;
    }
}
