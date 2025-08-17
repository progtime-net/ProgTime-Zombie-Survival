using UnityEngine;

public class PlayerProfileManager : MonoBehaviour
{
    public static PlayerProfileManager Instance { get; private set; }
    public PlayerProfile profile = new PlayerProfile();

    private const string SaveKey = "player_profile_json";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadProfile();
    }

    public void SaveProfile()
    {
        string json = JsonUtility.ToJson(profile);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();

        Debug.Log("[PROFILE] Player profile saved");
    }

    public void LoadProfile()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            profile = JsonUtility.FromJson<PlayerProfile>(json);

            Debug.Log("[PROFILE] Player profile loaded");
        }
        else
        {
            profile = new PlayerProfile();
            profile.nickname = "PLAYER_" + Random.Range(1000, 10000).ToString();

            Debug.Log("[PROFILE] Player profile created");
            SaveProfile();
        }
    }

    public void ChangeNickname(string nickname) => profile.nickname = nickname;
}
