using System;
using UnityEngine;

public class CartridgesController : Bonus
{
    public override void StopBoost() { }

    protected override bool Apply(GameObject interactor)
    {
        return false; // DELETE
        /// TODO: Do this when Gun will complete
    }
}
