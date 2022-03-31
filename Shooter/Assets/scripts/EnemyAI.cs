using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;

    NavMeshAgent navMashAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    EnemyHealth health;
    void Start()
    {
        navMashAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
    }


    void Update()
    {
        if (health.IsDead())
        {
            this.enabled = false;
            navMashAgent.enabled = false;
            return;
        }
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if(isProvoked)
        {
            EngageTarget();
        } 
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true; 
        }

    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }
    private void EngageTarget() 
    {
        FaceTarget();
        if (distanceToTarget <= navMashAgent.stoppingDistance)
        {
            AttackTarget();
        }

        if (distanceToTarget >= navMashAgent.stoppingDistance)
        {
            ChaseTarget();
        }
    }

    private void AttackTarget()
    {
        GetComponent<Animator>().SetBool("attack", true);
    }

    private void ChaseTarget()
    {       
        GetComponent<Animator>().SetTrigger("move");
        GetComponent<Animator>().SetBool("attack", false);
        navMashAgent.SetDestination(target.position);
    }
    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
