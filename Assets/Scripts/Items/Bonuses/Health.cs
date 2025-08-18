using UnityEngine;

namespace Items.Bonuses
{
    public class Health : Bonus
    {
        public override void StopBoost()
        {
            PlayerController.LocalPlayer.ChangeJump(5f/6f); // возвращаем к исходному(*5/6)
        }

        protected override bool Apply(GameObject interactor)
        {
            PlayerController.LocalPlayer.ChangeJump(1.2f); // (* 6/5)
            return true;
        }
    }
}
