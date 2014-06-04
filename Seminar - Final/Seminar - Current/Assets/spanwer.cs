using UnityEngine;
using System.Collections;



public class spanwer : MonoBehaviour {
	
    int length = 100;
	int width = 50;

	
	void generateField(int length, int width){
		
		for(int x = 0; x < length; x++){
			for(int z = 0; z < width; z++){
				
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.position = new Vector3(x - 50,0 + (Random.Range(-2F, 2F)*0.1F),z + 35);
				
			}
		}
		
	}

	// Use this for initialization
	void Start () {
		generateField (length, width);	
	}
	
	// Update is called once per frame

}
