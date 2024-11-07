using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class GridTest_v2 : MonoBehaviour
{
    
    public gridsquare[,] terrainGrid = null;
    [Range(1f,20f)]
    public float gridSpacing =1f;

    [Range(1,100)]
    public int gridSize =20;

    [Range(0f,100f)]
    public float treeDensity;

    void Start(){
        terrainGrid = new gridsquare[gridSize,gridSize];
    }
    [ContextMenu("generate new grid")]
    public void generateGrid(){
        //initializes new array
        terrainGrid = new gridsquare[gridSize,gridSize];

        //generates grid
        int index =0;
        for(int i =0; i<gridSize;i++){
            for(int j =0; j < gridSize;j++){
                //point position
                Vector3 point = new Vector3(i * gridSpacing,transform.position.y,j * gridSpacing);
                gridsquare sqr = new gridsquare(index,point,gridSquareType.empty);
                
                terrainGrid[i,j] = sqr;
            }
        }
    }
    [ContextMenu("Random generation")]
    public void randomGeneration(){
         for(int i =0; i<gridSize;i++){
            for(int j =0; j < gridSize;j++){
                float rand =  UnityEngine.Random.Range(0f,1f);
                gridSquareType tempType;
                if(rand < treeDensity / 100f){
                    tempType = gridSquareType.tree;
                    //circleGeneration(terrainGrid[i,j].position);
                }else{
                    tempType = gridSquareType.empty;
                }
                terrainGrid[i,j].type = tempType;
               
            }
        }
    }

    [ContextMenu("Random Point generation")]
    public void pointSpreadGen(){
        List<gridsquare> temp = new List<gridsquare>(); 
        for(int t=0;t<Random.Range(2,5);t++){
            Vector2 randpoint = new Vector2(Random.Range(0,gridSize-1),Random.Range(0,gridSize-1));
            int rand =  UnityEngine.Random.Range(2,5);
            for(int i = -rand; i<= rand;i++){
                for(int j = -rand; j <= rand;j++){
                    int one =(int)randpoint.x +i < gridSize && (int)randpoint.x +i >0?(int)randpoint.x +i: gridSize-1;
                    int y= (int)randpoint.y +j < gridSize && (int)randpoint.y +j >0?(int)randpoint.y +j: gridSize-1;
                        //terrainGrid[one,y].type = gridSquareType.tree;
                    temp.Add(terrainGrid[one,y]);               
                }
            }
       } 
        for(int i =0; i<gridSize;i++){
            for(int j =0; j < gridSize;j++){
                if(!temp.Contains(terrainGrid[i,j])){
                    terrainGrid[i,j].type = gridSquareType.empty;
                }
            }
        }
    }
    [ContextMenu("circle generation")]
    public void circleGeneration(Vector3 point){
        int numberOfPoints =10;
        //Vector2 startingPoint = new Vector2((gridSize -1)/2,(gridSize -1)/2);
        Vector2 startingPoint = new Vector2(point.x,point.z);
        float radius = (gridSpacing/2f);
        Vector3 p = new Vector3(radius,0f,0f);
        Vector3 center = new Vector3(startingPoint.x ,0f,startingPoint.y);
        float angle = (float) Math.PI * 2 / (float)numberOfPoints;
        List<Vector3> points = new List<Vector3>();

        for(int i=0; i< numberOfPoints;i++){
            float tmp = angle *i;
            float x = center.x + (radius * Random.Range(0.25f,1.2f)) * MathF.Cos(tmp);
            float y = center.z + (radius * Random.Range(0.25f,1.2f)) * MathF.Sin(tmp);
           points.Add(new Vector3(x,0f,y));
           GameObject tmpgm = GameObject.CreatePrimitive(PrimitiveType.Sphere);
           tmpgm.transform.SetParent(this.transform);
            tmpgm.transform.position = new Vector3(x,0f,y);
            tmpgm.transform.localScale *=Random.Range(0.1f,0.33f); 
        }


    }
    private void OnDrawGizmos() {
       /*
        if(terrainGrid != null){
            for(int i =0; i<gridSize;i++){
                for(int j =0; j < gridSize;j++){
                    gridSquareType gridType = terrainGrid[i,j].type;
                    switch(gridType){
                        case gridSquareType.empty:
                            Gizmos.color = Color.gray;
                        break;
                        case gridSquareType.boulder:
                            Gizmos.color = Color.black;
                        break;
                        case gridSquareType.tree:
                            Gizmos.color = Color.green;
                        break;
                        default:
                        break;
                    }
                
                    //Handles.Label(terrainGrid[i,j].position, $"{terrainGrid[i,j].rgbProb[0]}\n {terrainGrid[i,j].rgbProb[1]}\n {terrainGrid[i,j].rgbProb[2]}");
                    Gizmos.DrawSphere(terrainGrid[i,j].position,0.25f);
                    
                }
            }
        }
       */
        
    }
}
