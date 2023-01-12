using RootMotion.Demos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [SerializeField] bool freeze = false;
    [SerializeField] BeatCircleManager beatCircleManager;
    [SerializeField] UnityEvent OnGood = new UnityEvent();
    [SerializeField] UnityEvent OnBad = new UnityEvent();

    TPSController tpsController;
    // Start is called before the first frame update
    void Start()
    {
        tpsController = GetComponent<TPSController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!freeze)
        {
            if (tpsController.hasBall)
            {
                Vector3 relate = tpsController.viewPoint.InverseTransformPoint(tpsController.target.transform.position);
                int beat = beatCircleManager.JudgeTiming(0.1f);
                if (beat == 0)
                {
                    if (relate.z >= 8)
                    {
                        tpsController.Dance();
                        OnGood.Invoke();
                    }
                }
                
            }
        }
    }

    public void InputJump(TPSController controller)
    {
        //controller.inputJump = Input.GetAxis("LeftTrigger");
    }

    public void InputShoot(TPSController controller)
    {
        //controller.inputShoot = Input.GetAxis("LeftBumper");
    }

    public void InputCharge(TPSController controller)
    {
        if (!freeze)
        {
            controller.inputCharge = 0;
            //controller.inputCharge = Input.GetAxis("RightBumper");
            Vector3 relate = controller.viewPoint.InverseTransformPoint(controller.target.transform.position);
            if (!controller.hasBall)
            {
                if (relate.z <= 8.2)
                {
                    controller.inputCharge = 1;
                }
            }
        }
    }
    public void InputSprint(TPSController controller)
    {
        //controller.inputSprint = Input.GetAxis("RightTrigger");
    }
    public void InputLocomotion(TPSController controller)
    {
        if (!freeze)
        {

            //controller.inputHorizontal = Input.GetAxis("Horizontal");
            //controller.inputVertical = Input.GetAxis("Vertical");
            Vector3 relate = controller.viewPoint.InverseTransformPoint(controller.target.transform.position);
            if (!controller.hasBall)
            {
                if (relate.z >= 8)
                {
                    controller.inputVertical = -1.0f;
                    if ((Time.time % 2.0) <= (0.5 * 2.0))
                    {
                        controller.inputHorizontal = 1.0f;
                    }
                    else
                    {
                        controller.inputHorizontal = -1.0f;
                    }
                }
                else if (relate.z >= 4)
                {
                    controller.inputVertical = -1.0f;
                }
            }
            else
            {
                if (relate.z <= 8)
                {
                    controller.inputVertical = 1.0f;
                }
            }
        }
    }
}
