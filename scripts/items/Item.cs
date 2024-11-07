using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;
public interface ICollectible{
    public void Collect();
    public void pickUp();
}
public class Item : MonoBehaviour, ICollectible
{
   
    public string itemName;
    public string itemid;
    public static event Action<string,bool> onItemPassedOver;
    public UnityEvent<int> interacted;
    public void Collect()
    {       
        onItemPassedOver?.Invoke(itemName,true);
        
    }
    public void pickUp()
    {
        //will display item being picked up
        //will add item id to character inventory
        Debug.Log("picked up item");
        onItemPassedOver?.Invoke(itemName,false);
        //Destroy(gameObject,0.15f);
        //return this; 
        interacted.Invoke(0);
    }
}
