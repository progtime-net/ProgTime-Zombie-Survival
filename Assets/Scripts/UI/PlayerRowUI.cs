using TMPro;
using UnityEngine;

public class PlayerRowUI : MonoBehaviour
{
    public TMP_Text playerNameText;
    public TMP_Text scoreText;
    public TMP_Text deathsText;

    public void SetData(string name, int score, int deaths)
    {
        playerNameText.text = name;
        scoreText.text = score.ToString();
        deathsText.text = deaths.ToString();
    }
}
