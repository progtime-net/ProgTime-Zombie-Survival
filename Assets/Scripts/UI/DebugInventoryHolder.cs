using UnityEngine;

public class DebugInventoryHolder : MonoBehaviour
{
    [SerializeField] public Inventory inventory;
    void Start()
    {
        inventory.AddItem(new AK());
        inventory.AddItem(new Sniper());
        //inventory.AddItem(new TT());

    }

    // Update is called once per frame
    void Update()
    {

    }
}
