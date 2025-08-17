using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItemSelectorElement : MonoBehaviour
{
    public string itemName;
    private float _startWidth;
    private Image thisImage;
    private bool _isSelected;
    private float _animationTime = 1;
    private float _timeSnapshot;
    public void SetImage(Sprite image)
    {
        thisImage.sprite = image;
    }
    void Start()
    {
        _startWidth = thisImage.rectTransform.rect.width;
    }
    public void Select()
    {
        _isSelected = true;
        _timeSnapshot = Time.time;

        InvokeRepeating(nameof(UpdateSize), 0f, 0.1f);
    }
    public void Deselect()
    {
        _isSelected = false;
        _timeSnapshot = Time.time;

        InvokeRepeating(nameof(UpdateSize), 0f, 0.1f);

    }


    private void UpdateSize()
    {
        int _left = Mathf.CeilToInt(Mathf.Max(0f, _animationTime - (Time.time - _timeSnapshot)));


        //color transition + expansion

        if (_left <= 0)
        {
            CancelInvoke(nameof(UpdateSize));
        }
    }
}
