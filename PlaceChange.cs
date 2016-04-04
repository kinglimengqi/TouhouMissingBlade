using UnityEngine;
using System.Collections;

public class PlaceChange : MonoBehaviour {

    public string Key;
    private Transform NextPlace;
    private GameObject Youmu;



	void Start () {
	
        Youmu = GameObject.FindGameObjectWithTag("Player");
        NextPlace = this.transform.GetChild(0).transform;

	}

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D other) {

        if (other.tag == "Player") {

            if(Input.GetKeyDown(Key)) {

                Youmu.transform.position = NextPlace.position;
            }

         }

    }

}
