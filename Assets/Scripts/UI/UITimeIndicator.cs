using Mirror;
using TMPro;
using UnityEngine;

public class UITimeIndicator : MonoBehaviour
{

    private float _timeLeft = 0;
    [SerializeField] private TextMeshProUGUI timerText;
    private string _baseTimerText;
    private float _timeSnapshot;
    void Start()
    {
        _baseTimerText = timerText.text;
        timerText.text = _baseTimerText.Replace("XXX", _timeLeft.ToString());
    }
    public void StartTimer(int seconds)
    {
        _timeLeft = seconds;
        _timeSnapshot = Time.time;

        UpdateTimer(); 
        InvokeRepeating(nameof(UpdateTimer), 0f, 0.5f); 
    }

    private void UpdateTimer()
    {
        int _left = Mathf.CeilToInt(Mathf.Max(0f, _timeLeft - (Time.time - _timeSnapshot)));
        timerText.text = _baseTimerText.Replace("XXX", _left.ToString());

        if (_left <= 0)
        {
            CancelInvoke(nameof(UpdateTimer)); 
        }
    }



}
