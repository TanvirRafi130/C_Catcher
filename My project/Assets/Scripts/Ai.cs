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
    Coroutine movingCoroutine;
    Coroutine waitCoroutine;
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
        // movingCoroutine = Move(Vector3.zero); // Initialize the coroutine

    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        // Debug.LogError(distance);

        if (distance <= avoidDistance && isWaitOver /* && !isMoving */)
        {

            StartCoroutine(WaitBeforeNewPath());
            Vector3 loc = GameManager.Instance.GeRandomLocation();
            transform.DOLookAt(loc, 0.2f);
            if (movingCoroutine != null)
            {
                StopCoroutine(movingCoroutine);
                movingCoroutine = null;
            }
            movingCoroutine = StartCoroutine(Move(loc));

        }

    }

    IEnumerator Move(Vector3 targetPos)
    {
      /*   targetPos.y = transform.localPosition.y; // Keep the y position constant */
        isMoving = true;
        animator.SetTrigger(run);
        float stopDistance = 0.5f; // Allow slight inaccuracy to prevent infinite loops
        Rigidbody rb = GetComponent<Rigidbody>();

        while (Vector3.Distance(transform.position, targetPos) >= stopDistance)
        {
            // Only move in XZ, keep Y velocity unchanged (let physics handle Y)
            Vector3 direction = (targetPos - transform.position).normalized;
            Vector3 velocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);
            rb.linearVelocity = velocity;
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
        //rb.Sleep();
        isMoving = false;
        // Reset all triggers before setting new ones (loop through all parameters)
        for (int i = 0; i < animator.parameterCount; i++)
        {
            var param = animator.GetParameter(i);
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
        if(animator.GetBool(run) == true)
        {
            animator.ResetTrigger(run);
        }
        animator.SetTrigger(idle);

        Debug.LogError("eccc"); // Now this will execute!
    }

    bool isWaitOver = true;

    IEnumerator WaitBeforeNewPath()
    {
        isWaitOver = false;
        yield return new WaitForSeconds(1f); // Adjust the wait time as needed
        isWaitOver = true;
    }





}
