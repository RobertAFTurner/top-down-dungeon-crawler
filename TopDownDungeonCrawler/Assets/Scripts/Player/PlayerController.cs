using UnityEngine;

public class PlayerController : EntityController
{
    [SerializeField]
    private LayerMask floorLayerMask;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float startingDodgeSpeed;

    private float dodgeSpeed;

    private Vector3 dodgeDirection;

    private bool dodging = false;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform rendererTransform;

    [SerializeField]
    private int hitPoints;
    
    private Rigidbody2D playerRigidBody;

    void Start()
    {
        dodgeSpeed = startingDodgeSpeed;
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {      
        if(!dodging)
            HandleMove();
        
        HandleDodge();
        HandleDeath();
    }

    private void HandleDodge()  
    {
      if(Input.GetKeyDown(KeyCode.Space))
        {
            dodging = true;
            if (Input.GetKey(KeyCode.A)) // Left
            {
                animator.SetBool("Dodge_S", true);
                rendererTransform.rotation = new Quaternion(0, 180, 0, 0);
                dodgeDirection = Vector3.left;
            }
            else if (Input.GetKey(KeyCode.D)) // Right
            {
                animator.SetBool("Dodge_S", true);
                dodgeDirection = Vector3.right;
            }
            else if (Input.GetKey(KeyCode.W)) // Up
            {
                animator.SetBool("Dodge_U", true);
                dodgeDirection = Vector3.up;
            }

            else if (Input.GetKey(KeyCode.S)) // Down
            {
                animator.SetBool("Dodge_D", true);
                dodgeDirection = Vector3.down;
            }

        }

      if(dodging)
        {
            if(dodgeDirection == Vector3.left)
                playerRigidBody.velocity = new Vector2(-dodgeSpeed, playerRigidBody.velocity.y);
            if (dodgeDirection == Vector3.right)
                playerRigidBody.velocity = new Vector2(dodgeSpeed, playerRigidBody.velocity.y);
            if (dodgeDirection == Vector3.up)
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, dodgeSpeed);
            if (dodgeDirection == Vector3.down)
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, -dodgeSpeed);

            dodgeSpeed -= dodgeSpeed * 10f * Time.deltaTime;

            if(dodgeSpeed < 3f)
            {
                dodgeSpeed = startingDodgeSpeed;
                dodging = false;
                animator.SetBool("Dodge_S", false);
                animator.SetBool("Dodge_U", false);
                animator.SetBool("Dodge_D", false);
            }
        }
    }

    private void HandleMove()
    {
        playerRigidBody.velocity = Vector2.zero;
        animator.SetBool("Running_RD", false);
        animator.SetBool("Running_D", false);
        animator.SetBool("Running_U", false);

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
            animator.SetBool("Running_U", true);
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, speed);
        }
        else if (Input.GetKey(KeyCode.S)) // Down
        {
            animator.SetBool("Running_D", true);
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, -speed);
        }
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
    }

    public override void HandleDeath()
    {
        if (hitPoints <= 0)
            Destroy(gameObject);
    }
}
