using TMPro;
using UnityEngine;

public class MenuPopupCntrl : MonoBehaviour
{
    public static MenuPopupCntrl Instance { get; private set; }

    [Header("UI")]
    public GameObject panel;
    public TMP_Text messageText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(string message)
    {
        messageText.text = message;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
