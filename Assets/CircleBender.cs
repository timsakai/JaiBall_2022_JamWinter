using RootMotion.Demos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class CircleBender : MonoBehaviour
{
    // Start is called before the first frame update
    public float radius = 10;
    public int resolution = 64;
    public bool update;
    private bool pre_update;
    [SerializeField] private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        UpdateLine();
    }

    // Update is called once per frame
    void Update()
    {
        if ((lineRenderer != null) && (update && !pre_update))
        {
            UpdateLine();
        }
        pre_update = update;
    }

    public void UpdateLine()
    {
        List<Vector3> verts = new List<Vector3>();
        //float angleunit = (Mathf.PI * 2) / resolution;
        float angleunit = 360.0f / resolution;
        Vector2 unit = new Vector2(Mathf.Sin(angleunit), Mathf.Cos(angleunit));
        Vector2 needle = Vector2.up * radius;
        for (int i = 0; i <= resolution+1; i++)
        {
            verts.Add(needle);
            needle = MyMath.Rotate(needle,angleunit);
        }
        lineRenderer.positionCount = resolution + 1;
        lineRenderer.SetPositions(verts.ToArray());
    }
}
