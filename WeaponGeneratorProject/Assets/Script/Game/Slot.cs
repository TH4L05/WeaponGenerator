using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI type;
    [SerializeField] private TextMeshProUGUI speed;
    [SerializeField] private TextMeshProUGUI accuracy;
    [SerializeField] private TextMeshProUGUI ammoCapacity;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Sprite weaponImageDefault;
    [SerializeField] private Image slotImage;
    [SerializeField] private Color slotColorDefault;
    [SerializeField] private Button equipButton;

    #endregion Fields

    #region UnityFunctions

    private void Awake()
    {
        ResetSlot();
    }
    #endregion UnityFunctions

    public void UpdateSlot(WeaponData data, Sprite icon, Sprite frame)
    {
        slotImage.color = data.rarityColor;
        weaponImage.sprite = icon;

        weaponName.text = data.name;
        level.text = "Level: " + data.level.ToString();
        type.text = data.rarityValue.ToString() + " " + data.type.ToString();
        speed.text = data.rateOfFire.ToString("0") +"/min";
        damage.text = data.damageMin.ToString("0") + " - " + data.damageMax.ToString("0");
        accuracy.text = data.accuracy.ToString("0");
        ammoCapacity.text = data.ammoCapacity.ToString("0");

        var playerlevel = Game.instance.player.GetComponentInChildren<Player>().Level;

        if (playerlevel >= data.level)
        {
            equipButton.interactable = true;
        }

    }

    public void ResetSlot()
    {
        weaponName.text = "Name";
        level.text = "";
        type.text = "";
        speed.text = "";
        damage.text = "";
        accuracy.text = "";
        weaponImage.sprite = weaponImageDefault;
        slotImage.color = slotColorDefault;
        equipButton.interactable = false;
    }
}
