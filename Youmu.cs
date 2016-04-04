using System;
using UnityEngine;


public class Youmu : YoumuState {

    [SerializeField] private float MaxSpeed = 10f; //max speed
    [SerializeField] private float JumpForce = 200f; //jump force
    [Range(0, 1)][SerializeField] private float CrouchingSpeed = .3f; //crouching speed
    [SerializeField] private bool AirContorl = false; // wheather or not youmu can steer whlie jumping 能被风驱动？ not well understanding
    [SerializeField] private LayerMask WhatIsGround; // determine the ground

    private Transform GroundCheck; //to check youmu is grounded
    private Transform CeilingCheck;//to check youmu's head is gonging into the roof
    const float GroundRadius = .2f;//radius to overlap the circle if grounded
    const float CeilingRadius = .05f;//radius to check youmu's head and the roof or can stand up
    private  bool Grounded;
    private bool DoubleJump;//double jump
    private Rigidbody2D Rigidbody2DYoumu;
    private Animator Anim;
    private bool FacingRight = true;//face right as default

    public static readonly string IdleState = "BaseAction.Idle";
    public static readonly string RunState = "BaseAction.Run";
    public static readonly string WalkState = "BaseAction.Walk";
    public static readonly string AttackState = "BaseAction.Attack";
    public static readonly string AttackZState = "Attack.AttackZ";
    public static readonly string AttackXState = "Attack.AttackX";
    public static readonly string AttackCState = "Attack.AttackC";
    public static readonly string AttackVState = "Attack.AttackV";
    private string StateName;

    public int Combotimes = 0;
    public GameObject BladeAttack;
    public Transform AttackPoint;

    private void Awake(){
        GroundCheck = transform.Find("GroundCheck");
        CeilingCheck = transform.Find("CeilingCheck");
        Anim= GetComponent<Animator>();
        Rigidbody2DYoumu = GetComponent<Rigidbody2D>();
    }

	private void FixedUpdate () {
        Grounded = false;
        //set grounded false at first and go check if youmu stands anything considered as ground
	    Collider2D[] conlliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundRadius, WhatIsGround);
        //above is an array which catches all the conlliders in the circle definded with position, radius and mask
        for (int i = 0; i < conlliders.Length; i++){
            if (conlliders[i].gameObject != gameObject) {
                Grounded = true;
            }

        }
        Anim.SetBool("Ground",Grounded);//set the transfer condition in the animator
        Anim.SetFloat("vSpeed",Rigidbody2DYoumu.velocity.y);//vertical speed in the animator condition no use for the moment
    }

    public void Move (float move, bool jump, bool crouch) {
        // if youmu is crouching, check wheather she can stand up or not(check wheather there is anything above)
        if(!crouch && Anim.GetBool("Crouch")) {
            if(Physics2D.OverlapCircle(CeilingCheck.position,CeilingRadius,WhatIsGround)) {
                crouch=true;
            }
        }
        Anim.SetBool("Crouch",crouch);

        //move youmu attention!!! this value "move" is the horizontal axis on the keyboard value [-1,1]
        if(GroundCheck || AirContorl) {
            //set the movement of youmu, in() is a simple test if crouch= true move=move*CrouchingSpeed else =move
            move = (crouch? move*CrouchingSpeed : move);
            //set the speed condition in the animator 
            Anim.SetFloat("Speed", Mathf.Abs(move));
            //set the force on youmu to make her move
            Rigidbody2DYoumu.velocity = new Vector2(MaxSpeed*move,Rigidbody2DYoumu.velocity.y);
            //Filp
            if(move > 0 && !FacingRight) {
                Flip();//go other side
            }
            else if(move <0 && FacingRight) {
                Flip();
            }
        }
        //jump
        if(Grounded && Anim.GetBool("Ground") && jump) {
            //switch all the codition into false and add a jump force to youmu
            Grounded = false;
            DoubleJump = true;
            Anim.SetBool("Ground", false);
            Rigidbody2DYoumu.AddForce(new Vector2(0f,JumpForce));
        }
        //Double jump
        else if(!Grounded && !Anim.GetBool("Ground") && jump && DoubleJump) {
            Anim.SetBool("DoubleJump", true);
            Rigidbody2DYoumu.AddForce(new Vector2(0f, JumpForce));
            DoubleJump = false;
        }
        if (Grounded) Anim.SetBool("DoubleJump", false);//reset double jump
    }

    private void Flip() {
        //switch the way of youmu 
        FacingRight = !FacingRight;
        //multiply youmu's x local scale by -1
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }

    //attack part
    public void Attack(string key) {

        AnimatorStateInfo stateInfo = Anim.GetCurrentAnimatorStateInfo(0);
        //if (!stateInfo.IsName(IdleState))Anim.SetBool("AttackCmd", false);

        if (stateInfo.IsName(IdleState) || stateInfo.IsName(RunState) || stateInfo.IsName(WalkState) || stateInfo.IsName(AttackZState) 
            || stateInfo.IsName(AttackCState) || stateInfo.IsName(AttackXState) || stateInfo.IsName(AttackVState))
        {
            //if(!Anim.GetBool("AttackCmd")) Anim.SetBool("AttackCmd", true);
            //AttackAction(); //realize attack action1
            if (key == "V") Anim.SetBool("AttackV", true);
            if (key == "C") Anim.SetBool("AttackC", true);
            if (key == "X") Anim.SetBool("AttackX", true);
            if (key == "Z") Anim.SetBool("AttackZ", true);
            Debug.Log(Combotimes);
        }

    }

    public void Defence()
    {
        //Anim.SetBool("Defence", true);
    }

    public void AttackAction() {

        GameObject NewBlade = Instantiate(BladeAttack, AttackPoint.position, AttackPoint.rotation) as GameObject;
        
    }
}
