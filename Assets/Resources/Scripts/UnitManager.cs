using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    public RectTransform selectionBox;

    public List<Unit> unitList;
    public List<Unit> selectedUnits;

    private void Awake()
    {
        Instance = this;
    }

    bool isMouseDown, isDragging = false;
    public bool _isDragging { get { return isDragging; } }
    Vector3 mouseStartPosition;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!UIManager.Instance.mouseOverUI)
            {
                isMouseDown = true;
                mouseStartPosition = Input.mousePosition;
            }
            if (selectedUnits.Count > 0)
            {
                while (selectedUnits.Count > 0)
                {
                    selectedUnits[0].DeSelect();
                }
            }
            selectedUnits.Clear();
        }
        if (isMouseDown)
        {
            if (Vector3.Distance(mouseStartPosition, Input.mousePosition) > 1 && !isDragging)
            {
                isDragging = true;
                selectionBox.gameObject.SetActive(true);
            }
            if (isDragging)
            {
                float selectionWidth = Input.mousePosition.x - mouseStartPosition.x;
                float selectionHeight = Input.mousePosition.y - mouseStartPosition.y;
                selectionBox.sizeDelta = new Vector2 (Mathf.Abs(selectionWidth), Mathf.Abs(selectionHeight));
                selectionBox.transform.position = (mouseStartPosition + Input.mousePosition)/2;

                SelectUnits();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            isDragging = false;
            selectionBox.gameObject.SetActive(false);
        }
    }

    void SelectUnits()
    {
        foreach (Unit unit in unitList)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            float left = selectionBox.transform.position.x - (selectionBox.sizeDelta.x / 2);
            float right = selectionBox.transform.position.x + (selectionBox.sizeDelta.x / 2);
            float top = selectionBox.transform.position.y + (selectionBox.sizeDelta.y / 2);
            float bottom = selectionBox.transform.position.y - (selectionBox.sizeDelta.y / 2);

            if (screenPos.x > left && screenPos.x < right && screenPos.y > bottom && screenPos.y < top)
            {
                if (!selectedUnits.Contains(unit))
                {
                    selectedUnits.Add(unit);
                    unit.Select();
                }
            }
            else
            {
                if (selectedUnits.Contains(unit))
                {
                    selectedUnits.Remove(unit);
                    unit.DeSelect();
                }
            }
        }
    }
}
