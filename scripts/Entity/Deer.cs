using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Deer : MonoBehaviour
{
	 int hazardMask = 1 << 11;
	 int playerMask = 1 << 11;
	 [SerializeField,Range(1f,20f)]
	 float targetRadius;
	[SerializeField,Range(1f,20f)]
	
	 public float agentsize =0.6f;
    public Transform target;
	public CharacterController cnc;
    public float speed;
	public Vector3 result = Vector3.zero;
	public float[] interest = new float[8];
	public float[] Dangers = new float[8];

	public List<Collider> hazards = new List<Collider>();
    private void Update() {
        
		//detectTargets();
		detectObstacles();
		SeekTarget();
		avoidObstacles();
		getDirection();
		cnc.Move(result * speed * Time.deltaTime);
		transform.LookAt(target);
	}

	void detectTargets(){
		
	}
	void detectObstacles(){
		hazards = Physics.OverlapSphere(transform.position,targetRadius,hazardMask).ToList();
	}

    //main targeting loop
	void SeekTarget(){
		interest = new float[8];
		Dangers = new float[8];
		if(target != null){
		// interest
			Vector3 directionToTarget = target.position- transform.position;//get direction to target	
			
			
			for(int i =0; i <8; i++){
				float result =Vector3.Dot(directionToTarget.normalized,eigthDirections.directions[i]);
				if(result > 0){
					float value = result;

					if(value > interest[i]){
						interest[i] = value;
					}
				}
			}
		}
	}
	void avoidObstacles(){
		//hazards= Physics.OverlapSphere(transform.position,targetRadius,layerMask);
		
		foreach(Collider tmp in hazards){
			//gets direction to closest point of each obstacle
			Vector3 directionToObstacle = tmp.ClosestPoint(transform.position) - transform.position;
			float distanceToObstacle = directionToObstacle.magnitude;

			//calculate weigth based on distance tro obstacle
			float weight = distanceToObstacle <= agentsize?1:(targetRadius	-distanceToObstacle)/targetRadius;

			Vector3 directionToObnstacleNormalized = directionToObstacle.normalized;
			for (int i = 0; i < 8; i++)
			{
				float result = Vector3.Dot(directionToObnstacleNormalized, eigthDirections.directions[i]);
				float valuetoInput = result *weight;
				if(valuetoInput > Dangers[i]){
					Dangers[i] = valuetoInput;
				}
			}	
		}
	}
    //return best dirtectioon nbased on values assighned to each direction
    void getDirection(){
        //calculate best direction to move
		//combine both approaches
		for(int i =0; i < 8; i++){
			interest[i] = Mathf.Clamp01(interest[i] - Dangers[i]);
		}

		Vector3 outputDirection = Vector3.zero;
		for(int i =0; i< 8;i++){
			outputDirection += eigthDirections.directions[i] * interest[i];
		}

		outputDirection.Normalize();
		result = outputDirection;
    }

	void OnDrawGizmos(){
		Gizmos.DrawRay(this.transform.position, result * 2f);
		for(int i =0; i< 8;i++){
			Gizmos.color = Color.green;
			Gizmos.DrawRay(this.transform.position,eigthDirections.directions[i].normalized);
			//Handles.Label(transform.position + eigthDirections.directions[i].normalized, $"{interest[i]}");
		}
		
		foreach(Collider tmp in hazards){
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(tmp.transform.position,1f);
		}
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position,targetRadius);

	}
}
//Deer state machine:

//idle-> running

public  class eigthDirections{
    public static Vector3[] directions= 
	{
        new Vector3(0f,0f,1f),//front
        new Vector3(1f,0f,1f),//fR
        new Vector3(1f,0f,0f),//R
        new Vector3(1f,0f,-1f),//BR
        new Vector3(0f,0f,-1f),//B
        new Vector3(-1f,0f,-1f),//bL
        new Vector3(-1f,0f,0f),//L
        new Vector3(-1f,0f,1f)//FL
    };
    
}