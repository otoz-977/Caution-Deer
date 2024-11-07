 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
    public Transform target;
    public float followSpeed;
    public float threshold;
    float distanceToTarget;
    Vector3 directionToTarget;
   
    // Update is called once per frame
  private void LateUpdate() {
    directionToTarget = target.position - transform.position;
    distanceToTarget = directionToTarget.magnitude;

    if(distanceToTarget >= threshold){
        //move towards target
        transform.position += directionToTarget.normalized *followSpeed*Time.deltaTime;
        //transform.position = Vector3.Slerp(transform.position, directionToTarget,2.0f);
    }else{
        //stopp moving
    }
  }
}
  