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

    public void PopulateStructures(List<StructureItem> structures)
    {
        ClearStructures();
        foreach (StructureItem item in structures)
        {
            GameObject button = Instantiate(buttonPrefab, structureParent);
            button.GetComponent<UIConstructItem>().image.sprite = item.icon;
            button.GetComponent<UIConstructItem>().callback = item.callback;
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
        }
    }

    public void ClearUnits()
    {
        foreach (Transform child in unitParent)
        {
            Destroy(child.gameObject);
        }
    }
}
