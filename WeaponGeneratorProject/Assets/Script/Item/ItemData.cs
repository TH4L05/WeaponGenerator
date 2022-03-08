using UnityEngine;

public enum ItemType
{
    Default,
    Button,
    Money,
    Weapon,
}

[CreateAssetMenu(fileName = "New ItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    #region SerializedFields

    [SerializeField] private ItemType itemType;
    [SerializeField] private bool isDamagable;
    [SerializeField] private bool isMovable;
    [SerializeField] private bool isMagnetic;
    [SerializeField] private bool isInteractable;
    [SerializeField] private string infoText;

    #endregion

    #region Fields

    public ItemType ItemType => itemType;
    public bool IsDamagable => isDamagable;
    public bool IsMovable => isMovable;
    public bool IsMagnetic => isMagnetic;
    public bool IsInteractable => isInteractable;
    public string InfoText => infoText;

    #endregion
}
