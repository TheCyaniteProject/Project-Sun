using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// This is the base class. It's intended to be inhereted, but for now is used for all units
[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject selectionOutline;
    [Space]
    public int maxHealth = 100;
    public int health = 100;
    public int cost = 100;
    public int refund = 75;
    [Space]
    public Vector3 targetPos;
    public bool isStopped;

    private bool isSelected = false;
    private bool mouseOver = false;

    private void Awake()
    {
        targetPos = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UIManager.Instance.mouseOverUI)
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
        else if (Input.GetMouseButtonUp(1) && !UIManager.Instance.mouseOverUI)
        {
            if (isSelected)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Floor")))
                {
                    Debug.Log(name +": Target set to " + hit.point);
                    SetTarget(hit.point);
                }
            }
        }

        if (Vector3.Distance(transform.position, targetPos) < 1.5f)
        {
            agent.isStopped = true;
        }
        isStopped = agent.isStopped;
    }

    public void SetTarget(Vector3 position)
    {
        agent.isStopped = false;
        targetPos = position;
        agent.destination = targetPos;
    }

    public void Select()
    {
        if (selectionOutline != null)
        {
            selectionOutline.SetActive(true);
        }
        isSelected = true;
    }

    public void DeSelect()
    {
        if (selectionOutline != null)
        {
            selectionOutline.SetActive(false);
        }
        isSelected = false;
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
