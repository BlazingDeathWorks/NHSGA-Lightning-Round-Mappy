using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance = null;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private int score = 0;
    [SerializeField] private int highScore;



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        highScore = 20000; 
        highScoreText.SetText(highScore.ToString());
    }



    public void IncreaseScoreTram()
    {
        score += 10;
        scoreText.SetText(score.ToString());
    }

    //public void IncreaseScoreHit()
    //{
    //    score += 20;
    //    text.SetText(score.ToString());
    //}

    

    public void IncreaseScoreItem(int sc, bool isCombo)
    {
        int comboTimes = items.combo;
        Debug.Log(comboTimes);
        if (isCombo)
        {
            score += sc * comboTimes;
        }
        else
        {
            score += sc;
        }

        Debug.Log(score);
        scoreText.SetText(score.ToString());
    }

    
}
