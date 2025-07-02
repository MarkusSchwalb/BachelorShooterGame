using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [field: SerializeField] public string interactableText {  get; protected set; }

    public abstract void Interact();
}
