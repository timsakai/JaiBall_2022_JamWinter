using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleBender))]
public class CircleTween : MonoBehaviour
{
    CircleBender circleBender;
    [SerializeField] float delay = 0;
    [SerializeField] float duration = 1;
    [SerializeField] float wait = 1;
    public float fromRadius = 10;
    public float toRadius = 1;
    [SerializeField] bool loop = false;
    public bool destroyOnComplete = true;
    // Start is called before the first frame update
    void Start()
    {
        circleBender = GetComponent<CircleBender>();
        circleBender.radius = fromRadius;
        circleBender.UpdateLine();
    }

    public void SetSeqProps(float _delay,float _duration,float _wait,bool _loop)
    {
        delay = _delay;
        duration = _duration;
        wait = _wait;
        loop = _loop;
    }

    public void StartTween()
    {

        //Debug.Log(gameObject.name + " delay :" + delay);
        //Debug.Log(gameObject.name + " duration :" + duration);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => circleBender.radius, (x) => { circleBender.radius = x; circleBender.UpdateLine(); }, toRadius, duration)
            .SetEase(Ease.InQuad))
            .AppendInterval(wait)
            .SetLoops(loop ? -1 : 0).OnComplete(() => { if (destroyOnComplete) Destroy(gameObject); });

        Sequence super = DOTween.Sequence().Append(sequence).SetDelay(delay);
        //Debug.Log("BeatStart " + gameObject.name);
    }

    public float GetCircleProgress()
    {
        return circleBender.radius - 0.7f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
