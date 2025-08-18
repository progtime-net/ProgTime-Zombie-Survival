using UnityEngine;

public class SpeedBoostController : Bonus
{
    //PlayerMovement movement = new PlayerMovement();
    protected override bool Apply(GameObject interactor)
    {
        //movement.ChangeSpeed(1.2f);
        return true;
    }
}
