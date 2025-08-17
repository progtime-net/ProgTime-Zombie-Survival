using UnityEngine;

public class DamageBoostController : Bonus
{
    public override void StopBoost()
    {
        PlayerController.LocalPlayer.ChangeDamage(5/6f); // возвращаем к исходному(*5/6)
    }

    protected override bool Apply(GameObject interactor)
    {
        PlayerController.LocalPlayer.ChangeDamage(1.2f); // (* 6/5)
        return true;
    }
}