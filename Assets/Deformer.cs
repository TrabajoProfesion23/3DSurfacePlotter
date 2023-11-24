using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting.Dependencies.NCalc;
using Unity.VisualScripting;
using System;


[RequireComponent(typeof(MeshFilter))]
public class SingleThreadedDeformer : MonoBehaviour
{
    [SerializeField] protected float _speed = 2.0f;
    [SerializeField] protected float _amplitude = 0.25f;
    [SerializeField] protected GameObject text;
    protected Mesh Mesh;

    private Vector3[] _vertices;
    private int[] tri = new int[100 * 100 * 6];

    protected void Awake()
    {
        Mesh = this.GetComponent<MeshFilter>().mesh;

        for (int i = 0; i < 100; i++) 
        {
            for (int j = 0; j < 100; j++)
            {
                int k = i * 100 + j;
                Debug.Log(k);
                _vertices[k] = new Vector3(i,0f,j);
            }
        }

        
        int id = 0;
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                //first triangle..
                tri[id] = i * 101 + j;
                tri[id + 1] = tri[id] + 101;
                tri[id + 2] = tri[id + 1] + 1;

                //second triangle..
                tri[id + 3] = tri[id];
                tri[id + 4] = tri[id + 2];
                tri[id + 5] = tri[id] + 1;

                id += 6;
            }
        }


        //_vertices = Mesh.vertices;
    }   


    private void Update()
    {
        Deform();
    }

    private void Deform()
    {
        for (var i = 0; i < _vertices.Length; i++)
        {
            var position = _vertices[i];
            string exp = text.GetComponent<ModifyText>().getExpression();
            position.y = DeformerUtilities.CalculateDisplacement(position, Time.time, _speed, _amplitude, exp);
            _vertices[i] = position;
        }

        // MarkDynamic optimizes mesh for frequent updates according to docs
        Mesh.MarkDynamic();
        // Update the mesh visually just by setting the new vertices array
        Mesh.SetVertices(_vertices);
        Mesh.triangles = tri;
        // Must be called so the updated mesh is correctly affected by the light
        Mesh.RecalculateNormals();
    }
}

public static class DeformerUtilities
{
    //[BurstCompile]
    public static float CalculateDisplacement(Vector3 position, float time, float speed, float amplitude, string exp)
    {
        Expression e = new Expression(exp);
        if (e.HasErrors())
        {
            return 0;
        }

        e.Parameters["x"]=position.x;
        e.Parameters["y"]=position.z;
        float y = 0;
        float.TryParse(e.Evaluate(null).ToString(), out y);

        return y;
    }
}