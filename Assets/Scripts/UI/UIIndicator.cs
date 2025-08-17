using UnityEngine; 
using UnityEngine.UI;

public class UIIndicator : MonoBehaviour
{
    public Image IndicatorLevel;
    public Image IndicatorLevelFollow; //gray following thing below main color indicator
    public Image IndicatorLevelBckgr;

    private float IndicatorTarget = 1;
    public float IndicatorCurrent = 1;
    private float IndicatorFollowCurrent = 1;

    public float MainLerpValue = 0.2f;
    public float FolloLerpValue = 0.02f;

    void Start()
    {
        IndicatorLevel.type = Image.Type.Filled;
        IndicatorLevel.fillMethod = Image.FillMethod.Horizontal;
        IndicatorLevel.fillOrigin = (int)Image.OriginHorizontal.Left;



        IndicatorLevelFollow.type = Image.Type.Filled;
        IndicatorLevelFollow.fillMethod = Image.FillMethod.Horizontal;
        IndicatorLevelFollow.fillOrigin = (int)Image.OriginHorizontal.Left;

    }
    void Update()
    {
        SetValueIndicator(IndicatorCurrent);
        SetValueIndicatorFollow(IndicatorFollowCurrent);
        IndicatorCurrent = Mathf.Lerp(IndicatorCurrent, IndicatorTarget, MainLerpValue);
        IndicatorFollowCurrent = Mathf.Lerp(IndicatorFollowCurrent, IndicatorCurrent, FolloLerpValue);
    }
    private void SetValueIndicator(float t) // t â [0..1]
    { 
        t = Mathf.Clamp01(t);
        IndicatorLevel.rectTransform.anchorMax = new Vector2(t, IndicatorLevel.rectTransform.anchorMax.y);

    }
    private void SetValueIndicatorFollow(float t) // t â [0..1]
    { 
        t = Mathf.Clamp01(t);
        IndicatorLevelFollow.rectTransform.anchorMax = new Vector2(t, IndicatorLevelFollow.rectTransform.anchorMax.y);

    }



    public void SetValue(float t)
    {
        IndicatorTarget = t;
    }
    public void ResetIndicator()
    {
        IndicatorTarget = 1;
    }


}
