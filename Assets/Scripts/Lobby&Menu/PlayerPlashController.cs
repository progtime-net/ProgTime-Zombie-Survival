using UnityEngine;
using TMPro;

public class PlayerPlashController: MonoBehaviour
{
    public TextMeshProUGUI nicknameTxt;
    public TextMeshProUGUI pingTxt;
    public GameObject adminIcon;
    public GameObject kickBtn;

    private LobbyManager manager;

    private int playerId;
    private string playerNickname;
    private bool isAdmin;
    private bool isOnHost;

    public void Init(LobbyManager manager,
        int playerId, 
        string playerNickname, 
        bool isAdmin, bool isOnHost)
    {
        this.manager = manager;

        this.playerId = playerId;
        this.playerNickname = playerNickname;
        this.isAdmin = isAdmin;
        this.isOnHost = isOnHost;

        nicknameTxt.text = playerNickname;
        adminIcon.SetActive(isAdmin);
        kickBtn.SetActive(isOnHost && !isAdmin);
    }

    public void UpdatePing(string str) => pingTxt.text = str;

    public void KickBtnPressed()
    {
        if (!isOnHost) return;
        manager.KickFromUI(playerId);
    }
}
