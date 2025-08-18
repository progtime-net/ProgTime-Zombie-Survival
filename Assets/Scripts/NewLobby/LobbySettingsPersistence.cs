using UnityEngine;

public static class LobbySettingsPersistence
{
    private const string Key = "LobbySettings_v1";

    public static void Save(GameSettingsSnapshot s)
    {
        var box = new SaveBox(s);
        PlayerPrefs.SetString(Key, JsonUtility.ToJson(box));
        PlayerPrefs.Save();
    }

    public static bool TryLoad(out GameSettingsSnapshot s)
    {
        s = default;
        if (!PlayerPrefs.HasKey(Key)) return false;
        var json = PlayerPrefs.GetString(Key);
        var box = JsonUtility.FromJson<SaveBox>(json);
        s = box.ToSnapshot();
        return true;
    }

    [System.Serializable]
    private class SaveBox
    {
        public int mapIndex, impostorCountIndex, moveSpeedIndex, killCooldownIndex, meetingsPerPlayerIndex;
        public bool revealImpostors, anonymousVotes, taskbarUpdates;

        public SaveBox(GameSettingsSnapshot s)
        {
            mapIndex = s.mapIndex;
            impostorCountIndex = s.impostorCountIndex;
            moveSpeedIndex = s.moveSpeedIndex;
            killCooldownIndex = s.killCooldownIndex;
            meetingsPerPlayerIndex = s.meetingsPerPlayerIndex;
            revealImpostors = s.revealImpostors;
            anonymousVotes = s.anonymousVotes;
            taskbarUpdates = s.taskbarUpdates;
        }

        public GameSettingsSnapshot ToSnapshot() => new GameSettingsSnapshot
        {
            mapIndex = mapIndex,
            impostorCountIndex = impostorCountIndex,
            moveSpeedIndex = moveSpeedIndex,
            killCooldownIndex = killCooldownIndex,
            meetingsPerPlayerIndex = meetingsPerPlayerIndex,
            revealImpostors = revealImpostors,
            anonymousVotes = anonymousVotes,
            taskbarUpdates = taskbarUpdates
        };
    }
}
