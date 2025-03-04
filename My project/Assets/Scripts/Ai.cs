using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Ai : MonoBehaviour
{


    [SerializeField] float avoidDistance = 5f;
    [SerializeField] float searchRadius = 10f; // Area to search for a random point
    [SerializeField] float speed = 5f; // Adjust speed as needed
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

            StartCoroutine(Move(GameManager.Instance.GeRandomLocation()));

        }

    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
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
        Debug.LogError("eccc"); // Now this will execute!
    }





}
 