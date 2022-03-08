using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI ammoCapacityText;
    [SerializeField] private Image weaponImage;
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Slot weaponInfo;
    [SerializeField] private GameObject crosshair;

    #endregion

    #region UnityFunctions

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

    #endregion

    #region Info

    private void ShowInfoText(bool active, string infoText)
    {
        this.infoText.gameObject.SetActive(active);
        this.infoText.text = infoText;             
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
        if (obj == null) return;
        weaponInfo.gameObject.SetActive(active);      
        var weapon = obj.GetComponentInChildren<Weapon>();
        weaponInfo.UpdateSlot(weapon.Data, weapon.Icon);
    }

    #endregion

    #region Crosshair

    private void DisableCrosshair()
    {
        if(crosshair != null) crosshair.SetActive(false);
    }

    private void EnableCrosshair()
    {
        if (crosshair != null) crosshair.SetActive(true);
    }

    #endregion
}
