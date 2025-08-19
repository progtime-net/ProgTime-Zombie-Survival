using System.Linq;
using UnityEngine;

public class SpeedBoostController : Bonus
{
    public override void StopBoost()
    {
        PlayerController.LocalPlayer.ChangeSpeed(5f/6f); // возвращаем к исходному(* 5/6)
    }

    protected override bool Apply(GameObject interactor)
    {
        PlayerController.LocalPlayer.ChangeSpeed(1.2f); // (* 6/5)
        return true;
    }
}
