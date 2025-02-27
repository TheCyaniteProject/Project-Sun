using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIManager;

[RequireComponent(typeof(LineRenderer))]
public class UnitStructure : Structure
{
    public Transform spawnPoint;
    public Transform destinationPoint;
    public Vector3 targetPosition;
    public List<UnitItem> units;
    public LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        targetPosition = destinationPoint.position;
    }

    public override void Update()
    {
        base.Update();
        if (isSelected)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, new Vector3(spawnPoint.position.x, 0, spawnPoint.position.z));
            lineRenderer.SetPosition(1, new Vector3(destinationPoint.position.x, 0, destinationPoint.position.z));
            //lineRenderer.SetPosition(3, new Vector3(targetPosition.x, 0, targetPosition.z));
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    public virtual void SpawnUnit(Unit unit)
    {
        if (PlayerData.Instance.money >= unit.cost)
        {
            PlayerData.Instance.RemoveMoney(unit.cost);

            Unit newUnit = Instantiate(unit, UnitManager.Instance.transform);
            newUnit.transform.position = spawnPoint.position;
            newUnit.gameObject.SetActive(true);
            newUnit.SetTarget(destinationPoint.position);
            newUnit.teamID = teamID;
            UnitManager.Instance.unitList.Add(newUnit);
        }
    }

    public override void Select()
    {
        base.Select();
        UIManager.Instance.PopulateUnits(units);
    }

    public override void DeSelect()
    {
        if (StructureManager.Instance.selectedStructure == this)
        {
            UIManager.Instance.ClearUnits();
        }
        base.DeSelect();
    }

    private void OnDestroy()
    {
        DeSelect();
    }
}
