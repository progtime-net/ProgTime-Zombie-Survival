using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float interactDistance = 1000f;
    [SerializeField] private LayerMask interactMask = ~0; // or Interactable layer
    [SerializeField] private KeyCode useKey = KeyCode.E;
    [SerializeField] private TextMeshProUGUI promptText; // optional

    private IInteractable focused;

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
        if (Physics.Raycast(ray, out var hit, interactDistance, interactMask, QueryTriggerInteraction.Collide))
        {
            Debug.Log("Interacted Player with first aid kit successfull!");

            var interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != focused)
            {
                focused?.OnFocusExit(gameObject);
                focused = interactable;
                focused?.OnFocusEnter(gameObject);
            }

            if (promptText)
                promptText.text = (focused != null && focused.CanInteract(gameObject))
                    ? $"{focused.Prompt} — Press [{useKey}]"
                    : string.Empty;

            if (focused != null && focused.CanInteract(gameObject) && Input.GetKeyDown(useKey))
            {
                focused.Interact(gameObject);
            }
        }
        else
        {
            if (focused != null) { focused.OnFocusExit(gameObject); focused = null; }
            if (promptText) promptText.text = string.Empty;
        }
    }
}
