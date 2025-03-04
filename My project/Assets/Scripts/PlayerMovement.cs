using UnityEngine;
using NaughtyAttributes;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;

    [AnimatorParam("animator")] public string Catch;
    bool isCatchCalled = false;
    [AnimatorParam("animator")] public string speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
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

}
