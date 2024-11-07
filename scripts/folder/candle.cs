using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candle : MonoBehaviour
{
    public GameObject candlewick;
    public GameObject candleFlame;
    
    [Range(0f,3f)]
    public float power=1f;
    // Start is called before the first frame update
    void Start()
    {
        if(candlewick != null && candleFlame != null){
            candlewick.SetActive(false);
            candleFlame.SetActive(false);
            candlewick.transform.localScale = new Vector3(1f,0f,1f);
            
        }
    }

    public void Ligth(){
        if(candlewick.activeSelf){
            candlewick.SetActive(false);
            candlewick.transform.localScale = new Vector3(1f,0f,1f);
            candleFlame.SetActive(false);
        }else{
            candlewick.SetActive(true);
            candleFlame.SetActive(true);
            candlewick.transform.localScale = new Vector3(1f,power,1f);
        }
    }
}
