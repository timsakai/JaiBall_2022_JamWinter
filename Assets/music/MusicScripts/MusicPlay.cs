using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlay : MonoBehaviour
{
    public int playTime = 0;//BGM�𗬂�����
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip BGM = null;//BGM�̉��y
    [SerializeField] AudioClip Great = null;//�f�q�d�`�s����̉��y
    [SerializeField] AudioClip Good = null;//�f�n�n�c����̉��y
    [SerializeField] AudioClip Bad = null;//�a�`�c����̉��y
    AudioSource Audio;//AudioSource
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(playTime==0)//�Đ��񐔂�0�񂾂�����
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PlayMusic();//�a�f�l�Đ�
                playTime = 1;//�Đ������񐔂�1��
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))//1�������ꂽ��
        {
            PlayGreat();//Great�Đ�
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))//2�������ꂽ��
        {
            PlayGood();//Good�Đ�
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))//3�������ꂽ��
        {
            PlayBad();//Bad�Đ�
        }
    }

    public void PlayMusic()//BGM�̍Đ��̊֐�
    {
        
        audioSource.PlayOneShot(BGM);
    }
    public void PlayGreat()//Great�����SE�Đ�
    {
        
        audioSource.PlayOneShot(Great);
    }
    public void PlayGood()//Good�����SE�Đ�
    {
        
        audioSource.PlayOneShot(Good);
    }
    public void PlayBad()//Bad�����SE�Đ�
    {
        
        audioSource.PlayOneShot(Bad);
    }
}
