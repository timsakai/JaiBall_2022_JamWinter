using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] bool freeze = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
