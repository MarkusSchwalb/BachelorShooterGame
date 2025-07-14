using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.PackageManager;
using UnityEngine;

public class ShoItem : Interactable
{
    public PossibleShopItem[] Items;
    int slot = 0;
    public Transform ItemDisplay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Items == null || Items.Length == 0)
        {
            Debug.LogWarning("GunPickUpHasNoGuns");
            return;
        }
        ChooseItem();
        Instantiate(Items[slot].ItemObject, ItemDisplay);
        updateText();
    }

    private void updateText()
    {
        int currentXP = XPManager.Instance.currentXP;
        if (currentXP < Items[slot].price)
        {
            interactableText = "Not Enough XP";
            return;
        }
            switch (Items[slot].ItemType)
        {
            case ShopItemType.Weapon:
                if (Items[slot].ItemObject.TryGetComponent<Gun>(out Gun gun))
                {
                    GunData gunInfo = gun.GunInfo;
                    interactableText = "Press E to buy " + gunInfo.GunName + " Type " +
                    gunInfo.GunType + " FireRate " + gunInfo.FiringRate + " Damage " + 
                    gunInfo.normalDamage + " for " + Items[slot].price + " xp";
                }
                break;
            case ShopItemType.Grenade:
                interactableText = "Press E to buy Grenade" + " for " + Items[slot].price + " xp";
                break;
            case ShopItemType.HealthPack:
                interactableText = "Press E to buy Health Pack" + " for " + Items[slot].price + " xp";
                break;
            case ShopItemType.Ammo:
                interactableText = "Press E to buy Ammo" + " for " + Items[slot].price + " xp";
                break;
            case ShopItemType.Upgrade:
                interactableText = "Press E to buy Upgrade" + " for " + Items[slot].price + " xp";
                break;
        }
    }

    private void ChooseItem()
    {
        if (Items == null || Items.Length == 0) return;
        int value = 0;
        slot = 0;
        foreach (PossibleShopItem item in Items)
        {
            if (item == null) continue;
            value += item.Commones;
        }

        int randomValue = UnityEngine.Random.Range(0, value);
        value = 0;
        foreach (PossibleShopItem item in Items)
        {
            if (item == null)
            {
                slot++;
                continue;
            }
            value += item.Commones;

            if (value > randomValue)
            {
                return;
            }
            slot++;
        }
    }

    public override void Interact()
    {
        int currentXP = XPManager.Instance.currentXP;
        if (currentXP > Items[slot].price)
        {
            XPManager.Instance.ReduceXP(Items[slot].price);
            DoItemThing();
        }
    }

    private void DoItemThing()
    {
        Player player = FindFirstObjectByType<Player>();
        if ( player == null)
        {
            return;
        }
        switch (Items[slot].ItemType)
        {
            case ShopItemType.Weapon:
                if (Items[slot].ItemObject.TryGetComponent<Gun>(out Gun gun))
                {
                    GunData gunInfo = gun.GunInfo;
                    bool isMain = gun.isMainWeapon;
                    player.GetNewWeapon(isMain, Items[slot].ItemObject);
                }
                break;
            case ShopItemType.Grenade:
                player.ThrowableCount++;
                break;
            case ShopItemType.HealthPack:
                player.HealthComponent.Regenerate(50);
                break;
            case ShopItemType.Ammo:
                Gun mainGun = player.MainGunHand.Gun;
                Gun secondGun = player.SecondaryGunHand.Gun;
                //.Magazin?.PickUpAmmo(AmmoAmmount);
                if (mainGun != null) mainGun.Magazin.PickUpAmmo(50);
                if (secondGun != null) secondGun.Magazin.PickUpAmmo(50);
                Destroy(gameObject);
                break;
            case ShopItemType.Upgrade:
                //maybe sometime
                break;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
