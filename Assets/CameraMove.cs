using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //private int state = 0;
    [Serializable]
    private struct targets
    {
        //move�֐��̂��߂̈ʒu�擾
        public GameObject moveTarget;
        // �Ǐ]�Ώ� None�ɂ����move�֐����K�p�����
        public Transform follow;
        // �Ə������킹��Ώ�
        public Transform lookAt;
    }

    private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private targets[] targetList;
    private int targetNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))//���łP�L�[�������ƃJ�������o
        {
            move1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))//���łQ�L�[�������ƃJ�������o
        {
            move2();
        }

        if (targetList != null && targetList.Length > 0 && Input.GetAxis("LeftPush") > 0.1)
        {
            if (++targetNum >= targetList.Length) targetNum=0;//�E�N���b�N�ŏ��ԂɃJ�����̃X�e�[�^�X�؂�ւ���

        }

        var target = targetList[targetNum];//�ύX���Z�b�g
        virtualCamera.Follow = target.follow;
        virtualCamera.LookAt = target.lookAt;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }




    void move1()//���ʂ���̃J�������o
    {
        Vector3 target = targetList[targetNum].moveTarget.transform.position;
        this.transform.DOMove(new Vector3(target.x,target.y,target.z), 0f);
        this.transform.DOMove(new Vector3(-2.2f, 0.7f, -1.8f), 0f).SetRelative();
        this.transform.DOMove(new Vector3(0f, 0.5f, 1.8f), 7f).SetRelative();
    }

    void move2()//�����낵�J�������o
    {
        Vector3 target = targetList[targetNum].moveTarget.transform.position;
        this.transform.DOMove(new Vector3(target.x, target.y, target.z), 0f);
        this.transform.DOMove(new Vector3(3.2f, 4.2f, -1.8f),0f).SetRelative();
        this.transform.DOMove(new Vector3(0f, 0f, 4.8f), 7f).SetRelative();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(100, 1000, 200, 50), "[Tab]:ChangeCam");
    }

}
