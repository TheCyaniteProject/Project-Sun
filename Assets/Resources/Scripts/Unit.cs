using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

// This is the base class. It's intended to be inhereted, but for now is used for all units
[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(LineRenderer))]
public class Unit : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject selectionOutline;
    [Space]
    public int teamID = 0;
    public int maxHealth = 100;
    public int health = 100;
    public int cost = 100;
    public int refund = 75;
    [Space]
    public Vector3 targetPos;
    public bool isStopped;

    private bool isSelected = false;
    private bool mouseOver = false;
    [Space]
    public Animator animator;
    public LineRenderer lineRenderer;

    private void Awake()
    {
        targetPos = transform.position;
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    bool selecting = false;
    Vector3 lastPosition;
    public virtual void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UIManager.Instance.mouseOverUI)
        {
            if (mouseOver)
                selecting = true;
        }
        if (selecting && Input.GetMouseButtonUp(0) && !UIManager.Instance.mouseOverUI)
        {
            if (mouseOver)
            {
                Select();
            }
        }
        if (UnitManager.Instance._isDragging && !UnitManager.Instance.selectedUnits.Contains(this))
        {
            if (isSelected)
            {
                DeSelect();
            }
        }

        if (isStopped)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.Play("Idle");
                animator.SetFloat("offset", Random.Range(0f, 1f));
            }
        }
        else
        {
            animator.Play("Run");
        }

        float speed = (lastPosition - transform.position).magnitude;
        if (speed <= 0.01f && !isStopped)
        {
            isStopped = true;
        }
        else if (speed > 0.01f)
        {
            isStopped = false;
        }

        lineRenderer.SetPosition(0, new Vector3(transform.position.x, 0, transform.position.z));
        lineRenderer.SetPosition(1, targetPos);
        if (isSelected)
        {
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }

        lastPosition = transform.position;
    }

    public void SetTarget(Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 100f, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.destination = position;
            targetPos = agent.destination;
            agent.avoidancePriority = Random.Range(1, 100);
        }
    }

    public void Select()
    {
        if (selectionOutline != null)
        {
            selectionOutline.SetActive(true);
        }
        isSelected = true;
        if (!UnitManager.Instance.selectedUnits.Contains(this))
        {
            UnitManager.Instance.selectedUnits.Add(this);
        }
    }

    public void DeSelect()
    {
        if (selectionOutline != null)
        {
            selectionOutline.SetActive(false);
        }
        isSelected = false;
        if (UnitManager.Instance.selectedUnits.Contains(this))
        {
            UnitManager.Instance.selectedUnits.Remove(this);
        }
    }

    public void OnMouseEnter()
    {
        mouseOver = true;
    }

    public void OnMouseExit()
    {
        selecting = false;
        mouseOver = false;
    }
}
