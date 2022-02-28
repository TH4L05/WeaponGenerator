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
    [SerializeField] private ItemType itemType;
    public ItemType ItemType => itemType;
    public bool isDamagable;
    public bool isMovable;
    public bool isMagnetic;
    public bool isInteractable;
}
