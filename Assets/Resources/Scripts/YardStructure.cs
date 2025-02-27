using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YardStructure : Structure
{
    private void Start()
    {
        StructureManager.Instance.yards.Add(this);
    }

    private void OnDestroy()
    {
        StructureManager.Instance.yards.Remove(this);
    }
}
