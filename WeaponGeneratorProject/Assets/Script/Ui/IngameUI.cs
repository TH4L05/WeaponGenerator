using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI ammoCapacityText;
    [SerializeField] private Image weaponImage;
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Slot weaponInfo;
    [SerializeField] private GameObject crosshair;
    public string[] infoTexts;

    private void Awake()
    {
        GameEvents.ShowInfoText += ShowInfoText;
        GameEvents.ShowWeaponInfo += ShowWeaponInfo;

        Game.instance.player.WeaponController.AmmoValueChanged += UpdateAmmo;
        Game.instance.player.WeaponController.ActiveWeaponChanged += UpdateActiveWeapon;      
        Game.instance.player.LevelSystem.LevelUP += UpdateLevelText;
        Weapon.DisableCrosshair += DisableCrosshair;
        Weapon.EnableCrosshair += EnableCrosshair;
    }

    private void OnDestroy()
    {
        GameEvents.ShowInfoText -= ShowInfoText;
        GameEvents.ShowWeaponInfo -= ShowWeaponInfo;

        Game.instance.player.WeaponController.AmmoValueChanged -= UpdateAmmo;
        Game.instance.player.WeaponController.ActiveWeaponChanged -= UpdateActiveWeapon;
        Game.instance.player.LevelSystem.LevelUP -= UpdateLevelText;
        Weapon.DisableCrosshair -= DisableCrosshair;
        Weapon.EnableCrosshair -= EnableCrosshair;
    }

    private void ShowInfoText(bool status, int textid)
    {
        infoText.gameObject.SetActive(status);
        if (textid > infoTexts.Length)
        {
            infoText.text = "Wrong Infotext ID";
        }
        else
        {
            infoText.text = infoTexts[textid];
        }                
    }

    private void UpdateLevelText()
    {
        levelText.text = Game.instance.player.Level.ToString("00");
    }

    private void UpdateAmmo(int ammo)
    {
        ammoText.text = ammo.ToString("00");
    }

    private void UpdateActiveWeapon(WeaponData data, Sprite icon)
    {
        weaponNameText.text = data.name;
        ammoCapacityText.text = data.ammoCapacity.ToString("00");
        ammoText.text = data.ammoAmount.ToString("00");
        weaponImage.sprite = icon;
    }

    private void ShowWeaponInfo(bool active, GameObject obj)
    {       
        weaponInfo.gameObject.SetActive(active);
        if (obj == null) return;
        var weapon = obj.GetComponentInChildren<Weapon>();
        weaponInfo.UpdateSlot(weapon.Data, weapon.Icon, weapon.Frame);
    }

    private void DisableCrosshair()
    {
        if(crosshair != null) crosshair.SetActive(false);
    }

    private void EnableCrosshair()
    {
        if (crosshair != null) crosshair.SetActive(true);
    }

}
