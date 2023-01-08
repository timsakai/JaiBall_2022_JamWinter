using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleBender))]
public class CircleWaver : MonoBehaviour
{
    CircleBender circleBender;
    [SerializeField] float MaxRadius;
    [SerializeField] float MinRadius;
    [SerializeField] float duration = 1;
    // Start is called before the first frame update
    void Start()
    {
        circleBender = GetComponent<CircleBender>();
        circleBender.radius = MaxRadius;
        circleBender.UpdateLine();

        DOTween.To(() => circleBender.radius, (x) => { circleBender.radius = x; circleBender.UpdateLine(); }, MinRadius, duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
