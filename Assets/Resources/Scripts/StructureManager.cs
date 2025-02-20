using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public static StructureManager Instance;
    public Structure selectedStructure;

    public bool isBuilding = false;
    public Structure structurePrefab;
    public GameObject ghost;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (isBuilding)
        {
            if (!structurePrefab)
            {
                isBuilding = false;
            }

            // show ghost
            if (!ghost)
            {
                ghost = Instantiate(structurePrefab.gameObject);
                ghost.name = "Ghost Preview";
                Destroy(ghost.GetComponent<Structure>());
                foreach (Collider collider in ghost.GetComponentsInChildren<Collider>())
                {
                    Destroy(collider);
                }
                ghost.SetActive(true);
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Floor")))
            {
                Debug.Log(hit.transform.name);
                ghost.transform.position = hit.point;
            }

            if (Input.GetMouseButtonDown(0))
            {
                // place
                Structure newStructure = Instantiate(structurePrefab, transform);
                newStructure.transform.position = ghost.transform.position;
                newStructure.gameObject.SetActive(true);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ClearBuildStructure();
            }
        }
    }

    public void ClearBuildStructure()
    {
        isBuilding = false;
        structurePrefab = null;
        Destroy(ghost);
    }

    public void SetBuildStructure(Structure structure)
    {
        ClearBuildStructure();
        isBuilding = true;
        structurePrefab = structure;
    }
}
