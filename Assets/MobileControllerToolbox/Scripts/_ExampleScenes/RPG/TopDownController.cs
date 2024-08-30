using UnityEngine;


namespace MobileController
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class TopDownController : MonoBehaviour
    {
        [Header("Movement")]
        public Vector2 speed = Vector2.one;
        public float dashSpeed = 25f;
        public float placeRadious;
        [Space(15)]

        [Header("References")]
        public GameObject swordIn;
        public GameObject swordOut;
        public GameObject ballObj;
        public GameObject dustPrt;
        [Space(10)]
        public Analog analog;
        public Dpad dpad;
        public ArrowKeys arrowKeys;
        public Animator anim;


        private Vector3 facingRight = new Vector3(1, 1, 1);
        private Vector3 facingLeft = new Vector3(-1, 1, 1);

        private Rigidbody2D rb;
        private Vector2Int input;
        private GameObject ballRef;

        private bool canWalk = false;
        private bool moveBall = false;


        /**/


        #region Setup

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        #endregion


        #region Move

        void Update()
        {
            // handle input
            input = Vector2Int.zero;
            if (dpad.gameObject.activeSelf) input = dpad.GetDpadDirection();
            else if (arrowKeys.gameObject.activeSelf) input = arrowKeys.GetArrowKeysDirection();

            // handle movement
            if (input.x > 0) this.transform.localScale = facingRight;
            else if (input.x < 0) this.transform.localScale = facingLeft;

            if (!canWalk && rb.velocity.magnitude < 0.5f) canWalk = true;


            // handle animation
            if (input.x != 0 || input.y != 0)
            {
                anim.Play("Walk");
                dustPrt.SetActive(true);
            }
            else
            {
                anim.Play("Idle");
                dustPrt.SetActive(false);
            }


            // handle ball position
            if (moveBall) MoveBall();
        }


        public void DashLeft()
        {
            canWalk = false;

            this.transform.localScale = facingLeft;
            rb.AddForce(new Vector2(-dashSpeed, 0f), ForceMode2D.Impulse);
        }


        public void DashRight()
        {
            canWalk = false;

            this.transform.localScale = facingRight;
            rb.AddForce(new Vector2(dashSpeed, 0f), ForceMode2D.Impulse);
        }
        

        public void ShowSword()
        {
            swordIn.SetActive(false);
            swordOut.SetActive(true);
        }


        public void HideSword()
        {
            swordIn.SetActive(true);
            swordOut.SetActive(false);
        }


        public void InstanciateBall()
        {
            ballRef = GameObject.Instantiate(ballObj, this.transform.position, Quaternion.identity);
            moveBall = true;
        }


        public void MoveBall()
        {
            ballRef.transform.position = this.transform.position + (Vector3)analog.GetStickPosition() * placeRadious;
        }


        public void PlaceBall()
        {
            ballRef = null;
            moveBall = false;
        }


        void FixedUpdate()
        {
            if(canWalk) rb.velocity = new Vector2(input.x * speed.x, input.y * speed.y);
        }

        #endregion
    }
}