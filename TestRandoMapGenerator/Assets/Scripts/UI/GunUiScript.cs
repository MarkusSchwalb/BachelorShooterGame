using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunUiScript : MonoBehaviour
{
    [field: SerializeField] private Player player;
    [field: SerializeField] InventorySlot slot = InventorySlot.Main;

    public Color Selected;
    public Color NotSelected;

    private Image backgImage;

    public TextMeshProUGUI AmmoTextField;

    // Start is called before the first frame update
    void Start()
    {
        backgImage = GetComponent<Image>();

        if (player == null) 
        { 
            GameObject pl = GameObject.FindWithTag("Player");
            player = pl.GetComponent<Player>();

            if (player == null) return;

            player.InputReader.ReloadEvent += UpdateUis;
            player.InputReader.ShootEvent += UpdateUi;
        }
        UpdateUi();
    }

    private void UpdateUis()
    {
        counter = -1;
        UpdateUi(); 
    }

    private void UpdateUi()
    {
        //Debug.Log("UpdateUI");
        if (player == null) {
            Debug.Log("UpdateUI NO PLAYER FOUND");
            return;
        }
        
        switch (slot)
        {
            case InventorySlot.Main:
                UpdateAmmoDisplay(player.MainGun);
                UpdateSlot();
                break;
            case InventorySlot.Secondary:
                UpdateAmmoDisplay(player.SecondaryGun);
                UpdateSlot();
                break;
            default:
                break;
        }
    }

    private void UpdateSlot()
    {
        //Debug.Log("UpdateSlot");
        if (backgImage == null) return;

        if (player.currentSlot == 0 && slot == InventorySlot.Main)
        {
            backgImage.color = Selected;
            return;
        } 
        if (player.currentSlot == 1 && slot == InventorySlot.Secondary)
        {
            backgImage.color = Selected;
            return;
        }
        if (player.currentSlot == 2 && slot == InventorySlot.Throwable)
        {
            backgImage.color = Selected;
            return;
        }
        backgImage.color = NotSelected;
    }

    private void UpdateAmmoDisplay(GameObject gun)
    {
        //Debug.Log("UpdateAmmoDisplay");
        if (gun == null) { Debug.Log("UpdateAmmoDisplayNoGunFound"); return; }
        if (gun.TryGetComponent<MagazinComp>(out MagazinComp mag))
        {
            if (mag == null) return;
            string display = mag.CurrentBulletsInMag + "/" + mag.CurrentAmmo;
            AmmoTextField.text = display;
        } else
        {
            Debug.Log("noMagazinCompFound");
        }

    }

    float counter = 0;
    // Update is called once per frame
    void Update()
    {
        if (counter < 0.2f) { UpdateUi(); counter += Time.deltaTime; }
        if (player.InputReader.TriggerDown)
        {
            
        }

        UpdateUi();
    }


}
public enum InventorySlot
{
    Main, Secondary, Throwable
}