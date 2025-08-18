using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class EditorUtils : MonoBehaviour
{
    static double startTime;
    static bool showing;

    static EditorUtils()
    {
        EditorApplication.update += ShowFakeLoading;
        startTime = EditorApplication.timeSinceStartup;
        showing = true;
    }

    private static void ShowFakeLoading()
    {
        if (!showing) return;

        double elapsed = EditorApplication.timeSinceStartup - startTime;

        string message = "Initializing Mac_Burner™ 2.0…";

        float progress = Mathf.PingPong((float)elapsed, 1f);

        EditorUtility.DisplayProgressBar("Importing Assets", message, progress);

        if (elapsed > 4)
        {
            EditorUtility.ClearProgressBar();
            showing = false;
            EditorApplication.update -= ShowFakeLoading;
        }
    }
}

[InitializeOnLoad]
public static class EpicMusicPlayer
{
    static EpicMusicPlayer()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/SFX/epic_music.mp3");
            if (clip != null)
            {
                PlayClip(clip);
            }
        }
    }

    static readonly BindingFlags Flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

    static Type AudioUtilType =>
        typeof(AudioImporter).Assembly.GetType("UnityEditor.AudioUtil");

    public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
    {
        if (clip == null) throw new ArgumentNullException(nameof(clip));
        var t = AudioUtilType ?? throw new InvalidOperationException("UnityEditor.AudioUtil type not found.");

        var m =
            t.GetMethod("PlayPreviewClip", Flags, null, new[] { typeof(AudioClip), typeof(int), typeof(bool) }, null) ??
            t.GetMethod("PlayClip", Flags, null, new[] { typeof(AudioClip), typeof(int), typeof(bool) }, null) ??
            t.GetMethod("PlayClip", Flags, null, new[] { typeof(AudioClip) }, null);

        if (m == null)
            throw new MissingMethodException("Could not find AudioUtil.PlayPreviewClip/PlayClip with known signatures.");

        var args = m.GetParameters().Length == 1
            ? new object[] { clip }
            : new object[] { clip, startSample, loop };

        m.Invoke(null, args);
    }

    public static void StopAllPreviewClips()
    {
        var t = AudioUtilType;
        var m = t?.GetMethod("StopAllPreviewClips", Flags);
        m?.Invoke(null, null);
    }

    public static void SetClipSamplePosition(AudioClip clip, int sample)
    {
        var t = AudioUtilType;
        var m = t?.GetMethod("SetClipSamplePosition", Flags, null, new[] { typeof(AudioClip), typeof(int) }, null);
        m?.Invoke(null, new object[] { clip, sample });
    }
}
