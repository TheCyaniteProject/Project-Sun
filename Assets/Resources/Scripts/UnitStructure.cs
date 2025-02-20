using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIManager;

public class UnitStructure : Structure
{
    public Transform insidePoint;
    public Transform spawnPoint;
    public List<UnitItem> units;

    public virtual void SpawnUnit(Unit unit)
    {
        Unit newUnit = Instantiate(unit, UnitManager.Instance.transform);
        newUnit.transform.position = insidePoint.position;
        newUnit.gameObject.SetActive(true);
        newUnit.SetTarget(spawnPoint.position);
    }

    public override void Select()
    {
        base.Select();
        UIManager.Instance.PopulateUnits(units);
    }

    public override void DeSelect()
    {
        base.DeSelect();
        UIManager.Instance.ClearUnits();
    }
}
