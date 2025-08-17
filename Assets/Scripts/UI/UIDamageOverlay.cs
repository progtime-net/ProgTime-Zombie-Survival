using UnityEngine;
using UnityEngine.UI;

public class UIDamageOverlay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float decaySpeed = 0.99f;
    [SerializeField] private float alphaMultiplierBase = 0.7f;
    [SerializeField] private float alphaMultiplierSinusoid = 0.2f;
    private float _accumulatedDamage = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image.color = new Color(1,0,0,0);

    }

    // Update is called once per frame

    void Update()
    {
        if (_accumulatedDamage == 0)
            return;
        
        if (_accumulatedDamage < 0.003)
            _accumulatedDamage = 0;
        
        image.color = new Color(1,0,0, _accumulatedDamage 
            * alphaMultiplierBase
            + alphaMultiplierSinusoid 
            * Mathf.Clamp01(_accumulatedDamage) 
            * Mathf.Sin(Time.time * 3.4f + 2)
            * Mathf.Cos(Time.time * 5));
        _accumulatedDamage *= decaySpeed;
    }

    public void AddDamage(float damage)
    {
        _accumulatedDamage += damage;
    }
}
