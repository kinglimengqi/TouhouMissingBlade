using UnityEngine;
using System;

public class AiEnemy1 : MonoBehaviour {

    Transform Enemy1;
    GameObject Player;
    Animator AiEemy1;
    private  Rigidbody2D enemy;
    //GameObject child;
    

    float MoveSpeed = 1.0f;
    float Timer = 2;
    float LifePoint = 10;

    
    private void Start () {

        AiEemy1 = GetComponent<Animator>();
        Enemy1 = this.transform;
        Player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponent<Rigidbody2D>();
        //child = GameObject.Find("x");

    }



	//private void Update () {

       
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log(1);
            enemy.AddForce(new Vector2(0f, MoveSpeed * 500f));

        }
    }
}
