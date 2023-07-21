using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CongratsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;

    private void Awake()
    {
        if (ScoreManager.Score > ScoreManager.HighScore)
        {
            ScoreManager.Instance.SetHighscore(ScoreManager.Score);
            PlayerPrefs.SetInt("High Score", ScoreManager.Score);
        }
        if (highScoreText == null) return;
        highScoreText.text = "Highscore: " + ScoreManager.HighScore;
    }
}
