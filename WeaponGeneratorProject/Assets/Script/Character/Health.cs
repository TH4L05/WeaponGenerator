using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    #region Fields

    [SerializeField] private UiBar healthbar;
    [SerializeField] private float maxHealth;
    private float currentHealth;
    private bool noHealthLeft;

    public Action OnNoHealthLeft;

    #endregion

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    #region UpdateHealth

    public void ReduceHealth(float value)
    {
        if (noHealthLeft) return;

        currentHealth -= value;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }

        if (currentHealth == 0 && !noHealthLeft)
        {
            noHealthLeft = true;
            OnNoHealthLeft?.Invoke();
        }

       UpdateUI();
    }

    public void GainHealth(float value)
    {
        if (noHealthLeft) return;

        currentHealth += value;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (healthbar == null)
        {
            Debug.LogError("Healhbar reference is Missing !!!");
            return;
        }
        healthbar.UpdateBar(currentHealth, maxHealth);
    }

    #endregion;
}
