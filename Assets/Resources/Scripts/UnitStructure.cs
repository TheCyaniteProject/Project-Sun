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
        Debug.Log(UnitManager.Instance.name);
        Unit newUnit = Instantiate(unit, UnitManager.Instance.transform);
        newUnit.transform.position = insidePoint.position;
        newUnit.gameObject.SetActive(true);
        newUnit.SetTarget(spawnPoint.position);
        UnitManager.Instance.unitList.Add(newUnit);
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
