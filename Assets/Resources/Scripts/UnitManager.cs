using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    public RectTransform selectionBox;
    public float unitRadius = 5f;

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
        if (Input.GetMouseButtonUp(1) && !UIManager.Instance.mouseOverUI)
        {
            // get positions
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Floor")))
            {
                int factor = (int)Math.Round(selectedUnits.Count / 5.0f, MidpointRounding.AwayFromZero);
                if (factor == 0) factor = 1;

                Vector3[] positions = GetPositionsArray(hit.point, Enumerable.Range(1, factor).Select(x => x * 5).ToArray(), Enumerable.Range(1, factor).Select(x => unitRadius * x).ToArray());
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                        selectedUnits[i].SetTarget(positions[i]);
                }
            }
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

    public Vector3[] GetPositionsArray(Vector3 targetPosition, int[] counts, float[] radiuses)
    {
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < radiuses.Length; i++)
        {
            positions.AddRange(GetPositionsArray(targetPosition, counts[i], radiuses[i]));
        }

        return positions.ToArray();
    }

    public Vector3[] GetPositionsArray(Vector3 targetPosition, int count, float radius)
    {
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            float angle = i * (360f / count);
            Vector3 dir = ApplyRotationToVector(new Vector3(1,0), angle);
            Vector3 position = targetPosition + dir * radius;
            positions.Add(position);
        }

        return positions.ToArray();
    }

    private Vector3 ApplyRotationToVector(Vector3 vector, float angle)
    {
        return Quaternion.Euler(0, angle, 0) * vector;
    }
}
