using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Inventory : MonoBehaviour {

    private GameObject SlotContainer;
    public  GameObject prefabItem;

    [SerializeField] public ItemDataBaseList itemDataBase;

    public List<Item> ItemsInInventory = new List<Item>();

    //event delegates for consuming, gearing
    public delegate void ItemDelegate(Item item);
    public static event ItemDelegate ItemConsumed;


    public delegate void InventoryOpened();
    public static event InventoryOpened InventoryOpen;
    public static event InventoryOpened AllInventoriesClosed;

    public int positionNumberX;
    public int positionNumberY;
    public bool stackable;
    [SerializeField] public int height;
    [SerializeField] public int width;

    private void Start() {

        SlotContainer = transform.GetChild(0).gameObject;
        height = 4;
        width = 10;
        updateItemList();
        
    }


    public void sortItems() {

        int empty = -1;
        for (int i = 0; i< SlotContainer.transform.childCount; i++) {
            if (SlotContainer.transform.GetChild(i).childCount == 0 && empty == -1) 
                empty = i;
                else{
                    if(empty > -1) {
                        if (SlotContainer.transform.GetChild(i).childCount != 0) {
                            RectTransform rect = SlotContainer.transform.GetChild(i).GetChild(0).GetComponent<RectTransform>();
                            SlotContainer.transform.GetChild(i).GetChild(0).transform.SetParent(SlotContainer.transform.GetChild(empty).transform);
                            rect.localPosition = Vector3.zero;
                            i = empty + 1;
                            empty = i;
                    }
                }
            }
        }
    }

    private void Update() {

        updateItemIndex();
    }

    public void OnUpdateItemList() {

        updateItemList();
    }

    public void closeInventory()
    {
        this.gameObject.SetActive(false);
        //checkIfAllInventoryClosed();
    }

    public void openInventory()
    {
        this.gameObject.SetActive(true);
        if (InventoryOpen != null)
            InventoryOpen();
    }
    public void ConsumeItem(Item item)
    {
        if (ItemConsumed != null)
            ItemConsumed(item);
    }

    public void updateItemList() {
        ItemsInInventory.Clear();
        for (int i = 0; i <SlotContainer.transform.childCount; i++) {
            Transform trans = SlotContainer.transform.GetChild(i);
            if (trans.childCount != 0)
                ItemsInInventory.Add(trans.GetChild(0).GetComponent<ItemOnObject>().item);
        }
    }


    public bool checkIfItemAllreadyExist(int itemID, int itemNum)
    {
        updateItemList();
        int stack;
        for (int i = 0; i < ItemsInInventory.Count; i++)
        {
            if (ItemsInInventory[i].itemID == itemID)
            {
                stack = ItemsInInventory[i].itemNum + itemNum;
                if (stack <= ItemsInInventory[i].itemMaxNum)
                {
                    ItemsInInventory[i].itemNum = stack;
                    GameObject temp = getItemGameObject(ItemsInInventory[i]);
                    //if (temp != null && temp.GetComponent<ConsumeItem>().duplication != null)
                        //temp.GetComponent<ConsumeItem>().duplication.GetComponent<ItemOnObject>().item.itemNum = stack;
                    return true;
                }
            }
        }
        return false;
    }

    public GameObject getItemGameObject(Item item)
    {
        for (int k = 0; k < SlotContainer.transform.childCount; k++)
        {
            if (SlotContainer.transform.GetChild(k).childCount != 0)
            {
                GameObject itemGameObject = SlotContainer.transform.GetChild(k).GetChild(0).gameObject;
                Item itemObject = itemGameObject.GetComponent<ItemOnObject>().item;
                if (itemObject.Equals(item))
                {
                    return itemGameObject;
                }
            }
        }
        return null;
    }

    /*public void addItemToInventory(int id)
    {
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            if (SlotContainer.transform.GetChild(i).childCount == 0)
            {
                GameObject item = (GameObject)Instantiate(prefabItem);
                item.GetComponent<ItemOnObject>().item = itemDataBase.getItemByID(id);
                item.transform.SetParent(SlotContainer.transform.GetChild(i));
                item.GetComponent<RectTransform>().localPosition = Vector3.zero;
                item.transform.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<ItemOnObject>().item.itemIcon;
                item.GetComponent<ItemOnObject>().item.indexItemInList = ItemsInInventory.Count - 1;
                break;
            }
        }

        stackableSettings();
        updateItemList();

    }*/
    public GameObject addItemToInventory(int id, int value)
    {
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            if (SlotContainer.transform.GetChild(i).childCount == 0)
            {
                GameObject item = (GameObject)Instantiate(prefabItem);
                ItemOnObject itemOnObject = item.GetComponent<ItemOnObject>();
                itemOnObject.item = itemDataBase.getItemByID(id);
                if (itemOnObject.item.itemNum <= itemOnObject.item.itemMaxNum && value <= itemOnObject.item.itemMaxNum)
                    itemOnObject.item.itemNum = value;
                else
                    itemOnObject.item.itemNum = 1;
                item.transform.SetParent(SlotContainer.transform.GetChild(i));
                item.transform.GetChild(0).GetComponent<Image>().sprite = itemOnObject.item.itemIcon;
                itemOnObject.item.indexItemInList = ItemsInInventory.Count - 1;
                item.GetComponent<RectTransform>().localPosition = new Vector3(55f * 0.5f, 55f * 0.5f, 0);// position = half size of its parent / 2  
                item.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
                //if (inputManagerDatabase == null)
                //inputManagerDatabase = (InputManager)Resources.Load("InputManager");
                return item;
            }
        }

        stackableSettings();
        updateItemList();
        return null;

    }

    public void stackableSettings()
    {
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            if (SlotContainer.transform.GetChild(i).childCount > 0)
            {
                ItemOnObject item = SlotContainer.transform.GetChild(i).GetChild(0).GetComponent<ItemOnObject>();
                if (item.item.itemMaxNum > 1)
                {
                    RectTransform textRectTransform = SlotContainer.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<RectTransform>();
                    Text text = SlotContainer.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>();
                    text.text = "" + item.item.itemNum;
                    text.enabled = stackable;
                    textRectTransform.localPosition = new Vector3(positionNumberX, positionNumberY, 0);
                }
                else
                {
                    Text text = SlotContainer.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>();
                    text.enabled = false;
                }
            }
        }

    }

    public void deleteAllItems()
    {
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            if (SlotContainer.transform.GetChild(i).childCount != 0)
            {
                Destroy(SlotContainer.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
    }

    public void deleteItemFromInventory(Item item) {
    //待定


    }

    public List<Item> getItemList()
    {
        List<Item> theList = new List<Item>();
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            if (SlotContainer.transform.GetChild(i).childCount != 0)
                theList.Add(SlotContainer.transform.GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item);
        }
        return theList;
    }
    public void updateItemIndex() {
        
        for(int i = 0; i < ItemsInInventory.Count; i++) {

            ItemsInInventory[i].indexItemInList = i;
        }
    }
}
