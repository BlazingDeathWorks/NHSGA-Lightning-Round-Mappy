using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Zero out score after congrats seen
public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance = null;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private static int score = 0;
    [SerializeField] private static int highScore;
    private int randomNumber;



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        scoreText.SetText(score.ToString());
        highScore = 20000;
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

    public void IncreaseScoreHitMicroWave()
    {
        randomNumber = Random.Range(1, 5);
        score += 100 * randomNumber;
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
