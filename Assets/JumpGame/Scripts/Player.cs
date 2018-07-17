using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private static Player instance;    

    public Rigidbody2D myRigidbody { get; set; }  

    public Animator myAnimator { get; set; }  

    [SerializeField]
    private float movementSpeed;

    private bool attack;

    private bool slide;

    private bool facingRight;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jump;

    private bool jumpAttack;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    public bool OnLadder { get; set; } 

    [SerializeField]
    private float climbSpeed; 

    private IUsable usable;

    public static Player Instance   
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

	// Use this for initialization
	void Start ()
    {
        OnLadder = false;    
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();


	}

    void Update()
    {
        HandleInput();

    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");    

        isGrounded = IsGrounded();

        HandleMovement(horizontal,vertical);       
        Flip(horizontal);
        HandleAttacks();
        HandleLayers();
        ResetValues();
	}

    private void HandleMovement(float horizontal, float vertical)
    {
        if(myRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }

        if (!myAnimator.GetBool("slide") && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && (isGrounded || airControl))
        {
            myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
        }

        if(isGrounded && jump && !OnLadder)
        {
            isGrounded = false;
            myRigidbody.AddForce(new Vector2(0, jumpForce));
            myAnimator.SetTrigger("jump");
        }

        if(slide && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        {
            myAnimator.SetBool("slide", true);
        }
        else if(!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        {
            myAnimator.SetBool("slide", false);
        }

        if(OnLadder)              
        {
            myAnimator.speed = vertical != 0 ? Mathf.Abs(vertical) : Mathf.Abs(horizontal);
            myRigidbody.velocity = new Vector2(horizontal * climbSpeed, vertical * climbSpeed);
        }


        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));

    }

    private void HandleAttacks()
    {
        if(attack && isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myAnimator.SetTrigger("attack");
            myRigidbody.velocity = Vector2.zero;
        }

        if(jumpAttack && !isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
        {
            myAnimator.SetBool("jumpAttack", true);
        }

        if(!jumpAttack && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
        {
            myAnimator.SetBool("jumpAttack", false);
        }
        
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !OnLadder)
        {
            jump = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            attack = true;
            jumpAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            slide = true;
        }
        if (Input.GetKeyDown(KeyCode.E))       
        {
            Use();
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;
        }
    }

    private void ResetValues()
    {
        attack = false;
        slide = false;
        jump = false;
        jumpAttack = false;
    }

    private bool IsGrounded()
    {
        if(myRigidbody.velocity.y <= 0)
        {
            foreach(Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("land", false);
                        return true;
                    }
                }

                
            }

        }
        return false;
    }

    private void HandleLayers()
    {
        if(!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }

    private void Use()    
    {
        if(usable != null)
        {
            usable.Use();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Star")
        {
            AudioSource.PlayClipAtPoint(GameManager.Instance.starSound, gameObject.transform.position);
            GameManager.Instance.CollectedStars++;
            Destroy(other.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)    
    {
        if(other.tag == "Usable")
        {
            usable = other.GetComponent<IUsable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)     
    {
        if(other.tag == "Usable")
        {
            usable = null;
        }
    }
}
