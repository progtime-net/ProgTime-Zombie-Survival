using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreIndicator : MonoBehaviour
{

    private float ScoreCurrent = 0;
    private float ScoreTarget = 0;
    private string InitialTextValue;
    public float ScoreAddSpeed = 0.02f;
    public TextMeshProUGUI ScoreText;

    public void AddScore(float score)
    {
        ScoreTarget += score;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitialTextValue = ScoreText.text;
        ScoreText.text = InitialTextValue.Replace("XXX", "0");
    }

    // Update is called once per frame
    void Update()
    {
        print($"ScoreCurrent - ScoreTarget: {ScoreCurrent - ScoreTarget}");
        if (Math.Abs(ScoreCurrent - ScoreTarget) < 0.1)
            return;

        if (Math.Abs(ScoreCurrent - ScoreTarget) < 0.4) 
            ScoreCurrent = ScoreTarget;  
        ScoreCurrent = Mathf.Lerp(ScoreCurrent, ScoreTarget, ScoreAddSpeed);
        ScoreText.text = InitialTextValue.Replace("XXX", Math.Floor(ScoreCurrent).ToString());
    }
}
