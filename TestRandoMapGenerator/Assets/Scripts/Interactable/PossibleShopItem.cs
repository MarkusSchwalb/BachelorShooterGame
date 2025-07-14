using UnityEngine;
[System.Serializable]
public class PossibleShopItem
{
    public ShopItemType ItemType;
    public GameObject ItemObject;
    public int Commones;
    public int price = 1000; //
    
}
public enum ShopItemType
{
    Weapon, Grenade, HealthPack, Ammo, Upgrade
}
