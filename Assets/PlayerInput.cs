using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
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
        controller.inputJump = Input.GetAxis("LeftTrigger");
    }

    public void InputShoot(TPSController controller)
    {
        controller.inputShoot = Input.GetAxis("LeftBumper");
    }

    public void InputCharge(TPSController controller)
    {
        controller.inputCharge = Input.GetAxis("RightBumper");
    }
    public void InputSprint(TPSController controller)
    {
        controller.inputSprint = Input.GetAxis("RightTrigger");
    }
    public void InputLocomotion(TPSController controller)
    {
        controller.inputHorizontal = Input.GetAxis("Horizontal");
        controller.inputVertical = Input.GetAxis("Vertical");
    }
}
