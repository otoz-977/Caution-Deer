using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GridTest : MonoBehaviour
{
    public float[,] treeProb = {{0f,0.5f,0.5f},{0.3f,0.2f,0.5f},{0.6f,0.3f,0.1f}};
    public GameObject prefab;
    public GameObject items;
    public int gridSize =20;
    [Range(0f,100f)]
    public float treeDensity;
    public float gridSpacing =1f;
    float prev1,prev2;
    public static List<Vector3> gridpoints = new List<Vector3>();
    public static List<gridsquare> terrain = new List<gridsquare>();
    public gridsquare[,] terrainGrid;
    List<GameObject> tempObjs = new List<GameObject>();
    public bool generated = false;
    // Start is called before the first frame update
    void Start()
    {
        prev1 = gridSize;
        prev2 = gridSpacing;
        generateGrid();
    }

    // Update is called once per frame
    void Update()
    {
       if(prev1 != gridSize || prev2 != gridSpacing){
            generateGrid();
            generated =false;
            prev1 =gridSize;
            prev2 = gridSpacing;
            ClearTerrain();
       }
    }
    void generateGrid(){
         //generate Grid
        gridpoints.Clear();
        for(int i =0; i<gridSize;i++){
            for(int j =0; j < gridSize;j++){
                //point position
                Vector3 point = new Vector3(i * gridSpacing,0.2f,j * gridSpacing);
                gridpoints.Add(point);

            }
        }
    }
    //generate Terrain
    [ContextMenu("Generate Terrain")]
    void GenerateTerrain(){
        //generate the squareGrid
        int indexCounter =0;
    

        terrain.Clear();
        foreach(Vector3 p in gridpoints){
            //get random number
            float rand =  UnityEngine.Random.Range(0f,1f);
            gridSquareType tempType;
            if(rand < treeDensity / 100f){
                tempType = gridSquareType.tree;
            }else{
                tempType = gridSquareType.empty;
            }

            gridsquare square = new gridsquare(indexCounter,p,tempType);
            terrain.Add(square);
            indexCounter++;
            

            
        }
        generated =true;
        DisplayTerrain();
    }
    
    [ContextMenu("Display Terrain")]
    public void DisplayTerrain(){
        tempObjs.Clear();
        foreach(gridsquare sqr in terrain){
            if(sqr.type == gridSquareType.tree){
                GameObject tmp = Instantiate(prefab,sqr.position,Quaternion.identity);
                tempObjs.Add(tmp);
            }
        }
    }


    void ClearTerrain(){
        foreach(GameObject bo in tempObjs){
            Destroy(bo);
            tempObjs.Clear();
        }
    }

    private void OnDrawGizmos() {
        if(!generated){
            foreach(Vector3 p in gridpoints){
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(p,0.25f);
            }
        }
        else{
            foreach(gridsquare sqr in terrain){
                if(sqr.type == gridSquareType.empty){
                    Gizmos.color =Color.red;
                }else{
                    Gizmos.color =Color.green;
                }
                //Handles.Label(sqr.position,$"{sqr.type}");
                Gizmos.DrawSphere(sqr.position,0.25f);
            }
        }
    }
}

[System.Serializable]
public class gridsquare
{   
    public gridsquare(int i, Vector3 pos, gridSquareType t){
        this.index = i;
        this.position =pos;
        this.type =t;
    }
    public gridsquare(int i, Vector3 pos, gridSquareType t, int id){
        this.index = i;
        this.position =pos;
        this.type =t;
        this.propId = id;
    }

    public void clearTile(){
        rgbProb[0] =0f;
        rgbProb[1] =0f;
        rgbProb[2] =0f;

    }
    //grid id or index in list
    public int index;
    //grdi exact positioning
    public Vector3 position;
    //is point null of tree or rock or prefab
    public gridSquareType type = gridSquareType.empty;
    public int propId=0;
    public float[] rgbProb = {0f,0f,0f};

}
[System.Serializable]
public enum gridSquareType{
    empty,
    boulder,
    tree
}
/*
    //convert list to 2D array
    terrainGrid = new gridsquare[gridSize,gridSize];
    for(int i=0;i <gridSize;i++){
        for(int j=0;i <gridSize;j++){
            terrainGrid[i,j] = new gridsquare(indexCounter,p,gridSquareType.empty);
        }
    }

    //pick random cell and collapse it
    int x = UnityEngine.Random.Range(0,gridSize-1);
    int y = UnityEngine.Random.Range(0,gridSize-1);
    terrainGrid[x,y].type = gridSquareType.tree;
    terrainGrid[x,y].propId = WFCfunc();
    terrainGrid[x,y].rgbProb[terrainGrid[x,y].propId] = 1f;

    //propagate info in sorounding tiles
    for(int i=0;i <gridSize;i++){
        for(int j=0;i <gridSize;j++){
            terrainGrid[i,j] = new gridsquare(indexCounter,p,gridSquareType.empty);
        }
    }

    indexCounter++;
    ******
    int WFCfunc(){//test wave collacpse function function
        ///collapse it through random chance into three trees
        float r1= UnityEngine.Random.Range(0f,1f);
        if(r1 <=0.333){
            //red
            return 0;
        }else if(r1 >0.333 && r1 <=0.66){
            //green
            return 1;
        }else{
            //blue
            return 2;
        }
       
    }
*/