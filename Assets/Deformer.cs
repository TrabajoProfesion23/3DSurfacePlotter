using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting.Dependencies.NCalc;
using Unity.VisualScripting;


[RequireComponent(typeof(MeshFilter))]
public class SingleThreadedDeformer : MonoBehaviour
{
    [SerializeField] protected float _speed = 2.0f;
    [SerializeField] protected float _amplitude = 0.25f;
    [SerializeField] protected GameObject text;
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
            string exp = text.GetComponent<ModifyText>().getExpression();
            position.y = DeformerUtilities.CalculateDisplacement(position, Time.time, _speed, _amplitude, exp);
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
    public static float CalculateDisplacement(Vector3 position, float time, float speed, float amplitude, string exp)
    {
        Expression e = new Expression(exp);
        //Expression e = new Expression("Sqrt(Pow(10000,2)-Pow([x],2)-Pow([z],2))"); //Pow(1,2)-Pow([x],2)-Pow([z],2)

        e.Parameters["x"]=position.x;
        e.Parameters["z"]=position.z;
        float y = 0;
        float.TryParse(e.Evaluate(null).ToString(), out y);

        return y;
    }
}