using UnityEngine;

public class JumpBoostController : Bonus
{
    protected override bool Apply(GameObject interactor)
    {
        PlayerController.LocalPlayer.ChangeJump(1.2f);
        return true;
    }
}
