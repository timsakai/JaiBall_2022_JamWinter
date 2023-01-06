using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatCircleManager : MonoBehaviour
{
    [SerializeField] GameObject beatCircleDefault;
    [SerializeField] GameObject beatCircleMeasureFirst;
    List<CircleTween> tweens = new List<CircleTween>();
    float measureBeatCount = 4;
    float beatPerMinitue = 140;
    bool started = false;
    public static List<GameObject> GenaratedBeatCircle;
    // Start is called before the first frame update
    private void Awake()
    {
        measureBeatCount = TempoSettings.MeasureBeatCount;
        beatPerMinitue = TempoSettings.BeatPerMinitue;
        for (int i = 0; i < measureBeatCount; i++)
        {
            GameObject instance;
            if (i == 0)
            {
                instance = Instantiate(beatCircleMeasureFirst, transform);
            }
            else
            {
                instance = Instantiate(beatCircleDefault, transform);
            }
            instance.transform.position = transform.position;
            CircleTween tween = instance.GetComponent<CircleTween>();
            tweens.Add(tween);
        }
    }
    public int JudgeTiming(float goodThreshold)
    {
        bool beated = false;
        foreach (var tween in tweens)
        {
            float progress = tween.GetCircleProgress();
            //Debug.Log("progress: " + progress);
            if ((progress > 0.11f) && (progress < (10f * goodThreshold)))
            {
                Debug.Log("Beated:" + tween.gameObject.name);
                Debug.Log("Beated:" + progress);
                beated = true;
            }
        }
        if (beated) return 0;
        return -1;
    }
    void Start()
    {
        for (int i = 0; i < tweens.Count; i++)
        {
            float bps = beatPerMinitue / 60;
            float duration = 1 / bps;
            //Debug.Log("bps :" + bps);
            //Debug.Log("Duration :" + duration);
            tweens.ToArray()[i].SetSeqProps(duration * i, duration, duration * (measureBeatCount -1), true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (BeatManager.currentClip.loadState == AudioDataLoadState.Loaded)
        {
            if (!started)
            {
                foreach (var item in tweens)
                {
                    item.StartTween();
                }
                started = true;
            }
        }
    }

    private void OnGUI()
    {
        for (int i = 0; i < tweens.Count; i++)
        {
            GUI.Label(new Rect(50, 300 + i*20, 100, 50), tweens.ToArray()[i].GetCircleProgress().ToString());
        }
    }
}
