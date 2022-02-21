using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ZombieBehaviour : GazePointer
{
    // Start is called before the first frame update
    private int currentHealth;
    public int health = 100;
    public GameObject particalEffect;
    private Animator anim;

    public enum State
    {
        Idle,
        Follow,
        Die,
        Attack,
    }
    public State state = State.Idle;
    public Transform target;
    public float rotateSpeed = 0.3f;
    public float followRange = 10.0f;
    public float idleRange = 10.0f;

    private NavMeshAgent agent;
    bullets bulletspawn;
    IEnumerator IdleState()
    {
        Debug.Log("Idle:Enter");

        agent.isStopped = true;
        anim.SetFloat("speed", 0);
        anim.SetBool("attacking", false);

        while (state == State.Idle)
        {
            if (GetDistance() > followRange)
            {
                state = State.Follow;
            }
            yield return 0;
        }
        Debug.Log("Idle:Exit");
        GoToNextState();
    }
    public float GetDistance()
    {
        return (transform.position - target.transform.position).magnitude;
    }
    
    void GoToNextState()
    {
        StopAllCoroutines();
        string methodName = state.ToString() + "State";
    System.Reflection.MethodInfo info = GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    StartCoroutine((IEnumerator)info.Invoke(this,null));
    }
    IEnumerator FollowState()
    {
        Debug.Log("Follow:Enter");

        while (state == State.Follow)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            anim.SetFloat("speed", agent.velocity.magnitude);
            anim.SetBool("attacking", false);
            if (GetDistance() > idleRange)
            {
                state = State.Idle;
            }
            else if  (GetDistance()<=agent.stoppingDistance + 0.05f &&(agent.pathStatus == NavMeshPathStatus.PathComplete))
            {
                state = State.Attack;
            }
            yield return new WaitForSeconds(.2f);
        }
        Debug.Log("Follow:Exit");
        GoToNextState();
    }
    IEnumerator DieState()
    {
        agent.isStopped = true;
        anim.SetBool("attacking", false);
        anim.SetBool("dead", true);

        Debug.Log("Die:Enter");
            Destroy(this.gameObject, 5);
        yield return 0;
    }
    IEnumerator AttackState()
    {
        Debug.Log("Attack:Enter");

        anim.SetFloat("speed", 0);
        anim.SetBool("attacking", true);
        while (state == State.Attack)
        {
            RotateTowards(target);
            if (GetDistance()>(agent.stoppingDistance + 1))
            {
                state = State.Follow;
            }
            yield return 0;
        }
        Debug.Log("Attack:Exit");
        GoToNextState();
    }

    private void RotateTowards(Transform target )
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
    }
    void Start()
    {
        currentHealth = health;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bulletspawn = GameObject.FindObjectOfType<bullets>();
        GoToNextState();
    }

    // Update is called once per frame
    void Update()
    {

    }
    // public void OnCollisionEnter(Collision collision)
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        bulletspawn.ShootBullet();
        if (currentHealth > 0)
        {
            TakeDamage(UnityEngine.Random.Range(5, 20));
            Death();
        }
    }
    private void TakeDamage(int damageToDeal=0)
    {
        currentHealth -= damageToDeal;
        if (currentHealth <= 0)
        {
            anim.SetBool("dead", true);
            Destroy(this);
        }
        else
        {
            followRange = Mathf.Max(GetDistance(), followRange);
            state = State.Follow;
            anim.SetTrigger("hit");
        }
        GoToNextState();
    }
    public int damageAmount = 20;

    public void PhysicalAttack()
    {
        if (GetDistance()<=agent.stoppingDistance + 0.5f)
        {
            target.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
        }
    }
   /* public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Death();
    }*/
    public void Death()
    {
        particalEffect.SetActive(true);
        particalEffect.transform.SetParent(null);
        Destroy(this);
    }
}
