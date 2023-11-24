using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting.Dependencies.NCalc;
using Unity.VisualScripting;
using System;


[RequireComponent(typeof(MeshFilter))]
public class SingleThreadedDeformer : MonoBehaviour
{
    [SerializeField] protected GameObject text;
    protected Mesh Mesh;

    protected int nX = 20;
    protected int nZ = 20;
    protected float x_scale = 1f;
    protected float z_scale = 1f;

    private Vector3[] _vertices;
    private int[] tri;

    protected void Awake()
    {
        x_scale = this.GetComponent<Transform>().localScale.x;
        z_scale = this.GetComponent<Transform>().localScale.z;
        Mesh = this.GetComponent<MeshFilter>().mesh;

        _vertices = new Vector3[nX * nZ];
        for (int i = 0; i < nZ; i++) 
        {
            for (int j = 0; j < nX; j++)
            {
                int k = i * nX + j;
                _vertices[k] = new Vector3((j-nX/2) * x_scale,0f,(i-nZ/2) * z_scale);
            }
        }


        tri = new int[(nX-1) * (nZ-1) * 6];
        int id = 0;
        for (int i = 0; i < (nX - 1); i++)
        {
            for (int j = 0; j < (nZ - 1); j++)
            {
                //first triangle..
                tri[id] = i * nX + j;
                tri[id + 1] = tri[id] + nX;
                tri[id + 2] = tri[id + 1] + 1;

                //second triangle..
                tri[id + 3] = tri[id];
                tri[id + 4] = tri[id + 2];
                tri[id + 5] = tri[id] + 1;

                id += 6;
            }
        }

        // MarkDynamic optimizes mesh for frequent updates according to docs
        Mesh.MarkDynamic();
        // Update the mesh visually just by setting the new vertices array
        Mesh.vertices = _vertices;
        Mesh.triangles = tri;
        // Must be called so the updated mesh is correctly affected by the light
        Mesh.RecalculateNormals();
        Mesh.RecalculateBounds();
    }   


    private void Update()
    {
        Deform();
    }

    private void Deform()
    {
        Debug.Log(_vertices.Length);
        for (var i = 0; i < _vertices.Length; i++)
        {
            var position = _vertices[i];
            string exp = text.GetComponent<ModifyText>().getExpression();
            position.y = DeformerUtilities.CalculateDisplacement(position, exp);
            _vertices[i] = position;
        }

        // MarkDynamic optimizes mesh for frequent updates according to docs
        Mesh.MarkDynamic();
        // Update the mesh visually just by setting the new vertices array
        Mesh.SetVertices(_vertices);
        // Must be called so the updated mesh is correctly affected by the light
        Mesh.RecalculateNormals();
        Mesh.RecalculateBounds();
    }
}

public static class DeformerUtilities
{
    //[BurstCompile]
    public static float CalculateDisplacement(Vector3 position, string exp)
    {
        if (exp == "") return 0;
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