using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;
    // [SerializeField] private float amount = 10f;

    public int Current => currentHealth;
    public int Max => maxHealth;
    public bool IsFull => currentHealth >= maxHealth;

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + Mathf.Abs(amount), maxHealth);
        // TODO: trigger UI update
    }

    public void Damage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - Mathf.Abs(amount), 0);
        // TODO: death handling
    }
}
