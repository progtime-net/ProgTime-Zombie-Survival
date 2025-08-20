using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEndUI : MonoBehaviour
{
    public GameObject panel;                  // Сама панель
    public Transform rowsParent;              // Контейнер для строк (с Vertical Layout Group)
    public GameObject playerRowPrefab;        // Префаб строки
    public TMP_Text wavesText;                // Отдельный текст для волн

    public void ShowResults(List<PlayerController> players, int wavesSurvived)
    {
        panel.SetActive(true);

        // Удаляем старые строки если есть
        foreach (Transform child in rowsParent)
        {
            Destroy(child.gameObject);
        }

        // Создаём строки игроков
        foreach (var p in players)
        {
            GameObject row = Instantiate(playerRowPrefab, rowsParent);
            PlayerRowUI rowUI = row.GetComponent<PlayerRowUI>();
            rowUI.SetData(p.name, p.score, p.deaths);
        }

        // Устанавливаем количество волн
        wavesText.text = $"Волны: {wavesSurvived}";
    }
}
