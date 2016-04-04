using UnityEngine;
using System.Collections;
using UnityEditor;


public class CreateItemDatabase
{
    public static ItemDataBaseList asset;                                                  //The List of all Items

    [MenuItem("Assets/Create/ItemDataBase")]
    public static ItemDataBaseList createItemDatabase()                                    //creates a new ItemDatabase(new instance)
    {
        asset = ScriptableObject.CreateInstance<ItemDataBaseList>();                       //of the ScriptableObject InventoryItemList

        AssetDatabase.CreateAsset(asset, "Assets/Resources/ItemDatabase.asset");            //in the Folder AssetsItemDatabase.asset
        AssetDatabase.SaveAssets();                                                         //and than saves it there
        asset.itemList.Add(new Item());
        return asset;
    }


}
