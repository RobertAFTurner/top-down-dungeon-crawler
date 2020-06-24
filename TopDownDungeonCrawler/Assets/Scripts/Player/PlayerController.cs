using UnityEngine;

public class PlayerController : EntityController
{
    [SerializeField]
    private LayerMask floorLayerMask;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform rendererTransform;

    private float jumpTime;
    private bool isJumping = false;
    private Rigidbody2D playerRigidBody;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {        
        HandleMove();
    }

    private void HandleMove()
    {
        playerRigidBody.velocity = Vector2.zero;
        animator.SetBool("Running_RD", false);
        animator.SetBool("Running_D", false);

        if (Input.GetKey(KeyCode.A)) // Left
        {
            animator.SetBool("Running_RD", true);
            rendererTransform.rotation = new Quaternion(0, 180, 0, 0);
            playerRigidBody.velocity = new Vector2(-speed, playerRigidBody.velocity.y);            
        }
        else if (Input.GetKey(KeyCode.D)) // Right
        {
            animator.SetBool("Running_RD", true);
            rendererTransform.rotation = new Quaternion(0, 0, 0, 0);
            playerRigidBody.velocity = new Vector2(speed, playerRigidBody.velocity.y);            
        }

        if(Input.GetKey(KeyCode.W)) // Up
        {
            //animator.SetBool("IsRunning", true);
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, speed);
        }
        else if (Input.GetKey(KeyCode.S)) // Down
        {
            animator.SetBool("Running_D", true);
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, -speed);
        }
    }

    public override void HandleDeath()
    {
        Destroy(gameObject);
    }
}
