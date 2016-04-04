using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tips : MonoBehaviour {

	public GameObject Mess;
    public Transform empty;



    void Start () {
	
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Instantiate(Mess,empty.position,empty.rotation);
            //GameObject.Find("Mes").SendMessage("on");

        }
    }

}
