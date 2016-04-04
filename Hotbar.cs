using UnityEngine;
using System.Collections;

public class Hotbar : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {

        if(Input.GetButton("HotbarChange1")) {

        }
	    else if(Input.GetButtonDown("HotbarChange2")) {

        }
        else if(Input.GetButtonDown("Hotbar1")) {
            if(transform.GetChild(1).GetChild(1).childCount != 0)
                transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<ConsumeItem>().consumeIt();
        }
        else if(Input.GetButtonDown("Hotbar2")) {
            if (transform.GetChild(1).GetChild(2).childCount != 0)
                transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<ConsumeItem>().consumeIt();
        }
	}

    public void HotBarChange(int i) {

    }
}
