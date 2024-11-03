using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float MoveSpeed;
    public float JumpSpeed;
    public float JumpMax;
    public float JumpMin;
    public float MoveDir;
    public CinemachineVirtualCamera virtualCamera;
    public Vector3 Velocity;
    public GameObject Platform;
    private Rigidbody2D rig;
    public bool couldClimb;
    float startJumpPos;
    PlayerDir playDir;
    PlayerDir lastDir;
    bool isDead;
    public enum PlayerDir
    {
        Right,
        Left,
    }

    // Start is called before the first frame update
    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();
        couldClimb = false;
        isDead = false;
    }

    void CheckHorizontalMove()
    {
        MoveDir = Input.GetAxis("Horizontal");
        if((Velocity.x>0 && MoveDir == -1) 
            ||(Velocity.x<0 && MoveDir == 1)
            ||MoveDir == 0
            || Mathf.Abs(Velocity.x)>MoveSpeed)
        {
            float moveH = Mathf.Abs(Velocity.x);
            float introDir = Velocity.x > 0 ? 1 : -1;
            moveH -= MoveSpeed / 3;
            if (moveH < 0.01f) moveH = 0;
            Velocity.x = moveH * introDir;
        }
        else
        {
            if (MoveDir == 1)
            {
                Velocity.x += MoveSpeed / 1.5f;
                if (Velocity.x >= MoveSpeed) Velocity.x = MoveSpeed;
            }
            else if (MoveDir == -1)
            {
                Velocity.x -= MoveSpeed / 1.5f;
                if (Velocity.x <= -MoveSpeed) Velocity.x = -MoveSpeed;
            }
        }
        if(MoveDir!=0)
        {
            this.GetComponentInChildren<Animator>().SetBool("IsWalking",true);
        }
        else 
        {
            this.GetComponentInChildren<Animator>().SetBool("IsWalking", false) ;
        }
    }

    void CheckVerticalMove()
    {
        if(Input.GetAxis("Vertical") == 1 && couldClimb)
        {
            Velocity.y = MoveSpeed;
            rig.gravityScale = 0;
        }
        else if(couldClimb)
        {
            Velocity.y = 0;
            rig.gravityScale = 50;
        }
    }

    void CheckDir()
    {
        if (MoveDir == 0) return;
        lastDir = playDir;
        playDir = MoveDir > 0 ? PlayerDir.Right : PlayerDir.Left;
       // virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = playDir == PlayerDir.Right ? 0.4f : 0.6f;
        if (lastDir != playDir)
        {
            transform.localScale = new Vector3(GetDirInt, transform.localScale.y, transform.localScale.z);
        }
    }

    IEnumerator IntroJump(Vector2 vel, Vector2 maxVel)
    {
        float dis = 0;
        float curJumpMin = JumpMin * (vel.y + JumpSpeed) / JumpSpeed;
        float curJumpMax = JumpMax * (vel.y + JumpSpeed) / JumpSpeed;
        float curJumpSpeed = JumpSpeed + vel.y;
        while(dis<=curJumpMin && Velocity.y < curJumpSpeed)
        {
            dis = transform.position.y - startJumpPos;
            if(vel.y<=0)
            {
                Velocity.y += 240 * Time.fixedDeltaTime;
            }
            yield return new WaitForFixedUpdate();
        }
        Velocity.y = curJumpSpeed;
        while(Input.GetKey(KeyCode.Space) && dis<curJumpMax)
        {
            dis = transform.position.y - startJumpPos;
            Velocity.y = curJumpSpeed;
            yield return new WaitForFixedUpdate();
        }

        while(Velocity.y>0)
        {
            if(dis>JumpMax)
            {
                Velocity.y -= 100 * Time.fixedDeltaTime;
            }
            else
            {
                Velocity.y -= 200 * Time.fixedDeltaTime;
            }
            yield return new WaitForFixedUpdate();
        }

        Velocity.y = 0;
        yield return 0.1f;
    }

    void CheckJump()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if(GetIsGrounded)
            {
                Velocity.y = JumpSpeed;
                startJumpPos = transform.position.y;
                StartCoroutine(IntroJump(Vector2.zero, Vector2.zero));
            }
        }
    }

    int GetDirInt { get { return playDir == PlayerDir.Right ? 1 : -1; } }
    bool GetIsGrounded { get { return this.GetComponentInChildren<CheckGround>().IsGrounded; } }
    private void FixedUpdate()
    {
        if (isDead) return;
        CheckVerticalMove();
        CheckJump();
        CheckHorizontalMove();
        CheckDir();
        //if(Platform!=null)
        //{
        //    bool jump = Velocity.y != 0;
        //    Velocity.y = 0;
        //    this.transform.position = Vector2.MoveTowards(this.transform.position, this.transform.position + Velocity * Time.fixedDeltaTime, MoveSpeed*Time.fixedDeltaTime*0.5f);
        //    if(jump)
        //    {
        //        rig.velocity = (new Vector2(0, 10));
        //    }
        //}
        rig.MovePosition(transform.position + Velocity * Time.fixedDeltaTime);
       // rig.velocity = Velocity;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "BIND")
        {
            couldClimb = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "BIND")
        {
            couldClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "BIND")
        {
            couldClimb = false;
            Velocity.y = 0;
            rig.gravityScale = 50;
        }
    }

    IEnumerator Death()
    {
        this.GetComponentInChildren<Animator>().SetBool("Die", true);
        yield return new WaitForSeconds(1.5f);
       GameManager.Instance.HandleReloadCurrentLevel();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag =="DAMAGE" || collision.transform.parent.tag == "DAMAGE")
        {
            if(!isDead)
            {
                print("DAMAGE");
                isDead = true;
                StartCoroutine(Death());
            }
        }
       
    }
    // Update is called once per frame
    void Update()
    {
      
    }
}
