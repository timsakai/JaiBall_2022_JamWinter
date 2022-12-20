using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] float m_mouseSensitivity;
    public static float MouseSensitivity;
    public static Settings settings;
    // Start is called before the first frame update
    void Start()
    {
        settings = this;
        Settings.MouseSensitivity = m_mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
