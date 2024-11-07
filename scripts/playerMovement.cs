using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class playerMovement : MonoBehaviour
{
    [Header("trip status")]
    [Range(1f,20f)]
    public float stunTime;
    float timePassed;
    public Animator anim;
    public static bool isTripped =false;
    public static bool isPlaying = true;
    public static playerState currentState;
    public static Vector3 position;
    Vector3 playerInput;//input of where characters moves
    Vector3 displacement;
    Vector3 directionToTarget;//location of cursor

    float movRotAlignment;//alingment of move direction and look direction
    
    public CharacterController cnc;//cnc  movement

    [SerializeField,Range(0f,10f)]
    float rotationSpeed;// base speed at which the player rotates towards the pointer
    Vector3 hitpoint;//wher cursor lands
    Ray ray;//ray that hit where cursor moves


    [SerializeField, Range(0f, 100f)]
	float maxSpeedWalking = 5f,maxSpeedRunning= 10f;//max speed for walking and sprinting
    [SerializeField, Range(0f, 100f)]
	float maxAccelerationWalking = 5f,maxAccelerationRunning = 10f;//acceleration for walking and running

    float maxAcceleration, maxSpeed;
    
    Vector3 velocity;

    public MeshRenderer render;

    [Header("unity Events")]
    public UnityEvent<int> tripped = new UnityEvent<int>();
    // Start is called before the first frame update
    
    void Start(){
        //render = GetComponent<MeshRenderer>();
        currentState = playerState.walk;
        //anim.SetBool("isRunning",false);
    }
    //temporary function to signal changes in status
    void changeStatus_TMP(){
        if(Input.GetKey(KeyCode.LeftShift)){
            render.material.color = Color.green;

        }
        else if(Input.GetKey(KeyCode.LeftControl)){
            render.material.color = Color.red;
        }
        else{
            render.material.color = Color.white;
        }
    }
        
    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed",velocity.magnitude);
        GameLoop.playerPosition= transform.position;
        GameLoop.playerLoudnes = (int)currentState +1;
        timePassed -= Time.deltaTime;
        if(isPlaying){
            Movement();
            if(!isTripped){
                LookAtPoint();
                changeStatus_TMP();
                movRotAlignment= Vector3.Dot(directionToTarget.normalized,displacement.normalized);
            }else{
                if(timePassed <0){
                    timePassed =0;
                    isTripped=false;
                }
                Debug.Log(timePassed);


            }
        }
        
    }

    void Movement(){

        //running mechanosm
        if(Input.GetKey(KeyCode.LeftShift)){
            maxAcceleration = maxAccelerationRunning;
            maxSpeed = maxSpeedRunning;
            currentState = playerState.sprint;
            anim.SetBool("isRunning",true);
            GameLoop.playerLoudnes =5;

        }else{
             maxAcceleration = maxAccelerationWalking;
            maxSpeed = maxSpeedWalking;
            currentState = playerState.walk;
            anim.SetBool("isRunning",false);
            GameLoop.playerLoudnes =3;
        }

        if(movRotAlignment <=  0f){
            maxAcceleration = maxAcceleration *0.5f;
            maxSpeed = maxSpeed*0.5f;
        }
        if(Input.GetKey(KeyCode.LeftControl)){
            //render.material.color = Color.red;
            anim.SetBool("isCrunching",true);
            maxAcceleration = maxAcceleration *0.25f;
            maxSpeed = maxSpeed*0.5f;
            currentState = playerState.crouch;
            GameLoop.playerLoudnes =2;
        }else{
            currentState = playerState.walk;
            anim.SetBool("isCrunching",false);
            GameLoop.playerLoudnes =3;
        }

        playerInput = Vector2.zero;
        //Get input from player
        if(!isTripped){
            playerInput.x = Input.GetAxis("Horizontal");
            playerInput.y = Input.GetAxis("Vertical");
            playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        }

        Vector3 desiredVelocity  = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
       
        displacement = velocity * Time.deltaTime;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;

        if(Input.GetKey(KeyCode.LeftControl)){
            if(velocity.magnitude > desiredVelocity.magnitude){
                maxSpeedChange*=10;
            }
        }
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
		velocity.z =Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
       
        cnc.Move(displacement);
    }
    void LookAtPoint(){
        int layerMask = 1 << 8;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
         if(Physics.Raycast(ray,out hit,layerMask)){           
                hitpoint =hit.point;
                hitpoint.y = transform.position.y;          
        }
        directionToTarget = hitpoint -transform.position;
        // Calculate the desired rotation
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward + directionToTarget.normalized, Vector3.up);
        // Rotate towards the target rotation on the local T-axis
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("trap")){
            if(!Input.GetKey(KeyCode.LeftControl)){  
                Debug.Log("tripped");
                isTripped =true;
                timePassed = stunTime;
                tripped.Invoke(0);
            }
        }
        
    }
    void OnDrawGizmos(){

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, displacement.normalized);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, directionToTarget.normalized);
        
    }


}

public enum playerState{
    crouch,
    walk,
    sprint
}