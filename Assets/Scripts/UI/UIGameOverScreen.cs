using System;
using System.Linq;
using UnityEngine;

public class UIGameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject UIOnePlayerResultPanelPrefab;
    [SerializeField] private GameObject listðHolderOfPlayersScores;
    [SerializeField] private Animator animator;

    public static UIGameOverScreen Instance { get; internal set; }

    private void Start()
    {
        Instance = this;
    }
    public void Show()//÷àñ ñîðîê íî÷è. Ñîí - õîðîøàÿ èäåÿ!
    {
        animator.SetBool("isOpen", true);

        int _pos = 1;
        foreach (var item in GameManager.Instance.AllPlayers.OrderBy(x=>x.score))
        {
            print(item);
            var panel = Instantiate(UIOnePlayerResultPanelPrefab, listðHolderOfPlayersScores.transform);
            print(panel);
            if (panel is null)
            {
                print("null panel");
                continue;
            }

            panel.GetComponent<UIOnePlayerResultPanel>().SetPlayer(item, _pos++);
            print("done");
        }
        print("THE DONE");
    }
#if UNITY_EDITOR
    // Editor helpers (works in play mode). Uses server path if available.
    [ContextMenu("Show")]
    void ContextShow()
    {
        Show();
    } 
#endif

}
