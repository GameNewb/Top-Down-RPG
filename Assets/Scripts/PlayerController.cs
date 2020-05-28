using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRigidBody2D;
    [SerializeField] Animator animator;

    [SerializeField] float moveSpeed = 10f;

    public static PlayerController instance;
    public string sceneTransitionName = "";

    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    public bool isLoading = false;

    // Start is called before the first frame update
    void Start()
    {
        // Keep only 1 instance of the object
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // Make sure we're deleting a "copy" and not a newly loaded "Player" object (i.e. if Player obj exists in 2 scenes, delete the one in the new scene we transitioned to)
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        if (playerRigidBody2D == null)
        {
            playerRigidBody2D = GetComponent<Rigidbody2D>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        // Keep player in the scene when transitioning
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Disable controls when loading
        if (isLoading == false)
        {
            float xAxis = Input.GetAxisRaw("Horizontal");
            float yAxis = Input.GetAxisRaw("Vertical");

            playerRigidBody2D.velocity = (new Vector2(xAxis, yAxis)) * moveSpeed;

            animator.SetFloat("moveX", playerRigidBody2D.velocity.x);
            animator.SetFloat("moveY", playerRigidBody2D.velocity.y);

            // Stop left/right motion
            if (xAxis == 1f || xAxis == -1f || yAxis == 1f || yAxis == -1f)
            {
                animator.SetFloat("lastMoveX", xAxis);
                animator.SetFloat("lastMoveY", yAxis);
            }

            // Keep Player within tilemap bounds
            /*float xCameraClamp = Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x);
            float yCameraClamp = Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y);
            transform.position = new Vector3(xCameraClamp, yCameraClamp, transform.position.z);*/
        }
    }

    // Restrict player movement based on Tilemap boundaries
    public void SetBounds(Vector3 bottomLeft, Vector3 topRight)
    {
        Vector3 leftBoundaryReduction = new Vector3(0.3f, 0.5f, 0f);
        Vector3 rightBoundaryReduction = new Vector3(-0.3f, -0.5f, 0f);

        bottomLeftLimit = bottomLeft + leftBoundaryReduction;
        topRightLimit = topRight + rightBoundaryReduction;
    }
}
