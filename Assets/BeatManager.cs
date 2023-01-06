using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    [SerializeField] AudioSource BaseMusicAudioSource;
    public static AudioClip currentClip;
    [SerializeField] UnityEvent OnGood = new UnityEvent();
    [SerializeField] UnityEvent OnBad = new UnityEvent();

    [SerializeField] float successCoolTime = 0.2f;
    [SerializeField] float badCoolTime = 0.4f;
    float currentCoolTimeGoal = 0f;
    float cooltime = 0f;

    bool isStarted = false;
    float playtime = 0f;
    float beatUnit = 0f;
    [SerializeField] float goodThreshold = 0.1f;
    [SerializeField] BeatCircleManager beatCircleManager;
    // Start is called before the first frame update
    private void Awake()
    {
        currentClip = BaseMusicAudioSource.clip;
    }
    void Start()
    {
        float bps = TempoSettings.BeatPerMinitue / 60;
        beatUnit = 1 / bps;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentClip.loadState == AudioDataLoadState.Loaded) isStarted = true;
        if (isStarted) playtime += Time.deltaTime;

        cooltime += Time.deltaTime;

        if (cooltime >= currentCoolTimeGoal)
        {

            if (Input.GetAxis("Beat0") > 0
                || Input.GetAxis("Beat1") > 0
                || Input.GetAxis("Beat2") > 0
                || Input.GetAxis("Beat3") > 0)
            {
                cooltime = 0f;
                int judge = beatCircleManager.JudgeTiming(goodThreshold);
                if (judge <= -1)
                {
                    OnBad.Invoke();
                    currentCoolTimeGoal = badCoolTime;
                }
                else if (judge == 0)
                {
                    OnGood.Invoke();
                    currentCoolTimeGoal = successCoolTime;
                }
            }
        }
    }

    private int JudgeInput()
    {
        float mod = playtime % beatUnit;
        if ((mod <= (goodThreshold / 2)) || ((beatUnit - mod) <= (goodThreshold / 2)))
        {
            return 0;
        }
        return -1;
    }
}
