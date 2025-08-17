using UnityEngine;
using TMPro;

public class PlayerPlashController: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nicknameTxt;
    [SerializeField] private TextMeshProUGUI pingTxt;
    [SerializeField] private GameObject adminIcon;
    [SerializeField] private GameObject kickBtn;

    private LobbyManager _manager;

    private int _playerId;
    private string _playerNickname;
    private bool _isAdmin;
    private bool _isOnHost;

    public void Init(LobbyManager manager,
        int playerId, 
        string playerNickname, 
        bool isAdmin, bool isOnHost)
    {
        _manager = manager;

        _playerId = playerId;
        _playerNickname = playerNickname;
        _isAdmin = isAdmin;
        _isOnHost = isOnHost;

        nicknameTxt.text = playerNickname;
        adminIcon.SetActive(isAdmin);
        kickBtn.SetActive(isOnHost && !isAdmin);
    }

    public void UpdatePing(string str) => pingTxt.text = str;

    public void KickBtnPressed()
    {
        if (!_isOnHost) return;
        _manager.KickFromUI(_playerId);
    }

    public string GetNickname()
    {
        return nicknameTxt.text;
    }

    public void UpdateNickname(string nickname)
    {
        _playerNickname = nickname;
        nicknameTxt.text = nickname;
    }
}
