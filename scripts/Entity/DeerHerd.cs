using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DeerHerd : MonoBehaviour
{
    int playerMask = 1 << 12;
    Transform Player =null;
    public float timePassed =0f;
    public float timrange = 5f;

    public List<Deer> herdmembers = new List<Deer>();
    public List<Transform> targets = new List<Transform>();
    // Update is called once per frame
    void Update()
    {

        //timer
         if(timePassed < 0f){
            timePassed = timrange;
            if(Player = null){

            }
            createRandom(-5,5);
           
        }else{
            timePassed -= Time.deltaTime;
            //updateTime.Invoke($"{Mathf.FloorToInt(timePassed /60f)}:{Mathf.FloorToInt(timePassed %60f)}");
            
        }

        Vector3 tmp = new Vector3();
        foreach(Deer d in herdmembers){
            tmp += d.transform.position;

        }
        transform.position= tmp.normalized;

    }


    void detectTargets(Vector3 pos){
        List<Collider> player = Physics.OverlapSphere(pos,30f,playerMask).ToList();
        if(player.Count > 0){

            Vector3 direction =player[0].transform.position - pos;
            float distance = direction.magnitude; 

            if(distance > GameLoop.playerLoudnes *5){
                Alert(direction,distance);
            }
        }
    }
    void createRandom(int mx,int min){

        int i =0;
        foreach(Deer d in herdmembers){
            UnityEngine.Vector3 pos =  new Vector3(d.transform.position.x + Random.Range(min,mx),d.transform.position.y,d.transform.position.z + Random.Range(min,mx));
            targets[i].position = pos;
            d.target = targets[i];
            i++;
            detectTargets(d.transform.position);
        }
    }
    void Alert(Vector3 dist, float di){
        int id =0;
        foreach(Deer d in herdmembers){
            dist.Normalize();
            Vector3 pos =  new Vector3(d.transform.position.x + (dist.x *-10f),d.transform.position.y,d.transform.position.z  + (dist.x *-10f));
            targets[id].position = pos;
            targets[id].position = pos;
            d.target = targets[id];
            id++;
        }
    }
}
