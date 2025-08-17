using UnityEngine;
using TMPro;
using System;

public class PlayerPlashController: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nicknameTxt;
    [SerializeField] private TextMeshProUGUI pingTxt;
    [SerializeField] private GameObject adminIcon;
    [SerializeField] private GameObject kickBtn;

    private Action<int> _onKickPlayer;

    private int _playerId;
    private string _playerNickname;
    private bool _isAdmin;
    private bool _isOnHost;

    public void Init(Action<int> onKickPlayer,
        int playerId, 
        string playerNickname, 
        bool isAdmin, bool isOnHost)
    {
        _onKickPlayer = onKickPlayer;

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
        _onKickPlayer?.Invoke(_playerId);
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
