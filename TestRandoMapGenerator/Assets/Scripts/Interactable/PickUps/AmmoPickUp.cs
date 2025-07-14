using UnityEngine;

public class AmmoPickUp : Interactable
{
    int AmmoAmmount = 50;
    public override void Interact()
    {
        Player player = FindFirstObjectByType<Player>();

        Gun mainGun = player.MainGunHand.Gun;
        Gun secondGun = player.SecondaryGunHand.Gun;
        //.Magazin?.PickUpAmmo(AmmoAmmount);
        if (mainGun != null) mainGun.Magazin.PickUpAmmo(AmmoAmmount);
        if (secondGun != null) secondGun.Magazin.PickUpAmmo(AmmoAmmount);
        Destroy(gameObject);
    }
}
