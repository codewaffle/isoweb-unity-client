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
        AssetManager.Fetch(url, www => _renderer.material.mainTexture = www.texture);
    }

    public void SetPoints(Vector2[] points)
    {
        var tri = new Triangulator(points);
        var indices = tri.Triangulate();

        if (_mesh == null)
        {
            _mesh = new Mesh();
            _renderer.material = Config.DefaultMaterial;
        }

        _mesh.Clear();
        _mesh.vertices = points.Select(s => (Vector3) s).ToArray();
        _mesh.uv = points.Select(s => new Vector2(s.x / 4f, s.y / 4f)).ToArray();
        // so lazy
        _mesh.normals = points.Select(s => new Vector3(0, 0, -1f)).ToArray();
        _mesh.triangles = indices;
        _meshFilter.mesh = _mesh;
    }
}
