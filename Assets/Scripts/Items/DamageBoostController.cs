using UnityEngine;

public class DamageBoostController : Bonus
{
    protected override bool Apply(GameObject interactor)
    {
        PlayerController.LocalPlayer.ChangeDamage(1.2f);
        return true;
    }
}