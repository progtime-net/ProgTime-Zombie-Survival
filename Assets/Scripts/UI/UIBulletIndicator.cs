using UnityEngine;

public class UIBulletIndicator : MonoBehaviour
{
    public int CurrentBullets = 0;
    public int TargetBullets;
    public int TotalBullets;
    public float ChangeBulletSmoothness = 0.2f;

    public void SetTotalBullets(int totalBullets)
    {
        TotalBullets = totalBullets;
    }
    /// <summary>
    /// force set - when changing weapons
    /// </summary>
    public void SetCurrentBullets(int currentBullets)
    {
        CurrentBullets = currentBullets;
        TargetBullets = currentBullets;
    }

    public void UpdateBulletsLeft(int bulletsLeft)
    {
        TargetBullets = bulletsLeft;
    }

     
    void Start()
    {
        if (TargetBullets == CurrentBullets)
            return;
        
        CurrentBullets = (int)Mathf.Lerp(CurrentBullets, TargetBullets, ChangeBulletSmoothness);

    }
     
}
