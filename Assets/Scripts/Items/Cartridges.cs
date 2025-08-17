using UnityEngine;

public class Cartridges : MonoBehaviour
{
    [SerializeField] private int maxCartridge = 125;
    [SerializeField] private int currentCartridge = 125;
    [SerializeField] private int countCartridgesInClip = 25;

    public int CurrentCartridge => currentCartridge;
    public int MaxCartridge => maxCartridge;
    public bool IsFull => currentCartridge >= maxCartridge;

    /// TODO: end this class and make inheritance from ICartridge or abstract class Cartridge

    public void Take()
    {
        currentCartridge += Mathf.Min(MaxCartridge, CurrentCartridge + countCartridgesInClip);
    }
}
