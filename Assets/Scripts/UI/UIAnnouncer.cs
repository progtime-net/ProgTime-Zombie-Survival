using TMPro;
using UnityEngine;

public class UIAnnouncer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI announcementText;
    public void Announce(string text)
    {
        print(text);
        announcementText.text = text;
        animator.SetTrigger("announce");
    }
}
