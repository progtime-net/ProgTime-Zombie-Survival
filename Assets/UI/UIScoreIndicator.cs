using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreIndicator : MonoBehaviour
{

    private float scoreCurrent = 0;
    private float scoreTarget = 0;
    private string initialTextValue;
    [SerializeField] private float scoreAddSpeed = 0.02f;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void AddScore(float score)
    {
        scoreTarget += score;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialTextValue = scoreText.text;
        scoreText.text = initialTextValue.Replace("XXX", "0");
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(scoreCurrent - scoreTarget) < 0.1)
            return;

        if (Math.Abs(scoreCurrent - scoreTarget) < 0.4) 
            scoreCurrent = scoreTarget;  
        scoreCurrent = Mathf.Lerp(scoreCurrent, scoreTarget, scoreAddSpeed);
        scoreText.text = initialTextValue.Replace("XXX", Math.Floor(scoreCurrent).ToString());
    }
}
