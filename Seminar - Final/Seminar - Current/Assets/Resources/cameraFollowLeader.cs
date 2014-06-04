using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class cameraFollowLeader : MonoBehaviour {

	// Use this for initialization

	GameObject controller;
	Gallium GA;
	bluePrint[] leaders;
	float switchDelay = 5;
	float prevTime  = -1000;
	Transform highestOb;
	Transform prevOb;
	bool switcher;

	void Start () {
		switcher = true;
		prevTime = Time.time;
		controller = GameObject.Find ("Controller");
		Gallium GA = (Gallium)controller.GetComponent ("Gallium");
		leaders = GA.robotsInScene;



	}
	
	// Update is called once per frame
	void Update () {
		Gallium GA = (Gallium)controller.GetComponent ("Gallium");
		leaders = GA.robotsInScene;



		if (leaders [0] != null) {
			if (highestOb == null) {
				highestOb = leaders[0].trackerObject;
			}

						if (Time.time > prevTime + switchDelay) {
		
								foreach (bluePrint a in leaders) {
										if (a.trackerObject.transform.position.x < highestOb.transform.position.x) {

												highestOb = a.trackerObject;
												//switcher = true;
												
												
												//Debug.Log (highestOb);
										}

								}
								prevTime = Time.time;
			
						}
				}
//			if(switcher){
		if (highestOb != null) {
				transform.position = new Vector3 (highestOb.position.x, highestOb.position.y + 10, highestOb.position.z + 25);
//				switcher = false;			//transform.position = new Vector3 (highestOb.position.x, highestOb.position.y + 10, highestOb.position.z - 10);
//			}
				}
	}
}
