using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SearchService;
using Unity.VisualScripting;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text team1;
    [SerializeField]
    private TMP_Text team2;
    [SerializeField]
    private TMP_Text clear1;
    [SerializeField]
    private TMP_Text clear2;
    [SerializeField]
    private Image[] scoreImages;
    [SerializeField]private Image[] scoreImagesTeam2;

    List<GameObject> team1ImageList = new List<GameObject>();


    [SerializeField]
    private Sprite[] sourceSprites;
    private int score1 = 0;
    private int totalScore1 = 0;
    private int score2 = 0;
    private int totalScore2 = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        
        team1.SetText("team1");
        team2.SetText("team2");
        clear1.enabled = false;
        clear2.enabled = false;
        fruitsShuffle();
        fruitsShuffleTeam2();
    }
    // Update is called once per frame
    void Update()
    {
        if (score1 > 100)
        {
            clear1.enabled = false;
            scoreImages[0].enabled = true;
        }
        if (score1 > 200)
        {
            scoreImages[1].enabled = true;
        }
        if (score1 > 300)
        {
            scoreImages[2].enabled = true;
        }
        if (score1 > 400)
        {
            scoreImages[3].enabled = true;
            
        }
        if (score1 > 500)
        {
            score1 = 500;
            scoreImages[4].enabled = true;
        }

        if (score2 > 100)
        {
            clear1.enabled = false;
            scoreImagesTeam2[0].enabled = true;
        }
        if (score2 > 200)
        {
            scoreImagesTeam2[1].enabled = true;
        }
        if (score2 > 300)
        {
            scoreImagesTeam2[2].enabled = true;
        }
        if (score2 > 400)
        {
            scoreImagesTeam2[3].enabled = true;

        }
        if (score2 > 500)
        {
            score2 = 500;
            scoreImagesTeam2[4].enabled = true;
        }

    }

    private void fruitsShuffle()
    {
        for (int i = 0; i < scoreImages.Length; i++)
        {
            scoreImages[i].sprite = sourceSprites[Random.Range(0, sourceSprites.Length)];
        }
    }

    private void fruitsShuffleTeam2()
    {
        for (int i = 0; i < scoreImagesTeam2.Length; i++)
        {
            scoreImagesTeam2[i].sprite = sourceSprites[Random.Range(0, sourceSprites.Length)];
        }
    }

    public void countScoreGood()//BEAT¬Œ÷Žž
    {
        score1+=30;
    }

    public void countScoreBad()//BEATŽ¸”sŽž
    {
        score1 += 5;
    }

    public void countScoreGoodTeam2()
    {
        score2 += 30;
    }
    public void countScoreBadTeam2()
    {
        score2 += 5;
    }
    public void Clear1()
    {
        
        clear1.enabled = true;
        scoreImages[0].enabled = false;
        scoreImages[1].enabled = false;
        scoreImages[2].enabled = false;
        scoreImages[3].enabled = false;
        scoreImages[4].enabled = false;
        totalScore1 += score1;
        score1 = 0;
        fruitsShuffle();
    }
    public void Clear2()
    {

        clear1.enabled = true;
        scoreImages[0].enabled = false;
        scoreImages[1].enabled = false;
        scoreImages[2].enabled = false;
        scoreImages[3].enabled = false;
        scoreImages[4].enabled = false;
        totalScore1 += score1;
        score1 = 0;
        fruitsShuffleTeam2();
    }
}
