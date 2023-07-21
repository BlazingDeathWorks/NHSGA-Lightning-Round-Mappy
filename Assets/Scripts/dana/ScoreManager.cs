using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Zero out score after congrats seen
public class ScoreManager : MonoBehaviour
{
    public static int Score => score;
    public static int HighScore => highScore;
    public static ScoreManager Instance = null;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private static int score = 0;
    [SerializeField] private static int highScore = 20000;

    public void SetHighscore(int thing)
    {
        highScore = thing;
    }
    



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        
    }

    private void Start()
    {
        scoreText.SetText(score.ToString());
        highScore = PlayerPrefs.GetInt("High Score");
        if (highScore == 0) highScore = 20000;
        highScoreText.SetText(highScore.ToString());
    }



    public void IncreaseScoreTram()
    {
        score += 10;
        scoreText.SetText(score.ToString());
    }

    public void IncreaseScoreHit()
    {
        score += 50;
        scoreText.SetText(score.ToString());
    }

    public void IncreaseScoreHitMicroWave(int sc)
    {
        score += sc;
        scoreText.SetText(score.ToString());
    }



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
