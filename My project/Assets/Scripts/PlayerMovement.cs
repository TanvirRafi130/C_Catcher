using UnityEngine;
using NaughtyAttributes;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;
    bool canMove = true;

    [AnimatorParam("animator")] public string Catch;

    [AnimatorParam("animator")] public string speed;
    [AnimatorParam("animator")] public string trip;

    bool isCatchCalled = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            canMove = false;
            Tripping();
        }
        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Directly transform movement using camera rotation (ignores Y-axis)
        moveDirection = Camera.main.transform.rotation * new Vector3(moveX, 0f, moveZ);
        moveDirection.y = 0; // Ensure no vertical movement
        moveDirection.Normalize();
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        // Apply movement
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

        // Rotate towards movement direction
        if (moveDirection.sqrMagnitude > 0.001f) // Avoid jittering when not moving
        {
            if (!isCatchCalled)
            {
                animator.SetTrigger(Catch);
                isCatchCalled = true;
            }
            animator.SetFloat(speed, 1f);
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up); // Fix ambiguous rotation
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            animator.SetFloat(speed, 0f);
            isCatchCalled = false;
        }
    }

    void Tripping()
    {
        Vector3 tripDirection = transform.forward + Vector3.up; // Move forward + jump
        rb.AddForce(tripDirection.normalized * 5f, ForceMode.Impulse); // Apply force
        isCatchCalled = false;
        animator.SetFloat(speed, 0f);
        animator.SetTrigger("trip");
    }

    public void AllowMovement()
    {
        canMove = true;

    }

}
