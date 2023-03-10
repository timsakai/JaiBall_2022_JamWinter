using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlay : MonoBehaviour
{
    public int playTime = 0;//BGMを流した回数
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip BGM = null;//BGMの音楽
    [SerializeField] AudioClip Great = null;//ＧＲＥＡＴ判定の音楽
    [SerializeField] AudioClip Good = null;//ＧＯＯＤ判定の音楽
    [SerializeField] AudioClip Bad = null;//ＢＡＤ判定の音楽
    AudioSource Audio;//AudioSource
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(playTime==0)//再生回数が0回だった時
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PlayMusic();//ＢＧＭ再生
                playTime = 1;//再生した回数を1に
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))//1が押されたら
        {
            PlayGreat();//Great再生
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))//2が押されたら
        {
            PlayGood();//Good再生
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))//3が押されたら
        {
            PlayBad();//Bad再生
        }
    }

    public void PlayMusic()//BGMの再生の関数
    {
        
        audioSource.PlayOneShot(BGM);
    }
    public void PlayGreat()//Great判定のSE再生
    {
        
        audioSource.PlayOneShot(Great);
    }
    public void PlayGood()//Good判定のSE再生
    {
        
        audioSource.PlayOneShot(Good);
    }
    public void PlayBad()//Bad判定のSE再生
    {
        
        audioSource.PlayOneShot(Bad);
    }
}
