using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class notDeer : MonoBehaviour
{
    int playerMask = 1 << 12;
    public Transform interestpoint;
    public static nDeerStates currentstate = nDeerStates.idle;

    float timePassed;
    public bool hasTarget =false, isShrine = false;
    public static float  DetectionRange;
    public float range;
    public Deer monster;
    


    public Transform[] shrinelocations;

    public Collider[] player;
    void Update(){
        timePassed -= Time.deltaTime;
        GameLoop.enemyPosition = this.transform.position;
        switch((int)GameLoop.gameIntensity){
            case 0:
            DetectionRange = 25f;
            break;
            case 1:
            DetectionRange = 50f;
            break;
            case 2:
            DetectionRange = 75f;
            break;
            case 3:
            DetectionRange = 100f;
            break;
            case 4:
            DetectionRange = 25f;
            break;
        }
        player = Physics.OverlapSphere(monster.transform.position, DetectionRange, playerMask);
        if(player.Count() >0){
            //playerDetected and will check is is in view
            Vector3 direction =player[0].transform.position - monster.transform.position;
            float distance = direction.magnitude; 

            if(distance > GameLoop.playerLoudnes *5){
                hasTarget =true;
                //Alert(direction,distance);
                interestpoint.position = GameLoop.playerPosition;
                 //RaycastHit hit = Physics.Raycast(transform.position, direction, DetectionRange,playerMask);
            }

        }else{
            if(isShrine){

                idle();
            }
        }
    }

    void Start(){
        timePassed = 15f;
    }
    //randomly travel between points
    public void idle(){

        if(timePassed<0){
            timePassed =15f;
            interestpoint.position = shrinelocations[Random.Range(0,shrinelocations.Count() -1)].position;
        }
    }

    public void hearSomething(Vector3 t){
        interestpoint.position =t;
    }
}
public enum nDeerStates{
    idle,
    investigate,
    seeking,
    agro
}
