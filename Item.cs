using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Item  {

    public string itemName;//物品名称
    public int itemID;//物品ID
    public string itemNameCN;//物品中文名
    public string itemDesc;//物品说明
    public Sprite itemIcon;//物品标志
    public int itemNum;//物品数量
    public int itemMaxNum;//物品数量上限
    public ItemType itemType;//物品属性
    public int itemPrice;//物品价格
    //public string itemAttribute;//物品属性 可用于定义一种物品增加多种属性 例如 解毒&回复生命
    public int itemAttributeValue;//物品属性值

    public int indexItemInList = 999;//物品索引

    public enum ItemType {

        //刀柄保养
        MaintainceHilt,
        //刀身保养
        MaintainceBlade,
        //符咒
        Spell,
        //药剂 这个类别估计以后要分些其他类
        Health,

    }

    //空构造函数

    public Item () {}

    //构造函数
    public Item (string name, int id, string nameCN, string desc, Sprite icon, int num, int maxnum, ItemType type, int price, int attributeValue) {
        
        itemName = name;
        itemID = id;
        itemNameCN = nameCN;
        itemDesc = desc;
        itemIcon = icon;
        itemNum = num;
        itemMaxNum = maxnum;
        itemType = type;
        itemAttributeValue = attributeValue;
        itemPrice = price;     

    }

    //深拷贝函数
    public Item Clone() {

        return (Item)this.MemberwiseClone();
    }
}
