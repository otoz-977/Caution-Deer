using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class TreeSpawner : MonoBehaviour
{
    List<GameObject> trees = new List<GameObject>();
    [Header("spawnign probability")]
    [Range(0f,100f)]
    public float treeDensity;
    [Range(0f,100f)]
    public float bushDensity;
    [Header("RGB mineral")]
    [Range(0f,100f)]
    public float redDensity;
    [Range(0f,100f)]
    public float greenDensity;
    
    [Range(0f,100f)]
    public float blueDensity;
    [Header("prefabs")]
    public GameObject[] Tree_prefabs;
    public GameObject[] Bush_prefabs;


    [Header("Ignore")]
    public GridTest_v2 tGrid;
    public GameObject treeHolder;
    public GameObject bushHolder;

    // Start is called before the first frame update
    [ContextMenu("Create Terrain")]
    public void BuildTerrain(){
        clearTrees();//clears map
        //distributes RGB energy. that determines the kind foilliage 
        seedTerrain(0,redDensity);
        seedTerrain(1,greenDensity);
        seedTerrain(2,blueDensity);


        //iterates over grid and plant trees
        for(int i =0; i<tGrid.gridSize;i++){
            for(int j =0; j < tGrid.gridSize;j++){               
                buildTree(i,j);             
            }
        }
        //iterates over grid and plant bushes
        for(int i =0; i<tGrid.gridSize;i++){
            for(int j =0; j < tGrid.gridSize;j++){
                buildBushes(i,j);
            }
        }
    }
    
    void buildTree(int x,int y){
        
        //checks which energy  is present the most in this tile
        int tmpid =0;
        float var =0f;
        for(int i =0; i<3; i++){
            if(var < tGrid.terrainGrid[x,y].rgbProb[i]){
                var = tGrid.terrainGrid[x,y].rgbProb[i];
                tmpid =i;
            }
        }
        if(UnityEngine.Random.Range(0f,1f) < treeDensity/100f){
            tGrid.terrainGrid[x,y].type = gridSquareType.tree;


            GameObject tmp = Instantiate(Tree_prefabs[getRandomTree(tmpid)],getrandomLocation(tGrid.terrainGrid[x,y].position),Quaternion.identity);
            tmp.transform.parent = treeHolder.transform;
            tGrid.terrainGrid[x,y].propId = tmpid;
            tmp.transform.localScale = new Vector3(1f,0.50f + tGrid.terrainGrid[x,y].rgbProb[tmpid],1f); 
            trees.Add(tmp);    
            //S = x
        }else{
            tGrid.terrainGrid[x,y].type = gridSquareType.empty;
            return;
        }
    }

    Vector3 getrandomLocation(Vector3 position){

        float  randx = Random.Range(-(tGrid.gridSpacing *0.75f),tGrid.gridSpacing *0.75f);
        float  randy = Random.Range(-(tGrid.gridSpacing *0.75f),tGrid.gridSpacing *0.75f);
        
        return new Vector3(position.x +randx,position.y,position.z + randy );
    }
    void buildBushes(int x,int y){
        if(tGrid.terrainGrid[x,y].type != gridSquareType.empty){
            return;
        }
         if(UnityEngine.Random.Range(0f,1f) < bushDensity/100f){
            int tmpid = countAverage(x,y);
            GameObject tmp = Instantiate(Bush_prefabs[getRandomTree(tmpid)],getrandomLocation(tGrid.terrainGrid[x,y].position),Quaternion.identity);
            tmp.transform.parent =bushHolder.transform;
        }              
    }

   int countAverage(int x,int y){
       
        
        int[] tmp ={0,0,0};
        for(int t =x-2; t< x+2;t++){
            for(int z =y-1; z < y+1;z++){
                if(checkRange(t,tGrid.gridSize) && checkRange(z,tGrid.gridSize)){
                    switch(tGrid.terrainGrid[t,z].propId){
                            case 0:
                                tmp[0]++;
                            break;
                            case 1:
                                tmp[1]++;
                            break;
                            case 2:
                                tmp[2]++;
                            break;
                            default:
                            break;
                    }
                }
                continue;                      
            }
        }
        int mx = tmp.Max();
        return tmp.ToList().IndexOf(mx);
    }
    int getRandomTree(int i){
        switch(i){
            case 0:
                return 0;
            
            case 1:
                return Random.Range(0.0f,0.99f) < 0.5f?1:3;
            
            case 2:
                return 2;
            
            default:
                return 0;           
        }
    }
    [ContextMenu("Clear Terrain")]
    void clearTrees(){
        trees.Clear();
        foreach(Transform child in treeHolder.transform){
              DestroyImmediate(child.gameObject);
        }
        foreach(Transform child in bushHolder.transform){
              DestroyImmediate(child.gameObject);
        }       
    }
    void seedTerrain(int mineral,float res){
         for(int i =0; i<tGrid.gridSize;i++){
            for(int j =0; j < tGrid.gridSize;j++){
                float rand =  UnityEngine.Random.Range(0f,1f);
                if(rand < res / 100f){
                    //gets grids that are green  
                    tGrid.terrainGrid[i,j].rgbProb[mineral] =1f;
                }
               
            }
        }
        //average grid spread
        for(int i =0; i<tGrid.gridSize;i++){
            for(int j =0; j < tGrid.gridSize;j++){
               
                //average folliage density
                //float foilageDensity = tGrid.terrainGrid[i,j].rgbProb[0];

                float sum =0f;
                int ind =0;
                for(int t =i-1; t< i+1;t++){
                    for(int z =j-1; z < j+1;z++){
                        if(checkRange(t,tGrid.gridSize) && checkRange(z,tGrid.gridSize)){
                            sum += tGrid.terrainGrid[t,z].rgbProb[mineral];
                            ind++;
                        }
                        continue;
                        
                    }
                }
                tGrid.terrainGrid[i,j].rgbProb[mineral] = sum /ind;
            }
        }
    }
    bool checkRange(int t, int max){
        return t>=0 && t< max?true:false;
    }
}
