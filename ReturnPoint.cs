using UnityEngine;
using System.Collections;

public class ReturnPoint : MonoBehaviour {


    public GameObject Mess;
    public Transform empty;



    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Instantiate(Mess, empty.position, empty.rotation);
            //GameObject.Find("Mes").SendMessage("on");

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            gameObject.SetActive(false);
            //GameObject.Find("Mes").SendMessage("on");

        }
    }
}
