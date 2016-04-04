using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour,IDragHandler,IPointerDownHandler,IEndDragHandler{

    private Inventory inventory;
    private Transform draggedItemBox;
    private GameObject oldSlot;
    private RectTransform rectTransform;
    private RectTransform rectTransformSlot;
    private CanvasGroup canvasGroup;
    private Vector2 pointerOffset;

    public delegate void ItemDelegate();
    public static event ItemDelegate updateInventoryList;

	void Start () {
	
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransformSlot = GameObject.FindGameObjectWithTag("DraggingItem").GetComponent<RectTransform>();
        inventory = transform.parent.parent.parent.GetComponent<Inventory>();
        draggedItemBox = GameObject.FindGameObjectWithTag("DraggingItem").transform;

	}
	
    public void OnDrag (PointerEventData data) {

        if (rectTransform == null) 
            return;
        if (data.button == PointerEventData.InputButton.Left) {
            rectTransform.SetAsLastSibling();//set the ui as the last layer, so it will be on the top of all
            transform.SetParent(draggedItemBox);
            
            canvasGroup.blocksRaycasts = false;
            Vector2 localPointerPosition;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransformSlot, Input.mousePosition, data.pressEventCamera, out localPointerPosition)) {
                //bool Returns true if the plane of the RectTransform is hit
                rectTransform.localPosition = localPointerPosition;

            }
            //draggedItemBox.localPosition = localPointerPosition - pointerOffset;
        }
        inventory.OnUpdateItemList();
    }

    public void OnPointerDown (PointerEventData data) {

        if(data.button == PointerEventData.InputButton.Left) {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransformSlot, data.position, data.pressEventCamera, out pointerOffset);
            
                //bool Returns true if the plane of the RectTransform is hit
                oldSlot = transform.parent.gameObject;

           
        }
        if (updateInventoryList != null)
            updateInventoryList();
    }

    public void OnEndDrag (PointerEventData data) {

        if(data.button == PointerEventData.InputButton.Left) {

            canvasGroup.blocksRaycasts = true;
            Transform newSlot = null;
            if (data.pointerEnter != null)
                newSlot = data.pointerEnter.transform;

            if (newSlot != null)
            {
                //getting the items from the slots, GameObjects and RectTransform
                GameObject firstItemGameObject = this.gameObject;
                GameObject secondItemGameObject = newSlot.parent.gameObject;
                RectTransform firstItemRectTransform = this.gameObject.GetComponent<RectTransform>();
                RectTransform secondItemRectTransform = newSlot.parent.GetComponent<RectTransform>();
                Item firstItem = rectTransform.GetComponent<ItemOnObject>().item;
                Item secondItem = new Item();
                if (newSlot.parent.GetComponent<ItemOnObject>() != null)
                secondItem = newSlot.parent.GetComponent<ItemOnObject>().item;

                //get some informations about the two items
                bool sameItem = firstItem.itemName == secondItem.itemName;
                bool sameItemRerferenced = firstItem.Equals(secondItem);
                 bool secondItemStack = false;
                bool firstItemStack = false;
                if (sameItem)
                {
                    firstItemStack = firstItem.itemNum < firstItem.itemMaxNum;
                    secondItemStack = secondItem.itemNum < secondItem.itemMaxNum;
                }

            GameObject Inventory = secondItemRectTransform.parent.gameObject;
            if (Inventory.tag == "Slot")
                Inventory = secondItemRectTransform.parent.parent.parent.gameObject;

            if (Inventory.tag.Equals("Slot"))
                Inventory = Inventory.transform.parent.parent.gameObject;

         
            //dragging into a Hotbar            
            if (Inventory.GetComponent<Hotbar>() != null)
                {
                    int newSlotChildCount = newSlot.transform.parent.childCount;
                    bool isOnSlot = newSlot.transform.parent.GetChild(0).tag == "ItemIcon";
                    //dragging on a slot where allready is an item on
                    if (newSlotChildCount != 0 && isOnSlot)
                    {
                        //check if the items fits into the other item
                        bool fitsIntoStack = false;
                        if (sameItem)
                            fitsIntoStack = (firstItem.itemNum + secondItem.itemNum) <= firstItem.itemMaxNum;
                        //if the item is stackable checking if the firstitemstack and seconditemstack is not full and check if they are the same items

                        if (inventory.stackable && sameItem && firstItemStack && secondItemStack)
                        {
                            //if the item does not fit into the other item
                            if (fitsIntoStack && !sameItemRerferenced)
                            {
                                secondItem.itemNum = firstItem.itemNum + secondItem.itemNum;
                                secondItemGameObject.transform.SetParent(newSlot.parent.parent);
                                Destroy(firstItemGameObject);
                                secondItemRectTransform.localPosition = new Vector3(25f, 25f, 0);
                                /*if (secondItemGameObject.GetComponent<ConsumeItem>().duplication != null)
                                {
                                    GameObject dup = secondItemGameObject.GetComponent<ConsumeItem>().duplication;
                                    dup.GetComponent<ItemOnObject>().item.itemNum = secondItem.itemValue;
                                    Inventory.GetComponent<Inventory>().stackableSettings();
                                    dup.transform.parent.parent.parent.GetComponent<Inventory>().updateItemList();
                                }*/
                            }

                            else
                            {
                                //creates the rest of the item
                                int rest = (firstItem.itemNum + secondItem.itemNum) % firstItem.itemMaxNum;

                                //fill up the other stack and adds the rest to the other stack 
                                if (!fitsIntoStack && rest > 0)
                                {
                                    firstItem.itemNum = firstItem.itemMaxNum;
                                    secondItem.itemNum = rest;

                                    firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                    secondItemGameObject.transform.SetParent(oldSlot.transform);

                                    firstItemRectTransform.localPosition = new Vector3(25f, 25f, 0);
                                    secondItemRectTransform.localPosition = new Vector3(25f, 25f, 0);

                                    //secondItemGameObject.GetComponent<SplitItem>().inv.stackableSettings();

                                }
                            }

                        }
                        //if does not fit
                        else
                        {
                            //creates the rest of the item
                            int rest = 0;
                            if (sameItem)
                                rest = (firstItem.itemNum + secondItem.itemNum) % firstItem.itemMaxNum;

                            //bool fromEquip = oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null;

                            //fill up the other stack and adds the rest to the other stack 
                            if (!fitsIntoStack && rest > 0)
                            {
                                secondItem.itemNum = firstItem.itemMaxNum;
                                firstItem.itemNum = rest;

                                //createDuplication(this.gameObject);

                                firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                secondItemGameObject.transform.SetParent(oldSlot.transform);

                                firstItemRectTransform.localPosition = new Vector3(25f, 25f, 0);
                                secondItemRectTransform.localPosition = new Vector3(25f, 25f, 0);

                            }
                            //if they are different items or the stack is full, they get swapped
                            else if (!fitsIntoStack && rest == 0)
                            {
                                /*if (!fromEquip)
                                {
                                    firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                    secondItemGameObject.transform.SetParent(oldSlot.transform);
                                    secondItemRectTransform.localPosition = Vector3.zero;
                                    firstItemRectTransform.localPosition = Vector3.zero;

                                    if (oldSlot.transform.parent.parent.gameObject.Equals(GameObject.FindGameObjectWithTag("MainInventory")))
                                    {
                                        Destroy(secondItemGameObject.GetComponent<ConsumeItem>().duplication);
                                        createDuplication(firstItemGameObject);
                                    }
                                    else
                                    {
                                        //createDuplication(firstItemGameObject);
                                    }
                                }
                                else
                                {
                                    firstItemGameObject.transform.SetParent(oldSlot.transform);
                                    firstItemRectTransform.localPosition = Vector3.zero;
                                }*/
                            }

                        }
                    }
                    //empty slot
                    else
                    {
                        if (newSlot.tag != "Slot" && newSlot.tag != "ItemIcon")
                        {
                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                            firstItemRectTransform.localPosition = new Vector3(25f, 25f, 0);
                        }
                        else
                        {
                            firstItemGameObject.transform.SetParent(newSlot.transform);
                            firstItemRectTransform.localPosition = new Vector3(25f, 25f, 0);

                            //if (newSlot.transform.parent.parent.GetComponent<EquipmentSystem>() == null && oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null)
                                //oldSlot.transform.parent.parent.GetComponent<Inventory>().UnEquipItem1(firstItem);
                            //createDuplication(firstItemGameObject);
                        }
                    }

                }

            }
            }
        inventory.OnUpdateItemList();
    }

}
