using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject pauseMenuPanel;
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

    // ������� 1 � ������� �������� ����� fill (����� �������)
    public void SetHealth(float t) // t � [0..1]
    {
        t = Mathf.Clamp01(t);
        BloodLevel.type = Image.Type.Filled;
        BloodLevel.fillMethod = Image.FillMethod.Horizontal;
        BloodLevel.fillOrigin = (int)Image.OriginHorizontal.Left;
        BloodLevel.fillAmount = t;  // ������ ������ ��� �����������
    }

    public void Remuse()
    {
        pauseMenuPanel.SetActive(false);
    }

    public void OpenSettig()
    {
        
    }

    public void Exit()
    {
        
    }

}
