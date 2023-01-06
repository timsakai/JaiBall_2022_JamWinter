using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoSettings : MonoBehaviour
{
    public static TempoSettings tempo;
    [SerializeField] float m_measureBeatCount = 4;
    [SerializeField] float m_beatPerMinitue = 140;
    public static float MeasureBeatCount;
    public static float BeatPerMinitue;
    // Start is called before the first frame update
    private void Awake()
    {
        tempo = this;
        MeasureBeatCount = m_measureBeatCount;
        BeatPerMinitue = m_beatPerMinitue;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
