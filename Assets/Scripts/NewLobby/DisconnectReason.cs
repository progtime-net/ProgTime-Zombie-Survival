using Mirror;

public struct KickMessage : NetworkMessage
{
    public string reason;
}

public static class DisconnectReason
{
    public static string Text { get; private set; } = string.Empty;
    public static bool Voluntary { get; private set; } = false;

    public static void Set(string reason)
    {
        Text = reason ?? string.Empty;
        Voluntary = false;
    }

    public static void MarkVoluntary()
    {
        Text = string.Empty;
        Voluntary = true;
    }

    public static void Clear()
    {
        Text = string.Empty;
        Voluntary = false;
    }
}
