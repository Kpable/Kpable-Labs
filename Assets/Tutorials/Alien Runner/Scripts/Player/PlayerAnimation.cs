using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    private Animator anim;

	void Awake () {
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", 1f);
	}
	
    void OnCollisionEnter2D(Collision2D target)
    {
       // Debug.Log("Collided With target: " + target.gameObject.name);

        if (target.gameObject.name.Contains("obstacle"))
        {
           // Debug.Log("Collided With Obstacle");
            //anim.SetBool("isWalking", false);
            anim.SetFloat("Speed", 0f);
        }
    }

    void OnCollisionExit2D(Collision2D target)
    {
        //Debug.Log("Collided With target: " + target.gameObject.name);

        if (target.gameObject.name.Contains("obstacle"))
        {
            //Debug.Log("Collided With Obstacle");
            anim.SetBool("isWalking", true);
            anim.SetFloat("Speed", 1f);

        }
    }
}
