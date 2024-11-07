using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;


public class worldInteraction : MonoBehaviour
{
    public InventoryPlayer inventory;
    [SerializeField]
    TMP_Text itemTextField;
    [SerializeField]
    GameObject itemTextUi;
    [SerializeField]
    List<ICollectible>itemsInReach = new List<ICollectible>();
    ICollectible closestInteractible = null;
    private void Update() {
        //get action from E

        if(Input.GetKeyDown(KeyCode.E)){
             itemsInReach[itemsInReach.Count-1].pickUp();
            //Leftover from when collectible items where being implemented
            /*
            if(itemsInReach.Count >0){
                if(itemsInReach[itemsInReach.Count-1]!= null){
                     //if (itemsInReach[itemsInReach.Count-1]("item")){}
                    itemsInReach[itemsInReach.Count-1].pickUp());
                    itemsInReach.RemoveAt(itemsInReach.Count-1);
                    return;
                }
                

            }
            Item tmp =closestInteractible.pickUp();
            */
        }
    }
    private void OnEnable() {
        Item.onItemPassedOver += EnableItemUI;//subscribes to item action
       
    }
    private void OnDisable() {
        Item.onItemPassedOver -= EnableItemUI;//unsubscribes to item action
        
    }
    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("item")){
            ICollectible icollectible = other.GetComponent<ICollectible>();
            if(icollectible != null){
                icollectible.Collect();
                //Debug.Log("ffff");
                itemsInReach.Add(icollectible);
            }
        } 
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("item")){
            ICollectible icollectible = other.GetComponent<ICollectible>();
            itemsInReach.Remove(icollectible);
            EnableItemUI("/",false);
        }
    }
    void EnableItemUI(string n,bool t){
        if(t){
            //displays the text for item interaction panel
            itemTextUi.SetActive(t);
            itemTextField.text = $"press E to {n}";
            return;
        }
        //hides the item interaction panel and clears text
        itemTextUi.SetActive(t);
        itemTextField.text = $"---";
    }


}
