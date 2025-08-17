using System.Collections;
using Mirror;
using UnityEngine;

<<<<<<<< HEAD:Assets/Scripts/Items/Bonus.cs
public abstract class Bonus : NetworkBehaviour, IInteractable
========
public abstract class Bonus : Item, IInteractable
>>>>>>>> development:Assets/Scripts/Items/Bonuses/Bonus.cs
{
    [Header("Display")]
    [SerializeField] private string displayName = "Bonus";

    [Header("Spin")]
    [SerializeField] private float spinSpeed = 130f;
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

    [SerializeField] private float timeBoost = 7f; // время действия буста в секундах

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

        print("test3");
        // var playerController = interactor.GetComponent<PlayerController>();
        if (Apply(interactor) && interactor.CompareTag("Player"))
        {
            print("test4");
            _consumed = true;
            // if (pickupVfx) Instantiate(pickupVfx, transform.position, Quaternion.identity);
            // if (pickupSfx) AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
            Destroy(gameObject); // or pool/disable if preferred
        }
    }

    [Server]
    protected void OnTriggerEnter(Collider other)
    {
        print("test");
        // if (!isServer) return;
        GameObject obj = other.gameObject;
        print(obj);
        if (obj.CompareTag("Player"))
        {
            print("test2");

            Interact(obj);
        }
    }
    protected virtual bool Apply(GameObject interactor)
    {
        StartCoroutine(StopBoostCoroutine());
        return false;
    }

    private IEnumerator StopBoostCoroutine()
    {
        yield return new WaitForSeconds(timeBoost);
        StopBoost();
    }
    public abstract void StopBoost();

    void Update()
    {
        UpdateRotation();
        UpdateYPosBySin();
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
