using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] Animator animator;

    [SerializeField] float moveSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        if (rigidbody2D == null)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");

        rigidbody2D.velocity = (new Vector2(xAxis, yAxis)) * moveSpeed;

        animator.SetFloat("moveX", rigidbody2D.velocity.x);
        animator.SetFloat("moveY", rigidbody2D.velocity.y);
        
        // Stop left/right motion
        if (xAxis == 1f || xAxis == -1f || yAxis == 1f || yAxis == -1f)
        {
            animator.SetFloat("lastMoveX", xAxis);
            animator.SetFloat("lastMoveY", yAxis);
        }
        
    }
}
