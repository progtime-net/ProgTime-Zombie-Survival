using UnityEngine;
using UnityEngine.UI;

public class UIDamageOverlay : MonoBehaviour
{
    public Image image;
    public float DecaySpeed = 0.99f; 
    public float AlphaMultiplierBase = 0.7f;
    public float AlphaMultiplierSinusoid = 0.2f;
    private float AccumulatedDamage = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image.color = new Color(1,0,0,0);

    }

    // Update is called once per frame

    void Update()
    {
        if (AccumulatedDamage == 0)
            return;
        
        if (AccumulatedDamage < 0.003)
            AccumulatedDamage = 0;
        
        image.color = new Color(1,0,0, AccumulatedDamage 
            * AlphaMultiplierBase
            + AlphaMultiplierSinusoid 
            * Mathf.Clamp01(AccumulatedDamage) 
            * Mathf.Sin(Time.time * 3.4f + 2)
            * Mathf.Cos(Time.time * 5));
        AccumulatedDamage *= DecaySpeed;
    }

    public void AddDamage(float damage)
    {
        AccumulatedDamage += damage;
    }
}
