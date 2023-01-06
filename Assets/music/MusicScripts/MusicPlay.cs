using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlay : MonoBehaviour
{
    public int playTime = 0;//BGM‚ğ—¬‚µ‚½‰ñ”
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip BGM = null;//BGM‚Ì‰¹Šy
    [SerializeField] AudioClip Great = null;//‚f‚q‚d‚`‚s”»’è‚Ì‰¹Šy
    [SerializeField] AudioClip Good = null;//‚f‚n‚n‚c”»’è‚Ì‰¹Šy
    [SerializeField] AudioClip Bad = null;//‚a‚`‚c”»’è‚Ì‰¹Šy
    AudioSource Audio;//AudioSource
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(playTime==0)//Ä¶‰ñ”‚ª0‰ñ‚¾‚Á‚½
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PlayMusic();//‚a‚f‚lÄ¶
                playTime = 1;//Ä¶‚µ‚½‰ñ”‚ğ1‚É
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))//1‚ª‰Ÿ‚³‚ê‚½‚ç
        {
            PlayGreat();//GreatÄ¶
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))//2‚ª‰Ÿ‚³‚ê‚½‚ç
        {
            PlayGood();//GoodÄ¶
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))//3‚ª‰Ÿ‚³‚ê‚½‚ç
        {
            PlayBad();//BadÄ¶
        }
    }

    public void PlayMusic()//BGM‚ÌÄ¶‚ÌŠÖ”
    {
        
        audioSource.PlayOneShot(BGM);
    }
    public void PlayGreat()//Great”»’è‚ÌSEÄ¶
    {
        
        audioSource.PlayOneShot(Great);
    }
    public void PlayGood()//Good”»’è‚ÌSEÄ¶
    {
        
        audioSource.PlayOneShot(Good);
    }
    public void PlayBad()//Bad”»’è‚ÌSEÄ¶
    {
        
        audioSource.PlayOneShot(Bad);
    }
}
