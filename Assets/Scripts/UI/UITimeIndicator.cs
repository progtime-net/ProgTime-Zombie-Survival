using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UITimeIndicator : MonoBehaviour
{
    private float _timeLeft = 0;
    [SerializeField] private TextMeshProUGUI timerText;
    private string _baseTimerText;

    void Start()
    {
        _baseTimerText = timerText.text;
        timerText.text = _baseTimerText.Replace("XXX", _timeLeft.ToString());
        GameManager.Instance.OnTimerUpdate += UpdateTimer;
    }

    void UpdateTimer(int seconds)
    {
        timerText.text = _baseTimerText.Replace("XXX", seconds.ToString());
    }
}
