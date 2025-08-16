using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image BloodLevel;
    public Image BloodLevelBckgr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BloodLevel.type = Image.Type.Filled;
        BloodLevel.fillMethod = Image.FillMethod.Horizontal;
        BloodLevel.fillOrigin = (int)Image.OriginHorizontal.Left;

    }

    // Update is called once per frame
    void Update()
    {
        print(BloodTarget);
        print(BloodCurrent);
        SetHealth(BloodCurrent);
        BloodCurrent = (BloodTarget + BloodCurrent) / 2;
    }


    float BloodTarget = 1;
    float BloodCurrent = 1;

    // ¬ариант 1 Ч полоска здоровь€ через fill (самый удобный)
    public void SetHealth(float t) // t в [0..1]
    {
        print(t);
        t = Mathf.Clamp01(t);
        BloodLevel.rectTransform.anchorMax = new Vector2(t, BloodLevel.rectTransform.anchorMax.y);

    }

    public void ResetHealth()
    {
        BloodTarget = 1;
    }
    public void MinusHealth()
    {
        BloodTarget -= 0.1f;
    }


}
