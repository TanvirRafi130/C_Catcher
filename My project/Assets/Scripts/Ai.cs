using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Ai : MonoBehaviour
{


    [SerializeField] float avoidDistance = 5f;
    [SerializeField] float searchRadius = 10f; // Area to search for a random point
    [SerializeField] float speed = 5f; // Adjust speed as needed
    [SerializeField] Animator animator;
    [SerializeField, AnimatorParam("animator")] string idle;
    [SerializeField, AnimatorParam("animator")] string run;
    Rigidbody rb;
    Player player;
    bool isMoving = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.Instance;

    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        // Debug.LogError(distance);

        if (distance <= avoidDistance && !isMoving)
        {
            Vector3 loc = GameManager.Instance.GeRandomLocation();
            transform.DOLookAt(loc,0.2f);
            StartCoroutine(Move(loc));

        }

    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        animator.SetTrigger(run);
        float stopDistance = 0.5f; // Allow slight inaccuracy to prevent infinite loops
        Rigidbody rb = GetComponent<Rigidbody>();

        while (Vector3.Distance(transform.position, targetPos) >= stopDistance)
        {
            //  Debug.LogError(Vector3.Distance(transform.position, targetPos));
            Vector3 direction = (targetPos - transform.position).normalized;
            rb.linearVelocity = direction * speed; // Move towards target
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
        rb.Sleep();
        isMoving = false;
        animator.SetTrigger(idle);

        //  Debug.LogError("eccc"); // Now this will execute!
    }





}
