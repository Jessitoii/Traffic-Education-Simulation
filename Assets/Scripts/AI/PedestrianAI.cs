using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class PedestrianAI : MonoBehaviour
{
    [Header("Settings")]
    public float wanderRadius = 20f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 5f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Root'ta oldugunu varsayiyoruz
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // Agent ayarlari (CityPeople icin uygun degerler)
        agent.speed = Random.Range(1.5f, 2.5f); // Rastgele yurume hizi
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        
        // Ilk hedefe git
        SetNewRandomDestination();
    }

    void Update()
    {
        // Animator entegrasyonu
        if (animator != null)
        {
            // NavMeshAgent'in anlik hizini Animator'a gonder
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        // Hedefe vardik mi?
        if (!isWaiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                StartCoroutine(WaitAndMove());
            }
        }
    }

    IEnumerator WaitAndMove()
    {
        isWaiting = true;
        
        // Rastgele bekleme suresi
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);

        SetNewRandomDestination();
        isWaiting = false;
    }

    void SetNewRandomDestination()
    {
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        // 10.0f: NavMesh uzerinde gecerli bir nokta bulmak icin tolerans mesafesi
        NavMesh.SamplePosition(randDirection, out navHit, 10.0f, layermask);

        return navHit.position;
    }
}
