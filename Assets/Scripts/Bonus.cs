using UnityEngine;

public abstract class Bonus : MonoBehaviour, IInteractable
{
    [Header("Display")]
    [SerializeField] private string displayName = "Bonus";

    [Header("Spin")]
    [SerializeField] private float spinSpeed = 45f;
    [SerializeField] private Vector3 spinAxis = Vector3.up;
    [SerializeField] private bool spinInWorldSpace = true;

    [Header("Highlight / Backlight")]
    [SerializeField] private GameObject highlightRoot; // assign your Backlight child

    [Header("Feedback")]
    [SerializeField] private AudioClip pickupSfx;
    [SerializeField] private ParticleSystem pickupVfx;

    [Header("Move by Y(real Z)")]
    [SerializeField] private float amplitude = 1f;    // Высота волны
    [SerializeField] private float frequency = 1f;    // Частота колебаний

    private bool _consumed;

    public string Prompt => $"{displayName} {GetBonusHint()}";

    protected Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    protected virtual string GetBonusHint() => string.Empty;

    public virtual bool CanInteract(GameObject interactor) => !_consumed;

    public virtual void Interact(GameObject interactor)
    {
        if (_consumed) return;

        if (Apply(interactor))
        {
            _consumed = true;
            if (pickupVfx) Instantiate(pickupVfx, transform.position, Quaternion.identity);
            if (pickupSfx) AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
            Destroy(gameObject); // or pool/disable if preferred
        }
    }

    protected abstract bool Apply(GameObject interactor);

    void Update()
    {
        // UpdateRotation();
    }

    protected virtual void UpdateRotation()
    {
        if (spinSpeed != 0f)
            transform.Rotate(spinAxis * spinSpeed * Time.deltaTime,
                spinInWorldSpace ? Space.World : Space.Self);
    }

    public virtual void UpdateYPosBySin()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;

        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    public void OnFocusEnter(GameObject interactor) => SetHighlight(true);
    public void OnFocusExit(GameObject interactor) => SetHighlight(false);

    protected virtual void SetHighlight(bool on)
    {
        if (highlightRoot) highlightRoot.SetActive(on); // backlight hook for later
    }
}