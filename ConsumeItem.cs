using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ConsumeItem : MonoBehaviour, IPointerDownHandler
{
    public Item item;
    //public static GameObject inventory;

    private void Start() {
        item = GetComponent<ItemOnObject>().item;
        //if(GameObject.FindGameObjectWithTag("Inventory") != null)
            //inventory = GameObject.FindGameObjectWithTag("Inventory");
    }

    public void OnPointerDown(PointerEventData data) {

        Inventory myInventory = transform.parent.parent.parent.GetComponent<Inventory>();
        if (data.button == PointerEventData.InputButton.Right) {
            myInventory.ConsumeItem(item);
            item.itemNum--;

            if(item.itemNum <= 0) {
                myInventory.deleteItemFromInventory(item);
                Destroy(this.gameObject);
            }
        }
    }
    
    public void consumeIt() {

        Inventory inventory = transform.parent.parent.parent.GetComponent<Inventory>();

        inventory.ConsumeItem(item);
        item.itemNum--;

        if(item.itemNum <= 0) {
            Destroy(this.gameObject);
        }
    }
}
