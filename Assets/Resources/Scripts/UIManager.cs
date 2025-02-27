using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Transform structureParent;
    public Transform unitParent;

    public GameObject buttonPrefab;

    public bool mouseOverUI = false;

    public AudioSource voice;
    public AudioClip constructionStart;
    public AudioClip constructionEnd;
    public AudioClip trainingStart;
    public AudioClip trainingEnd;
    public AudioClip lowMoney;
    public AudioClip lowPower;
    public AudioClip canceled;
    public AudioClip unitAttacked;
    public AudioClip unitLost;
    public AudioClip buildingAttacked;
    public AudioClip buildingLost;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        mouseOverUI = EventSystem.current.IsPointerOverGameObject();
    }

    [System.Serializable]
    public class StructureItem
    {
        public string name;
        public Sprite icon;
        public Structure structure;
        public UnityEvent callback;
    }

    [System.Serializable]
    public class UnitItem
    {
        public string name;
        public Sprite icon;
        public Unit unit;
        public UnityEvent callback;
    }

    public void DisableStructures(Button button)
    {
        foreach (Transform child in structureParent)
        {
            if (child.GetComponent<Button>() != button)
            {
                child.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void EnableStructures()
    {
        foreach (Transform child in structureParent)
        {
            child.GetComponent<Button>().interactable = true;
        }
    }

    public void PopulateStructures(List<StructureItem> structures)
    {
        ClearStructures();
        foreach (StructureItem item in structures)
        {
            GameObject button = Instantiate(buttonPrefab, structureParent);
            button.GetComponent<UIConstructItem>().image.sprite = item.icon;
            button.GetComponent<UIConstructItem>().callback = item.callback;
            button.GetComponent<UIConstructItem>().buildTime = item.structure.cost/20;
            button.GetComponent<UIConstructItem>().isStructure = true;
            button.GetComponent<UIConstructItem>().cost = item.structure.cost;
        }
    }

    public void ClearStructures()
    {
        foreach (Transform child in structureParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void PopulateUnits(List<UnitItem> units)
    {
        ClearUnits();
        foreach (UnitItem item in units)
        {
            GameObject button = Instantiate(buttonPrefab, unitParent);
            button.GetComponent<UIConstructItem>().image.sprite = item.icon;
            button.GetComponent<UIConstructItem>().callback = item.callback;
            button.GetComponent<UIConstructItem>().isStructure = false;
            button.GetComponent<UIConstructItem>().buildTime = item.unit.cost/20;
            button.GetComponent<UIConstructItem>().cost = item.unit.cost;
        }
    }

    public void ClearUnits()
    {
        foreach (Transform child in unitParent)
        {
            if (child.gameObject)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
