using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class MyMath : MonoBehaviour
{
    /// <summary>
    /// Vector2‚ð‰ñ“]‚³‚¹‚é
    /// </summary>
    /// <param name="from">‰ñ“]‚³‚¹‚½‚¢Vector2</param>
    /// <param name="angle">‰ñ“]‚³‚¹‚éŠp“x</param>
    /// <returns></returns>
    public static Vector2 Rotate(Vector2 from, float angle)
    {
        Complex a = new Complex(from.x, from.y);
        float cos = Mathf.Cos(Mathf.Deg2Rad * angle);
        float sin = Mathf.Sin(Mathf.Deg2Rad * angle);
        Complex b = new Complex(cos, sin);
        Complex result = a * b;
        return new Vector2((float)result.Real, (float)result.Imaginary);
    }
    public static Vector2 ComplexMul(Vector2 from_a,Vector2 from_b)
    {
        Complex a = new Complex(from_a.x, from_a.y);
        Complex b = new Complex(from_b.x, from_b.y);

        Complex result = a * b;

        return new Vector2((float)result.Real, (float)result.Imaginary);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
