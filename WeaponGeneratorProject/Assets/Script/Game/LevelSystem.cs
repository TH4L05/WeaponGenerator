using System;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    #region Fields

    [SerializeField] private UiBar uiBar;
    [SerializeField] private int maxLevel = 60;
    [SerializeField] [Range(0.1f, 5.0f)] private float nextLevelFactor = 1.2f;
    private int level = 0;
    private int currentExp = 0;
    private int expToNextLevel = 100;

    public int Level => level;
    public int Experience => currentExp;
    public int ExpToNextLevel => expToNextLevel;

    public Action LevelUP;

    #endregion

    private void Start()
    {
        UpdateUI();
    }
    
    public void AddExperience(int amount)
    {
        currentExp += amount;
        CheckLevelUp();
        UpdateUI();
    }

    public void CheckLevelUp()
    {
        if (level == maxLevel) return;
        if (currentExp >= expToNextLevel)
        {
            level++;
            expToNextLevel += (int)(expToNextLevel * nextLevelFactor);
            Debug.Log($"Character leveled up to {level}");

            
        }
    }

    public void UpdateUI()
    {
        uiBar.UpdateBar(currentExp, expToNextLevel);       
    }


    public void SetMaxLevel(int maxLevel)
    {
        this.maxLevel = maxLevel;
    }

    public void SetNextLevelFactor(float factor)
    {
        nextLevelFactor = factor;
    }
}
