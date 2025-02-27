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
    public int attackDamage = 8;
    public float attackRange = 10;
    [Space]
    public Vector3 targetPos;
    public GameObject enemy;
    public Vector3 targetingOffset;
    [Space]
    public Animator animator;
    public LineRenderer lineRenderer;
    public ParticleSystem muzzleflash;
    public ParticleSystem groundImpact;
    public AudioSource shootAudio;
    public Color moveColor = Color.white;
    public Color attackColor = Color.white;


    [HideInInspector]public bool isStopped;
    [HideInInspector]private bool isSelected = false;
    [HideInInspector]private bool mouseOver = false;
    [HideInInspector]public bool isDead = false;

    [HideInInspector]public List<GameObject> targets = new List<GameObject>();

    private void Awake()
    {
        targetPos = transform.position;
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    bool selecting = false;
    Vector3 lastPosition;
    float shootTimer = 0f;
    float deathTimer = 5f;
    public virtual void Update()
    {
        if (!isDead)
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
            lineRenderer.SetPosition(1, new Vector3(targetPos.x, 0, targetPos.z));
            if (isSelected)
            {
                lineRenderer.enabled = true;
            }
            else
            {
                lineRenderer.enabled = false;
            }

            lastPosition = transform.position;

            if (isStopped && (!enemy || enemy.GetComponent<Unit>() && enemy.GetComponent<Unit>().isDead))
            {
                enemy = GetClosestValidTarget();
            }

            if (isStopped && enemy && Vector3.Distance(transform.position, enemy.transform.position) > attackRange)
            {
                agent.stoppingDistance = attackRange * 0.8f;
                SetTarget(enemy.transform.position);
            }
            else if (isStopped && enemy && Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
            {
                animator.Play("Fire");
                Vector3 targetPostition = new Vector3(enemy.transform.position.x,
                                           this.transform.position.y,
                                           enemy.transform.position.z);
                this.transform.LookAt(targetPostition);
                transform.eulerAngles += targetingOffset;
                lineRenderer.SetPosition(1, new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z));
                groundImpact.transform.position = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);

                shootTimer += Time.deltaTime;
                if (shootTimer >= 1f)
                {
                    Shoot();

                    shootTimer = 0f;
                }
            }
            if (enemy)
            {
                lineRenderer.material.color = attackColor;
            }
            else
            {
                lineRenderer.material.color = moveColor;
            }

            if (isStopped && !enemy)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    animator.Play("Idle");
                    animator.SetFloat("offset", Random.Range(0f, 1f));
                }
            }
            else if (!isStopped)
            {
                animator.Play("Run");
            }
        }
        else
        {
            DeSelect();
            deathTimer -= Time.deltaTime;
            if (deathTimer < 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Damage(int amount)
    {
        if (health == maxHealth)
        {
            if (teamID == PlayerData.Instance.teamID)
            {
                UIManager.Instance.voice.clip = UIManager.Instance.unitAttacked;
                UIManager.Instance.voice.Play();
            }
        }
        health -= amount;
        if (health <= 0f)
        {
            isDead = true;
            animator.applyRootMotion = true;
            animator.Play("Death");

            if (UnitManager.Instance.unitList.Contains(this))
            {
                UnitManager.Instance.unitList.Remove(this);
            }
            if (teamID == PlayerData.Instance.teamID)
            {
                UIManager.Instance.voice.clip = UIManager.Instance.unitLost;
                UIManager.Instance.voice.Play();
            }
        }
    }

    public GameObject GetClosestValidTarget()
    {
        GameObject _target = null;
        if (targets.Count != 0)
        {
            foreach (var target in targets)
            {
                if (target && ((target.GetComponent<Unit>() && !target.GetComponent<Unit>().isDead) || target.GetComponent<Structure>()))
                {
                    if (!_target)
                    {
                        _target = target;
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, target.transform.position) < Vector3.Distance(transform.position, _target.transform.position))
                        {
                            _target = target;
                        }
                    }
                }
            }
        }

        return _target;
    }

    private void Shoot()
    {
        muzzleflash.Play();
        shootAudio.Play();
        groundImpact.Play();

        //TODO check if unit or structure
        if (enemy)
        {
            if (enemy.GetComponent<Unit>())
            {
                enemy.GetComponent<Unit>().Damage(attackDamage);
            }
            else if (enemy.GetComponent<Structure>())
            {
                enemy.GetComponent<Structure>().Damage(attackDamage);
            }
        }
    }

    public void SetTarget(Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 100f, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.destination = hit.position;
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
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
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
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
        isSelected = false;
        if (UnitManager.Instance.selectedUnits.Contains(this))
        {
            UnitManager.Instance.selectedUnits.Remove(this);
        }
    }

    public void OnEnter(Collider other)
    {
        GameObject target = other.transform.gameObject;
        if (target && target.GetComponent<Unit>() && !target.GetComponent<Unit>().isDead)
        {
            if (target.GetComponent<Unit>().teamID != teamID)
            {
                if (!targets.Contains(target))
                {
                    targets.Add(target);
                }
            }
        }
        else if (target && target.GetComponent<Structure>())
        {
            if (target.GetComponent<Structure>().teamID != teamID)
            {
                if (!targets.Contains(target))
                {
                    targets.Add(target);
                }
            }
        }
    }

    public void OnExit(Collider other)
    {
        if (enemy && other.transform == enemy.transform && agent.stoppingDistance == 0)
        {
            enemy = null;
        }
        if (targets.Contains(other.transform.gameObject))
        {
            if (!other.transform.gameObject == enemy.gameObject)
            {
                targets.Remove(other.transform.gameObject);
            }
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

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
