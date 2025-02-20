using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This is the base class. It's intended to be inhereted, but for now is used for all structures
public class Structure : MonoBehaviour
{
    public Vector2 gridScale = Vector2.one;
    public GameObject selectionOutline;
    [Space]
    public int maxHealth = 100;
    public int health = 100;
    public int cost = 100;
    public int refund = 75;
    [Space]
    public UnityEvent onBuild;
    public UnityEvent onSell;
    public UnityEvent onSelect;
    public UnityEvent onDeselect;

    private bool mouseOver = false;

    public virtual void Select()
    {
        StructureManager.Instance.selectedStructure = this;
        if (selectionOutline != null )
        {
            selectionOutline.SetActive(true);
        }
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
    }

    public virtual void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UIManager.Instance.mouseOverUI)
        {
            if (mouseOver)
            {
                Select();
            }
            else
            {
                if (StructureManager.Instance.selectedStructure == this)
                    DeSelect();
            }
        }
    }

    public virtual void OnMouseEnter()
    {
        mouseOver = true;
    }

    public virtual void OnMouseExit()
    {
        mouseOver = false;
    }

}
