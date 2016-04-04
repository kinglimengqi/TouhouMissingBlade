using UnityEngine;
using System;
//using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Youmu))]
public class YoumuControl : MonoBehaviour {

	private Youmu Myyoumu;//get youmu component
    private bool Jump;
    private Animator MyyoumuAnim;
    private YoumuState MyyoumuState;
    public GameObject inventory;
    private Inventory mainInventory;

    private void Awake() {

        Myyoumu = GetComponent<Youmu>();
        MyyoumuAnim = GetComponent<Animator>();
        MyyoumuState = GetComponent<YoumuState>();
    }

    private void Start() {
        if (inventory != null) mainInventory = inventory.GetComponent<Inventory>();
    }

	// get jump botton down per frame so jump motion can't be missed
	private void Update () {
        
        if(!Jump) Jump = Input.GetButtonDown("Jump");



    }

    private void FixedUpdate() {

        string AttackKey;
        bool Crouch = Input.GetKey(KeyCode.DownArrow);


        //get the axis from the asset loaded on the third line, in unity down menu edit->projet settings
        float Haxis = Input.GetAxis("Horizontal");// value[-1,1]

        // pass all the key code to youmu
            Myyoumu.Move(Haxis, Jump, Crouch);
            Jump = false;//reset jump

        //attack motion
        AnimatorStateInfo stateInfo = MyyoumuAnim.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName(Youmu.IdleState)) MyyoumuAnim.SetBool("AttackCmd", false);

        /*if (stateInfo.IsName(Youmu.Attack1State) && (stateInfo.normalizedTime > 0.6f) && (Myyoumu.Combotimes == 2))
        {

            MyyoumuAnim.SetBool("AttackCmd", true);
            //realize attack action1
        }

        if (stateInfo.IsName(Youmu.Attack2State) && (stateInfo.normalizedTime > 0.8f) && (Myyoumu.Combotimes == 3))
        {

            MyyoumuAnim.SetBool("AttackCmd", true);
            //realize attack action1
        }*/

        if (Input.GetKeyUp(KeyCode.V))
        {
            AttackKey = "V";
            if(!MyyoumuAnim.GetBool("AttackCmd")) MyyoumuAnim.SetBool("AttackCmd", true);
            MyyoumuAnim.SetBool("AttackZ", false);
            MyyoumuAnim.SetBool("AttackX", false);
            MyyoumuAnim.SetBool("AttackC", false);
            Myyoumu.Combotimes++;
            Myyoumu.Attack(AttackKey);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {

            AttackKey = "C";
            if (!MyyoumuAnim.GetBool("AttackCmd")) MyyoumuAnim.SetBool("AttackCmd", true);
            MyyoumuAnim.SetBool("AttackZ", false);
            MyyoumuAnim.SetBool("AttackV", false);
            MyyoumuAnim.SetBool("AttackX", false);
            Myyoumu.Combotimes++;
            Myyoumu.Attack(AttackKey);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {

            AttackKey = "X";
            if (!MyyoumuAnim.GetBool("AttackCmd")) MyyoumuAnim.SetBool("AttackCmd", true);
            MyyoumuAnim.SetBool("AttackZ", false);
            MyyoumuAnim.SetBool("AttackV", false);
            MyyoumuAnim.SetBool("AttackC", false);
            Myyoumu.Combotimes++;
            Myyoumu.Attack(AttackKey);
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {

            AttackKey = "Z";
            if (!MyyoumuAnim.GetBool("AttackCmd")) MyyoumuAnim.SetBool("AttackCmd", true);
            MyyoumuAnim.SetBool("AttackX", false);
            MyyoumuAnim.SetBool("AttackV", false);
            MyyoumuAnim.SetBool("AttackC", false);
            Myyoumu.Combotimes++;
            Myyoumu.Attack(AttackKey);
        }





        if (Input.GetKeyDown(KeyCode.Q))
        {
            MyyoumuAnim.SetBool("Defence", true);
            Myyoumu.Defence();// not realized, code is in Youmu
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            MyyoumuAnim.SetBool("Defence", false);
        }

        if (Input.GetButtonDown("Inventory")) {//control inventory maybe unuseless in future
            if(!inventory.activeSelf) {
                mainInventory.openInventory();
            }
            else{
                mainInventory.closeInventory();
            }
        }

    }

    private void LateUpdate() {


        
    }
}
