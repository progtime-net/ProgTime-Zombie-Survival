using TMPro;
using UnityEngine;

public class UIOnePlayerResultPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void SetPlayer(PlayerController playerController, int leaderBoardPos)
    {
        scoreText.text = $"{playerController.score}";
        nameText.text = playerController.name;
        idText.text = $"{leaderBoardPos}.";

    }

}
