using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [System.Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip[] variations;
    }

    [Header("Sounds")]
    public Sound[] sounds;

    private Dictionary<string, AudioClip[]> _soundDict;
    private AudioSource _audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = gameObject.AddComponent<AudioSource>();
        _soundDict = new Dictionary<string, AudioClip[]>();
        foreach (var sound in sounds)
        {
            _soundDict[sound.name] = sound.variations;
        }
    }

    private AudioClip GetRandomClip(string name)
    {
        if (_soundDict.TryGetValue(name, out var clips) && clips.Length > 0)
        {
            int idx = Random.Range(0, clips.Length);
            return clips[idx];
        }
        return null;
    }

    public void Play(string name)
    {
        var clip = GetRandomClip(name);
        if (clip != null)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Sound '{name}' not found!");
        }
    }

    public void PlayOneShot(string name)
    {
        var clip = GetRandomClip(name);
        if (clip != null)
        {
            _audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Sound '{name}' not found!");
        }
    }
}