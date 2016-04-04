using UnityEngine;
using System.Collections;

public class LostPoint : MonoBehaviour {

    public string Key;
    public float Chance;
    private Transform NextPlace;
    private Transform EndPlace;
    private GameObject Youmu;
    public GameObject ReturnPoint;



    void Start()
    {

        Youmu = GameObject.FindGameObjectWithTag("Player");
        NextPlace = this.transform.GetChild(0).transform;
        EndPlace = this.transform.parent.GetChild(3).transform;

    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            float val = Random.value;
            Debug.Log(val);

            if (Input.GetKeyDown(Key))
            {
                if(val <= Chance && val >= 0) { 
                    Youmu.transform.position = NextPlace.position;
                    ReturnPoint.SetActive(true);
                }
                if (val <=1 && val > Chance)
                    Youmu.transform.position = EndPlace.position;
            }

        }

    }
}
