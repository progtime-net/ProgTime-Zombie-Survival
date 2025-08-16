using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image BloodLevel;
    public Image BloodLevelBckgr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    } 

    // ¬ариант 1 Ч полоска здоровь€ через fill (самый удобный)
    public void SetHealth(float t) // t в [0..1]
    {
        t = Mathf.Clamp01(t);
        BloodLevel.type = Image.Type.Filled;
        BloodLevel.fillMethod = Image.FillMethod.Horizontal;
        BloodLevel.fillOrigin = (int)Image.OriginHorizontal.Left;
        BloodLevel.fillAmount = t;  // мен€ет УдлинуФ без перелэйаута
    }
     
}
