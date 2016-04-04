using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YoumuState : MonoBehaviour {

    [SerializeField] public float maxHealth = 1000;
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxShapness = 100; //blade shapness/刀身锋利度
    [SerializeField] public float currentDamageBlade;
    [SerializeField] public float maxHiltComfort= 100; //hilt comfortable degree/刀柄舒适度
    [SerializeField] public float currentDamageHilt;
    [SerializeField] public GameObject inventory;
    //private Inventory mainInventory;

    public void OnEnable() {

        Inventory.ItemConsumed += OnConmuseItem;
    }

    public void OnDisable() {

        Inventory.ItemConsumed -= OnConmuseItem;
    }

    private void Start() {
        //if (inventory != null) mainInventory = inventory.GetComponent<Inventory>();
    }

    public void OnConmuseItem(Item item) {// use an item 
        
        if(item.itemType.ToString() == "MaintainceHilt") {// to maintaince the damage of the hilt/刀柄
            if ((currentDamageHilt + item.itemAttributeValue) > maxHiltComfort)
                currentDamageHilt = maxHiltComfort;
            else
                currentDamageHilt += item.itemAttributeValue;
        }
        else if (item.itemType.ToString() == "MaintainceBlade"){//to maintaince the damage of the blade
            if ((currentDamageBlade + item.itemAttributeValue) > maxShapness)
                currentDamageBlade = maxShapness;
            else
                currentDamageBlade += item.itemAttributeValue;
        }
        else if(item.itemType.ToString() == "Spell") {// to sumerise helper


        }
        else if (item.itemType.ToString() == "Health")// medical treatment
        {
            if((currentHealth + item.itemAttributeValue) > maxHealth)
                currentHealth = maxHealth;
            else
                currentHealth += item.itemAttributeValue;
        }
    }


    private void Damage(float damage) {
        /*if (Health > damage ) {
            Health -= damage;
        }
        else {
            YoumuDie();
        }*/
    }

    private void YoumuDie() {

        Destroy (gameObject);

    }

}
