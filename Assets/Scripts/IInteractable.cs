using UnityEngine;

interface IInteractable
{
    string Prompt { get; }                         // e.g., "First Aid Kit +25 HP"
    bool CanInteract(GameObject interactor);       // gate logic (e.g., health full?)
    void Interact(GameObject interactor);          // do the thing
    void OnFocusEnter(GameObject interactor);      // for highlight/backlight
    void OnFocusExit(GameObject interactor);
}
