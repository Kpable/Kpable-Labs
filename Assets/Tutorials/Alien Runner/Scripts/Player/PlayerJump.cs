using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace Kpable.Tutorials.AlienRunner
{
    public class PlayerJump : MonoBehaviour
    {

        //[SerializeField]
        //private AudioClip jumpClip;

        private Rigidbody2D myBody;

        private float jumpForce = 24f, forwardForce = 0f;

        private bool canJump;

        private Button jumpBtn;

        private Animator anim;

        void Awake()
        {
            myBody = GetComponent<Rigidbody2D>();

            jumpBtn = GameObject.Find("Jump Button").GetComponent<Button>();

            jumpBtn.onClick.AddListener(() => Jump());

            anim = GetComponent<Animator>();
        }

        public void Jump()
        {
            if (canJump)
            {
                canJump = false;

                //AudioSource.PlayClipAtPoint(jumpClip, transform.position);
                if (transform.position.x < 0)
                {
                    forwardForce = 3f;
                }
                else
                {
                    forwardForce = 0f;
                }

                myBody.velocity = new Vector2(forwardForce, jumpForce);
                anim.SetBool("isJumping", true);
                anim.SetBool("Grounded", false);

            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Mathf.Abs(myBody.velocity.y) == 0)
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("Grounded", true);
                canJump = true;

            }
        }
    }
}
