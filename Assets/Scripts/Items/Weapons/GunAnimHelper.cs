using System;
using UnityEngine;

public class GunAnimHelper : MonoBehaviour
{
    private Animator _animator;
    // private Action _reloadCallBack = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayShootAnim()
    {
        _animator.Play("Shoot");
    }

    public void PlayReloadAnim()
    {
        _animator.Play("Reload");
    }
}
