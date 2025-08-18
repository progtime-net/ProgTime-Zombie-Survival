using UnityEngine;

namespace Items.Bonuses
{
    public class FirstAidKitController : Bonus
    {
        [SerializeField] private float healAmount = 1;

        protected override string GetBonusHint() => $"+{healAmount} HP";

        protected override bool Apply(GameObject interactor)
        {
            print("test Apply");
            var isApply = PlayerController.LocalPlayer.ChangeHealth(healAmount);
            return isApply;
        }

        public override void StopBoost() { }
    }
}
