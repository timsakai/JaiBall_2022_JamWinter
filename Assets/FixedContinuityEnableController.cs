using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FixedContinuityEnableController : MonoBehaviour
{
    [SerializeField] bool Invert = false;
    [SerializeField] bool flagPrev = false;
    [SerializeField] float enableInert = 0.1f;
    float timer = 0f;
    [SerializeField] GameObject influence;
    // Start is called before the first frame update
    void Start()
    {
        influence.SetActive(Invert);
        //gameObject.SetActive(Invert);
    }

    private void FixedUpdate()
    {
        if (flagPrev)
        {
            influence.SetActive(true);
            timer = enableInert;

        }

        if (timer <= 0 && !flagPrev)
        {
            influence.SetActive(false);
        }
        //gameObject.SetActive(flagPrev);
        flagPrev = Invert;
        //Debug.Log("Fixedupd");
    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
    }

    public void UpdateFlag()
    {
        flagPrev = !Invert;
    }
}
