using System.Linq;
using UnityEngine;

public class SpeedBoostController : Bonus
{
    protected override bool Apply(GameObject interactor)
    {
        PlayerController.LocalPlayer.ChangeSpeed(1.2f);
        return true;
    }
}
