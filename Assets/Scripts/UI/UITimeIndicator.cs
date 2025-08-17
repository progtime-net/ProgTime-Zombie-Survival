using TMPro;
using UnityEngine;

public class UITimeIndicator : MonoBehaviour
{

    private int _timeLeft = 100;
    [SerializeField] private TextMeshProUGUI timerText;
    private string _baseTimerText;

    void Start()
    {
        _baseTimerText = timerText.text;
        timerText.text = timerText.text.Replace("XXX", _timeLeft.ToString());
    }
    public void StartTimer(int seconds)
    {
        InvokeRepeating("UpdateTimer", 0, seconds);
        //yield return new WaitForSeconds(seconds);
    }

    private void UpdateTimer()
    {
        timerText.text = timerText.text.Replace("XXX", _timeLeft.ToString());

    }


}
