using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// This is the base class. It's intended to be inhereted, but for now is used for all structures
public class Structure : MonoBehaviour
{
    public Vector2 gridScale = Vector2.one;
    public GameObject selectionOutline;
    [Space]
    public int teamID = 0;
    public int power = 10;
    public int maxHealth = 100;
    public int health = 100;
    public int cost = 100;
    public int buildTime = 10;
    [Space]
    public UnityEvent onBuild;
    public UnityEvent onSell;
    public UnityEvent onSelect;
    public UnityEvent onDeselect;

    public bool mouseOver = false;
    public bool isSelected = false;

    private void Start()
    {
        StructureManager.Instance.structureList.Add(this);
    }

    public virtual void Select()
    {
        if (teamID != PlayerData.Instance.teamID)
            return;

        StructureManager.Instance.SelectStructure(this);
        if (selectionOutline != null )
        {
            selectionOutline.SetActive(true);
        }
        isSelected = true;
    }
    public virtual void DeSelect()
    {
        if (StructureManager.Instance.selectedStructure == this)
        {
            StructureManager.Instance.selectedStructure = null;
        }
        if (selectionOutline != null)
        {
            selectionOutline.SetActive(false);
        }
        isSelected = false;
    }

    bool selecting = false;
    public virtual void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UIManager.Instance.mouseOverUI)
        {
            if (mouseOver)
                selecting = true;
        }
        if (selecting && Input.GetMouseButtonUp(0) && !UIManager.Instance.mouseOverUI)
        {
            if (mouseOver)
            {
                Select();
            }
        }
        if (UnitManager.Instance._isDragging)
        {
            if (isSelected)
            {
                DeSelect();
            }
        }
    }

    public void Damage(int amount)
    {
        if (health == maxHealth)
        {
            UIManager.Instance.voice.clip = UIManager.Instance.buildingAttacked;
            UIManager.Instance.voice.Play();
        }
        health -= amount;
        if (health <= 0f)
        {
            // TODO Animation
            Destroy(gameObject);

            if (StructureManager.Instance.structureList.Contains(this))
            {
                StructureManager.Instance.structureList.Remove(this);
            }
        }
    }

    public virtual void OnMouseEnter()
    {
        mouseOver = true;
    }

    public virtual void OnMouseExit()
    {
        selecting = false;
        mouseOver = false;
    }

    private void OnDestroy()
    {
        DeSelect();
        StructureManager.Instance.structureList.Remove(this);

        //UIManager.Instance.voice.clip = UIManager.Instance.buildingLost;
        //UIManager.Instance.voice.Play();
    }

}
