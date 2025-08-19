using System;
using UnityEngine;
using Mirror;
        
public class DayNightCycle : NetworkBehaviour
{
    [Header("Skybox")]
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Light sun;
        
    [Header("Transition")]
    [SerializeField] private float transitionDuration = 2f;
    [SerializeField] private float daySunIntensity = 1f;
    [SerializeField] private float nightSunIntensity = 0f;
        
    [Header("State")]
    [SyncVar(hook = nameof(OnIsDayChanged))]
    [SerializeField] private bool isDay = true;
    
        
    private float _currentBlend; 
    private float _startBlend;
    private float _targetBlend;
    private float _progress; 
    private bool _isTransitioning;
        
    public override void OnStartClient()
    {
        _currentBlend = isDay ? 0f : 1f;
        ApplyBlend(_currentBlend);
    }
        
    void Update()
    {
        if (!_isTransitioning) return;
        
        _progress += Time.deltaTime / Mathf.Max(0.0001f, transitionDuration);
        float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(_progress));
        _currentBlend = Mathf.Lerp(_startBlend, _targetBlend, t);
        ApplyBlend(_currentBlend);
        
        if (_progress >= 1f) _isTransitioning = false;
    }
        
    void OnIsDayChanged(bool oldValue, bool newValue)
    {
        _startBlend = _currentBlend;
        _targetBlend = newValue ? 0f : 1f;
        _progress = 0f;
        _isTransitioning = true;
    }
        
    void ApplyBlend(float blend)
    {
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_Blend", Mathf.Clamp01(blend));
            DynamicGI.UpdateEnvironment();
        }
        
        if (sun != null)
        {
            sun.intensity = Mathf.Lerp(daySunIntensity, nightSunIntensity, Mathf.Clamp01(blend));
            sun.enabled = sun.intensity > 0.05f;
        }
    }
        
    [Server]
    public void SetIsDay(bool value)
    {
        isDay = value;
    }
        
    [Command(requiresAuthority = false)]
    public void CmdSetIsDay(bool value)
    {
        isDay = value;
    }
    
        
#if UNITY_EDITOR
    // Editor helpers (works in play mode). Uses server path if available.
    [ContextMenu("Set Day")]
    void ContextSetDay()
    {
        if (isServer) SetIsDay(true);
        else CmdSetIsDay(true);
    }
        
    [ContextMenu("Set Night")]
    void ContextSetNight()
    {
        if (isServer) SetIsDay(false);
        else CmdSetIsDay(false);
    }
#endif
}