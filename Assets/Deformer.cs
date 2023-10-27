using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class SingleThreadedDeformer : MonoBehaviour
{
    [SerializeField] protected float _speed = 2.0f;
    [SerializeField] protected float _amplitude = 0.25f;
    protected Mesh Mesh;

    protected void Awake()
    {
        Mesh = GetComponent<MeshFilter>().mesh;
        _vertices = Mesh.vertices;
    }

    private Vector3[] _vertices;


    private void Update()
    {
        Deform();
    }

    private void Deform()
    {
        for (var i = 0; i < _vertices.Length; i++)
        {
            var position = _vertices[i];
            position.y = DeformerUtilities.CalculateDisplacement(position, Time.time, _speed, _amplitude);
            _vertices[i] = position;
        }

        // MarkDynamic optimizes mesh for frequent updates according to docs
        Mesh.MarkDynamic();
        // Update the mesh visually just by setting the new vertices array
        Mesh.SetVertices(_vertices);
        // Must be called so the updated mesh is correctly affected by the light
        Mesh.RecalculateNormals();
    }
}

public static class DeformerUtilities
{
    //[BurstCompile]
    public static float CalculateDisplacement(Vector3 position, float time, float speed, float amplitude)
    {
        /*var distance = 6f - Vector3.Distance(position, Vector3.zero);
        return Mathf.Sin(5 * speed + distance) * amplitude;*/
        return position.x*position.x/4 - position.z*position.z/4;
    }
}