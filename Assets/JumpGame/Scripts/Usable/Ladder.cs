using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IUsable
{
    [SerializeField]
    private Collider2D platformCollider;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Use()
    {
        if(Player.Instance.OnLadder)
        {
            //stop climb
            UseLadder(false,1,0,1);
        }
        else
        {
            //start climb
            UseLadder(true,0,1,0);
            Physics2D.IgnoreCollision(Player.Instance.GetComponent<Collider2D>(), platformCollider, true);
        }
    }

    private void UseLadder(bool onLadder, int gravity, int layerWeight, int animSpeed)
    {
        Player.Instance.OnLadder = onLadder;
        Player.Instance.myRigidbody.gravityScale = gravity;
        Player.Instance.myAnimator.SetLayerWeight(2, layerWeight);
        Player.Instance.myAnimator.speed = animSpeed;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            UseLadder(false, 1,0,1);
            Physics2D.IgnoreCollision(Player.Instance.GetComponent<Collider2D>(), platformCollider, false);
        }
    }

}
