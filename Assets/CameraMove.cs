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
        //move関数のための位置取得
        public GameObject moveTarget;
        // 追従対象 Noneにするとmove関数が適用される
        public Transform follow;
        // 照準を合わせる対象
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

        if (Input.GetKeyDown(KeyCode.Alpha1))//仮で１キーを押すとカメラ演出
        {
            move1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))//仮で２キーを押すとカメラ演出
        {
            move2();
        }

        if (targetList != null && targetList.Length > 0 && Input.GetAxis("LeftPush") > 0.1)
        {
            if (++targetNum >= targetList.Length) targetNum=0;//右クリックで順番にカメラのステータス切り替える

        }

        var target = targetList[targetNum];//変更をセット
        virtualCamera.Follow = target.follow;
        virtualCamera.LookAt = target.lookAt;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }




    void move1()//正面からのカメラ演出
    {
        Vector3 target = targetList[targetNum].moveTarget.transform.position;
        this.transform.DOMove(new Vector3(target.x,target.y,target.z), 0f);
        this.transform.DOMove(new Vector3(-2.2f, 0.7f, -1.8f), 0f).SetRelative();
        this.transform.DOMove(new Vector3(0f, 0.5f, 1.8f), 7f).SetRelative();
    }

    void move2()//見下ろしカメラ演出
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
