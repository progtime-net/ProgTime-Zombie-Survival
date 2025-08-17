using UnityEngine;

public class FirstAidKitController : Bonus
{
    [SerializeField] private int healAmount = 25;

    protected override string GetBonusHint() => $"+{healAmount} HP";

    protected override bool Apply(GameObject interactor)
    {
        print("test Apply");
        var isApply = PlayerController.LocalPlayer.ChangeHealth(1f);
        return isApply;
    }

    public override void StopBoost() { }
}
