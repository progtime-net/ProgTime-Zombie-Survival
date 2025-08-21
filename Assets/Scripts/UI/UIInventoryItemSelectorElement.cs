using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItemSelectorElement : MonoBehaviour
{ 
    private Image thisImage;
    private bool _isSelected; 
    private Animator animator; 
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Select()
    {
        _isSelected = true;
        animator.SetBool("isSelected", true);
         
    }
    public void Deselect()
    {
        _isSelected = false;
        animator.SetBool("isSelected", false); 

    }
    
    internal void ShowElement()
    {
    }

    internal void HideElement()
    {
    }

#if UNITY_EDITOR
    // Editor helpers (works in play mode). Uses server path if available.
    [ContextMenu("Select")]
    void ContextSelect()
    {
        Select();
    }

    [ContextMenu("Deselect")]
    void ContextDeselect()
    {
        Deselect();
    }
#endif
}
