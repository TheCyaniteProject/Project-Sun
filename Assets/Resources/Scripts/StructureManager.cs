using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UIManager;

public class StructureManager : MonoBehaviour
{
    public static StructureManager Instance;
    public Structure selectedStructure;

    public bool isBuilding = false;
    public Structure structurePrefab;
    public GameObject ghost;
    [Space]
    public List<Structure> structureList;
    public List<StructureItem> structures;
    [HideInInspector]public List<Structure> yards = new List<Structure>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UIManager.Instance.PopulateStructures(structures);
    }

    private void Update()
    {
        if (yards.Count == 0)
            UIManager.Instance.ClearStructures();

        if (selectedStructure)
        {
            if (Input.GetMouseButtonDown(0) && !UIManager.Instance.mouseOverUI)
            {
                selectedStructure.DeSelect();
            }
        }

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
                foreach (NavMeshObstacle obstacle in ghost.GetComponentsInChildren<NavMeshObstacle>())
                {
                    Destroy(obstacle);
                }

                ghost.SetActive(true);
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Floor")))
            {
                ghost.transform.position = hit.point;
            }

            if (Input.GetMouseButtonDown(0) && !UIManager.Instance.mouseOverUI)
            {
                // place
                Structure newStructure = Instantiate(structurePrefab, transform);
                newStructure.transform.position = ghost.transform.position;
                newStructure.gameObject.SetActive(true);
                newStructure.teamID = PlayerData.Instance.teamID;
                ClearBuildStructure();
                UIManager.Instance.EnableStructures();
            }
            else if (Input.GetMouseButtonDown(1) && !UIManager.Instance.mouseOverUI)
            {
                PlayerData.Instance.AddMoney(structurePrefab.cost);
                UIManager.Instance.EnableStructures();
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

    public void SelectStructure(Structure structure)
    {
        if (selectedStructure)
        {
            selectedStructure.DeSelect();
        }
        selectedStructure = structure;
    }
}
