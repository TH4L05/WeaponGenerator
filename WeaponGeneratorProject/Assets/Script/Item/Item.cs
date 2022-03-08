using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Interactable))]
public class Item : MonoBehaviour , IDamageable
{
    [SerializeField] private ItemData data;
    public ItemData Data => data;
    private Interactable interactable;
    private Rigidbody rb;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        rb = GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        interactable.Interact();
    }

    public void TakeDamage(float damage)
    {
        if (!data.IsDamagable) return;
        Debug.Log($"{transform.name} get hit and takes damage of {damage}");
    }
}
