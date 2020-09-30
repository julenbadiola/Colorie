using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class _3_GameSuperficie : MonoBehaviour
{
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        int xstart = 822;
        List<GameObject> cubes = new List<GameObject>();
        for(int i = 0; i < 80; i++){
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(xstart, -68f, -2500);
            cube.transform.localScale = new Vector3(30, 30, 1);
            cube.transform.parent = parent.transform;
            int ystart = -68;
            cubes.Add(cube);
            for(int e = 0; e < 30; e ++){
                GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube2.transform.position = new Vector3(xstart, ystart, -2500);
                cube2.transform.localScale = new Vector3(30, 30, 1);
                ystart += 30;
                cube2.transform.parent = parent.transform;
                cubes.Add(cube2);
            }
            xstart += 30;
            
        }
        System.Random random = new System.Random();
        Color predominant;
        Color other;
        if(UnityEngine.Random.Range( 0, 1 )==1){
            predominant = Color.red;
            other = Color.white;
        }else{
            predominant = Color.white;
            other = Color.red;
        }

        int[] arr = Enumerable.Range(0, cubes.Count-1).OrderBy (x => random.Next ()).Take (Mathf.FloorToInt(cubes.Count / 3f)).ToArray();
        for (int i = 0; i < parent.transform.childCount; i++) {
            if(arr.Contains(i)){
                parent.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.color = predominant;
            }else{
                parent.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.color = other;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
