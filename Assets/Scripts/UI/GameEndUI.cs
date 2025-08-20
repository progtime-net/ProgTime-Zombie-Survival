using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEndUI : MonoBehaviour
{
    public GameObject panel;                  // ���� ������
    public Transform rowsParent;              // ��������� ��� ����� (� Vertical Layout Group)
    public GameObject playerRowPrefab;        // ������ ������
    public TMP_Text wavesText;                // ��������� ����� ��� ����

    public void ShowResults(List<PlayerController> players, int wavesSurvived)
    {
        panel.SetActive(true);

        // ������� ������ ������ ���� ����
        foreach (Transform child in rowsParent)
        {
            Destroy(child.gameObject);
        }

        // ������ ������ �������
        foreach (var p in players)
        {
            GameObject row = Instantiate(playerRowPrefab, rowsParent);
            PlayerRowUI rowUI = row.GetComponent<PlayerRowUI>();
            rowUI.SetData(p.name, p.score, p.deaths);
        }

        // ������������� ���������� ����
        wavesText.text = $"�����: {wavesSurvived}";
    }
}
