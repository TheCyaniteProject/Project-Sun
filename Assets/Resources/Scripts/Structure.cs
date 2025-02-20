using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public Vector2 gridScale = Vector2.one;
    public GameObject selectionOutline;
    [Space]
    public int maxHealth = 100;
    public int health = 100;

    private bool mouseOver = false;

    public void Select()
    {
        StructureManager.Instance.selectedStructure = this;
        if (selectionOutline != null )
        {
            selectionOutline.SetActive(true);
        }
    }
    public void DeSelect()
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mouseOver)
            {
                Select();
            }
            else
            {
                DeSelect();
            }
        }
    }

    public void OnMouseEnter()
    {
        mouseOver = true;
    }
    public void OnMouseExit()
    {
        mouseOver = false;
    }

}
