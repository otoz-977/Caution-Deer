using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InventoryPlayer:MonoBehaviour
{
    [SerializeField]
    public List<gear> gearDatabase = new List<gear>();
    Dictionary<string,gear> displayDict = new Dictionary<string,gear>();
   
    public List<gear> playerInventory= new List<gear>();

    private void Start() {
        //gearDatabase = Resources.LoadAll<gear>(gear);
        foreach (gear item in gearDatabase)
        {
            if(!displayDict.ContainsKey(item.id)){
                displayDict.Add(item.id,item);
            }
           
        }
    }

    public void addItem(Item item){
         playerInventory.Add(displayDict[item.itemid]);
        Debug.Log($"added{item.itemName}");
    }
    public void removeItem(string id){
        //playerInventory.Remove(gearDatabase[id]);
    }

   

   
}



[CreateAssetMenu(fileName = "InventoryPlayer", menuName = "InventoryPlayer", order = 0)]
public class gear: ScriptableObject {
   

    public string id;
    public string gearName;
    public Sprite gearSprite;
    public gearType category;
}
public enum gearType
{
    weapon,
    tool,
    clothes,
    food
}