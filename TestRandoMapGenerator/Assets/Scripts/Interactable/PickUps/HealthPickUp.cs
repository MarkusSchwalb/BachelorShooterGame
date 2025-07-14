using UnityEngine;

public class HealthPickUp : Interactable
{
    float RegValue = 30;
    public override void Interact()
    {
        Player player = FindFirstObjectByType<Player>();

        player.HealthComponent.Regenerate(RegValue);
        Destroy(gameObject);
    }
}
