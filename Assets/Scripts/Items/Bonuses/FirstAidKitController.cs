using UnityEngine;

public class FirstAidKitController : Bonus
{
    [SerializeField] private int healAmount = 25;

    [Header("Spin")]
    [SerializeField] private float spinSpeed = 45f;
    [SerializeField] private Vector3 spinAxis = Vector3.up;
    [SerializeField] private bool spinInWorldSpace = true;

    protected override string GetBonusHint() => $"+{healAmount} HP";

    // private Vector3 startPosition;
    // void Start()
    // {
    //     // startPosition = transform.position;
    // }

    void Update()
    {
        UpdateYPosBySin();
        UpdateRotation();
    }

    protected override bool Apply(GameObject interactor)
    {
        var health = interactor.GetComponent<Health>();
        if (!health) return false;

        // Option: prevent pickup if already full
        if (health.IsFull) return false;

        health.Heal(healAmount);
        return true;
    }
}
