using UnityEngine;
using Mirror;

public class DayNightCycle : NetworkBehaviour
{
    [Header("Skybox")]
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Light sun;

    [Header("Settings")]
    [SerializeField] private float cycleDuration = 600f;
    [SerializeField]
    [SyncVar(hook = nameof(OnTimeChanged))]
    private float timeOfDay = 0f;

    private float _timer = 0f;

    void Update()
    {
        if (isServer)
        {
            _timer += Time.deltaTime;
            timeOfDay = (_timer % cycleDuration) / cycleDuration;
        }

        UpdateCycle(timeOfDay);
    }

    void OnTimeChanged(float oldValue, float newValue)
    {
        UpdateCycle(newValue);
    }

    void UpdateCycle(float t)
    {
        if (skyboxMaterial == null) return;
        float blend = Mathf.Sin(t * Mathf.PI);
        skyboxMaterial.SetFloat("_Blend", blend);
        UpdateSun(t, blend);
        DynamicGI.UpdateEnvironment();
    }

    void UpdateSun(float t, float blend)
    {
        if (sun == null) return;

        sun.transform.rotation = Quaternion.Euler(new Vector3((t * 360f) - 90f, 170f, 0));
        sun.intensity = Mathf.Lerp(1f, 0f, blend);
        sun.enabled = sun.intensity > 0.05f;
    }
}