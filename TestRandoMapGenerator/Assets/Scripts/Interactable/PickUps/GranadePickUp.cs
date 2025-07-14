using UnityEngine;

public class GranadePickUp : Interactable
{
    public override void Interact()
    {
        Player player = FindFirstObjectByType<Player>();
        player.ThrowableCount++;
        Destroy(gameObject);
    }
}
