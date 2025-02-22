using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public int teamID = 0;

    public List<Team> teams;

    [Space]
    public TMP_Text moneyLabel;
    public Color fullPower;
    public Color lowPower;
    public Color noPower;
    public Slider powerSlider;
    public Image powerBar;

    public int money = 9999;

    public int maxPower { get { return GetMaxPower(); } }
    public int availablePower { get { return GetAvailablePower(); } }

    [System.Serializable]
    public class Team
    {
        public int teamID;
        public Color color;

        public Team()
        {
            color = Color.white;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (powerSlider)
        {
            powerSlider.maxValue = maxPower;
            powerSlider.value = availablePower >=0? availablePower: 0;
            if (availablePower >= maxPower * 0.5f)
            {
                powerBar.color = fullPower;
            }
            else if (availablePower < maxPower * 0.5f && availablePower >= maxPower * 0.1f)
            {
                powerBar.color = lowPower;
            }
            else if (availablePower < maxPower * 0.1f)
            {
                powerBar.color = noPower;
            }
        }
        if (moneyLabel)
        {
            moneyLabel.text = money.ToString();
        }
    }

    public void RemoveMoney(int value)
    {
        money -= value;
    }

    public void AddMoney(int value)
    {
        money += value;
    }

    public int GetMaxPower()
    {
        int power = 0;

        foreach (Structure structure in StructureManager.Instance.structureList)
        {
            if (structure && structure.power > 0)
            {
                power += structure.power;
            }
        }

        return power;
    }

    public int GetAvailablePower()
    {
        int power = maxPower;

        foreach (Structure structure in StructureManager.Instance.structureList)
        {
            if (structure && structure.power < 0)
            {
                power += structure.power;
            }
        }

        return power;
    }
}
