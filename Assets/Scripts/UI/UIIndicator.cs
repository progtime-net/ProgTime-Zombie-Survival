using UnityEngine; 
using UnityEngine.UI;

public class UIIndicator : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Image indicatorLevel;
    [SerializeField] private Image indicatorLevelFollow; //gray following thing below main color indicator
    [SerializeField] private Image indicatorLevelBckgr;

    private float _indicatorTarget = 1;
    public float IndicatorCurrent = 1;
    private float _indicatorFollowCurrent = 1;

    [SerializeField] private float mainLerpValue = 0.2f;
    [SerializeField] private float followLerpValue = 0.02f;

    void Start()
    {
        indicatorLevel.type = Image.Type.Filled;
        indicatorLevel.fillMethod = Image.FillMethod.Horizontal;
        indicatorLevel.fillOrigin = (int)Image.OriginHorizontal.Left;
        
        indicatorLevelFollow.type = Image.Type.Filled;
        indicatorLevelFollow.fillMethod = Image.FillMethod.Horizontal;
        indicatorLevelFollow.fillOrigin = (int)Image.OriginHorizontal.Left;

    }
    void Update()
    {
        SetValueIndicator(IndicatorCurrent);
        SetValueIndicatorFollow(_indicatorFollowCurrent);
        IndicatorCurrent = Mathf.Lerp(IndicatorCurrent, _indicatorTarget, mainLerpValue);
        _indicatorFollowCurrent = Mathf.Lerp(_indicatorFollowCurrent, IndicatorCurrent, followLerpValue);
    }
    private void SetValueIndicator(float t) // t � [0..1]
    { 
        t = Mathf.Clamp01(t);
        indicatorLevel.rectTransform.anchorMax = new Vector2(t, indicatorLevel.rectTransform.anchorMax.y);

    }
    private void SetValueIndicatorFollow(float t) // t � [0..1]
    { 
        t = Mathf.Clamp01(t);
        indicatorLevelFollow.rectTransform.anchorMax = new Vector2(t, indicatorLevelFollow.rectTransform.anchorMax.y);

    }



    public void SetValue(float t)
    {
        _indicatorTarget = t;
    }
    public void ResetIndicator()
    {
        _indicatorTarget = 1;
    }


}
