using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleBender))]
public class CircleTween : MonoBehaviour
{
    CircleBender circleBender;
    public float delay = 0;
    public float duration = 1;
    public float wait = 1;
    public float fromRadius = 10;
    public float toRadius = 1;
    public bool loop = false;
    public bool destroyOnComplete = true;
    // Start is called before the first frame update
    void Start()
    {
        circleBender = GetComponent<CircleBender>();
        circleBender.radius = fromRadius;
        circleBender.UpdateLine();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => circleBender.radius, (x) => { circleBender.radius = x; circleBender.UpdateLine(); }, toRadius, duration)
            .SetEase(Ease.InQuad))
            .AppendInterval(wait)
            .SetLoops(loop ? -1 : 0).OnComplete(() => { if (destroyOnComplete) Destroy(gameObject); });

        Sequence super = DOTween.Sequence().Append(sequence).SetDelay(delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
