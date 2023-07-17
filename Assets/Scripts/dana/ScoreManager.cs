using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance = null;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int score = 0;
    


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

   

    public void IncreaseScoreTram()
    {
        score += 10;
        text.SetText(score.ToString());
    }

    //public void IncreaseScoreHit()
    //{
    //    score += 20;
    //    text.SetText(score.ToString());
    //}

    public void IncreaseScoreItem(int sc, bool isCombo)
    {
        int comboTimes = movement.combo;
        if (isCombo)
        {
            score += sc * comboTimes;
        }
        else
        {
            score += sc;
        }
        
        text.SetText(score.ToString());
    }

    
}
