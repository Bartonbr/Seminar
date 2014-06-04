using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEditor;
using AssemblyCSharp;

public class Architect : MonoBehaviour
{

		public Vector3 spawnLocation = new Vector3 (0, 10, 0);
		public List<GameObject> FUArray = new List<GameObject> ();
		float maxUnitSize = 5.1f;
		int strutsPerUnit = 5;
		

		// Use this for initialization
		void Start ()
		{
				//generateRobots ("Assets/Resources/initialPop.txt");
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public bool generateRobots (string fileName)
		{
			
				try {

						string line;
						GameObject previousInstance = null;
						GameObject instance1 = null;
						GameObject instance2 = null;
						StreamReader reader = new StreamReader (fileName, Encoding.Default);
						int[] instructions = new int[4];
						String BUID;
						GameObject parent = null;

						using (reader) {
					
								do {


										line = reader.ReadLine ();
										if (line != null) {

												if (line.StartsWith ("B")) {
														BUID = line;
														spawnLocation = spawnLocation + new Vector3 (10, 0, 0);
														FUArray = new List<GameObject> ();
														previousInstance = null;
														parent = (GameObject)Instantiate (Resources.Load ("parentObject"));
														parent.transform.name = BUID;
										



													
											
												} else {
														instructions = stringToIntArray (line);

														if (previousInstance == null) {
																instance1 = (GameObject)Instantiate (Resources.Load ("FunctionalUnits/F" + instructions [2]));
																instance1.transform.position = spawnLocation;
																FUArray.Add (instance1);		
																parent.transform.position = instance1.transform.position;
																instance1.transform.parent = parent.transform;
																instance1.transform.name = instructions [0].ToString ();
														} 

														if (findUnit (instructions [1].ToString ()) != null) {
																instance2 = findUnit (instructions [1].ToString ());
																Debug.Log ("Unit already exists");
																//affixStruts (instance1, instance2, instructions [4]);
														} else {														
																instance1 = findUnit (instructions [0].ToString ());
																instance2 = (GameObject)Instantiate (Resources.Load ("FunctionalUnits/F" + instructions [3]));
																instance2.transform.position = spawnLocation;
																instance2.transform.name = instructions [1].ToString ();
														
																FUArray.Add (instance2);
																instance2.transform.parent = parent.transform;
																placeNewUnit (instance1, instance2, instructions [4]);
																
																affixStruts (returnClosest (instance1.transform.root.gameObject, instance2) [0].gameObject, returnClosest (instance1.transform.root.gameObject, instance2) [1].gameObject);

														}
														previousInstance = instance1;




												}

										}
										//	createPrefab (parent);
								} while(line != null);
								reader.Close ();
								return true;
						}
				} catch (IOException e) {
						Debug.Log (e.Message); //dont work for sum reeson
						return false;
				}



		}

		public bool generateRobots (bluePrint[] population)
		{

				string line;
				GameObject previousInstance = null;
				GameObject instance1 = null;
				GameObject instance2 = null;			
				int[] instructions = new int[4];
				String BUID;
				GameObject parent = null;

					
				for (int bluePrint = 0; bluePrint < population.Length; bluePrint ++) {
						bluePrint currentBP = population [bluePrint];
		
						BUID = currentBP.ID;
						spawnLocation = spawnLocation + new Vector3 (10, 0, 0);
						FUArray = new List<GameObject> ();
						previousInstance = null;
						parent = (GameObject)Instantiate (Resources.Load ("parentObject"));
						parent.transform.name = BUID;
	
						for (int chromosome = 0; chromosome < currentBP.chromosomes.Length; chromosome++) {




								line = currentBP.chromosomes [chromosome];
								if (line != null) {

										instructions = stringToIntArray (line);
							
										if (previousInstance == null) {
												instance1 = (GameObject)Instantiate (Resources.Load ("FunctionalUnits/F" + instructions [2]));
												instance1.transform.position = spawnLocation;
												FUArray.Add (instance1);		
												parent.transform.position = instance1.transform.position;
												instance1.transform.parent = parent.transform;
												instance1.transform.name = instructions [0].ToString ();
										} 
							
										if (findUnit (instructions [1].ToString ()) != null) {
												instance2 = findUnit (instructions [1].ToString ());
												Debug.Log ("Unit already exists");
												//affixStruts (instance1, instance2, instructions [4]);
										} else {														
												instance1 = findUnit (instructions [0].ToString ());
												instance2 = (GameObject)Instantiate (Resources.Load ("FunctionalUnits/F" + instructions [3]));
												instance2.transform.position = spawnLocation;
												instance2.transform.name = instructions [1].ToString ();
								
												FUArray.Add (instance2);
												instance2.transform.parent = parent.transform;
												placeNewUnit (instance1, instance2, instructions [4]);
								
												affixStruts (returnClosest (instance1, instance2) [0].gameObject, returnClosest (instance1, instance2) [1].gameObject);
								
										}
										previousInstance = instance1;
							
							
							
							
								}
						
						}
						//	createPrefab (parent);

	

							
						currentBP.instantiation = parent;
				}
				

				return true;
		}

		public GameObject generateRobot (bluePrint currentBP, Vector3 spawnLocation)
		{	

				string line;
				GameObject previousInstance = null;
				GameObject instance1 = null;
				GameObject instance2 = null;			
				int[] instructions = new int[4];
				String BUID;
				GameObject parent = null;	
				//Debug.Log ("crhoms and sl" + currentBP.chromosomes [0]);
				BUID = currentBP.ID;
				
				FUArray = new List<GameObject> ();
				previousInstance = null;
				parent = (GameObject)Instantiate (Resources.Load ("parentObject"));
				parent.tag = "Empty";
				parent.transform.name = BUID;


				for (int chromosome = 0; chromosome < currentBP.chromosomes.Length; chromosome++) {
			
			
			
						line = currentBP.chromosomes [chromosome];
						if (line != null) {
				
								instructions = stringToIntArray (line);
				
								if (previousInstance == null) {
										instance1 = (GameObject)Instantiate (Resources.Load ("FunctionalUnits/F" + instructions [2]));
										Transform[] children = new Transform[instance1.transform.childCount];

										instance1.transform.position = spawnLocation;
										FUArray.Add (instance1);		
										parent.transform.position = spawnLocation;
										instance1.transform.parent = parent.transform;
										instance1.transform.name = instructions [0].ToString ();
										int index = 0;
										foreach (Transform child in instance1.transform) {
												children [index++] = child;
												//Debug.Log(child);
										}
										currentBP.trackerObject = children [0];
								} 
				
								if (findUnit (instructions [1].ToString ()) != null) {
										instance2 = findUnit (instructions [1].ToString ());
										
										Debug.Log ("Unit already exists: " + instructions [1].ToString ());
										//affixStruts (instance1, instance2, instructions [4]);
								} else {														
										instance1 = findUnit (instructions [0].ToString ());
										instance2 = (GameObject)Instantiate (Resources.Load ("FunctionalUnits/F" + instructions [3]));
										instance2.transform.position = spawnLocation;
										instance2.transform.name = instructions [1].ToString ();
					
										FUArray.Add (instance2);
										
										placeNewUnit (instance1, instance2, instructions [4]);
									
						
										affixStruts (returnClosest (instance1.transform.root.gameObject, instance2) [0].gameObject, returnClosest (instance1.transform.root.gameObject, instance2) [1].gameObject);
										instance2.transform.parent = parent.transform;
					
								}
								previousInstance = instance1;

	
						}
				}

				//currentBP.instantiation = parent;

				return parent;
		}
		
		void placeNewUnit (GameObject initialUnit, GameObject attachingUnit, int attachingSide)
		{
	
				Transform[] closestStruts;
				attachingUnit.transform.position = attachingUnit.transform.position;
				float delta;
				switch (attachingSide) {
				case 0:
						attachingUnit.transform.position += new Vector3 (0, maxUnitSize, 0);
						closestStruts = returnClosest (initialUnit.transform.root.gameObject, attachingUnit);
						delta = Vector3.Distance (closestStruts [0].position, closestStruts [1].position);
						attachingUnit.transform.position += new Vector3 (0, -(delta - (maxUnitSize / 3)), 0);
						break;
				case 1:
						attachingUnit.transform.position += new Vector3 (0, -maxUnitSize, 0);
						closestStruts = returnClosest (initialUnit.transform.root.gameObject, attachingUnit);
						delta = Vector3.Distance (closestStruts [0].position, closestStruts [1].position);
						attachingUnit.transform.position += new Vector3 (0, (delta - (maxUnitSize / 3)), 0);
						break;
				case 2:
						attachingUnit.transform.position += new Vector3 (0, 0, maxUnitSize);
						closestStruts = returnClosest (initialUnit.transform.root.gameObject, attachingUnit);
						delta = Vector3.Distance (closestStruts [0].position, closestStruts [1].position);
						attachingUnit.transform.position += new Vector3 (0, 0, -(delta - (maxUnitSize / 3)));
						break;
				case 3:
						attachingUnit.transform.position += new Vector3 (maxUnitSize, 0, 0);
						closestStruts = returnClosest (initialUnit.transform.root.gameObject, attachingUnit);
						delta = Vector3.Distance (closestStruts [0].position, closestStruts [1].position);
						attachingUnit.transform.position += new Vector3 (-(delta - (maxUnitSize / 3)), 0, 0);
						break;
				case 4:
						attachingUnit.transform.position += new Vector3 (0, 0, -maxUnitSize);
						closestStruts = returnClosest (initialUnit.transform.root.gameObject, attachingUnit);
						delta = Vector3.Distance (closestStruts [0].position, closestStruts [1].position);
						attachingUnit.transform.position += new Vector3 (0, 0, (delta - (maxUnitSize / 3)));
						break;
				case 5:
						attachingUnit.transform.position += new Vector3 (-maxUnitSize, 0, 0);
						closestStruts = returnClosest (initialUnit.transform.root.gameObject, attachingUnit);
						delta = Vector3.Distance (closestStruts [0].position, closestStruts [1].position);
						attachingUnit.transform.position += new Vector3 ((delta - (maxUnitSize / 3)), 0, 0);
						break;
				default:
						Debug.Log ("Attaching Side: Number out of acceptable range");
						break;


				}

		

		}

		void alignOrigin ()
		{

		}

		void rotateUnit (GameObject unitToRotate, int rotation)
		{
				switch (rotation) {
				case 0:		
			//no rotation
						break;
				case 1:
						unitToRotate.transform.rotation = unitToRotate.transform.rotation * Quaternion.Euler (0, 0, 90);
						break;
				case 2:
						unitToRotate.transform.rotation = unitToRotate.transform.rotation * Quaternion.Euler (0, 0, 180);
						break;
				case 3:
						unitToRotate.transform.rotation = unitToRotate.transform.rotation * Quaternion.Euler (0, 0, 270);
						break;
				case 4:
						unitToRotate.transform.rotation = unitToRotate.transform.rotation * Quaternion.Euler (0, 0, 90);
						break;
				case 5:
						unitToRotate.transform.rotation = unitToRotate.transform.rotation * Quaternion.Euler (0, 0, 270);
						break;
				default:
						Debug.Log ("Rotation: Number out of acceptable range");
						break;
			
			
				}
		}

		List<Transform> getNonEmptyChildren (GameObject parent)
		{
		//	Debug.Log ("Evaluating: " + parent.name);
			List<Transform> ret = new List<Transform> ();
//				Debug.Log (parent.name);
//				foreach (Transform Child in parent.transform) {
//		
//						getNonEmptyChildren (Child.gameObject);
//
//						if (Child.tag.CompareTo ("Empty") != 0) {
//								//	Debug.Log(Child.name + "Added");
//								ret.Add (Child);
//						}
//
//			
//
//				}
		Transform[] maybeRet = parent.GetComponentsInChildren<Transform> ();
		//Debug.Log ("returning");

				foreach (Transform thing in maybeRet) {
						if(thing.gameObject.tag.CompareTo("Empty") != 0){
				ret.Add(thing);
					//	Debug.Log (thing.name + "is in ret");

			}
				}
				return ret;

		}

		Transform[] returnClosest (GameObject unitOne, GameObject unitTwo)
		{
				Transform[] returnStruts = new Transform[2];
				List<Transform> strutsOne = new List<Transform> ();
				List<Transform> strutsTwo = new List<Transform> ();
				float currentDistance = 5000.0f;
				Transform strutOne = null;
				Transform strutTwo = null;
				int strutsInArray = 0;
				//	int numOfEqDist = 0;
				List<Transform> eqStrutsOne = new List<Transform> ();
				List<Transform> eqStrutsTwo = new List<Transform> ();
		
		
		
				strutsOne = getNonEmptyChildren (unitOne);
//		Debug.Log ("things in ret");
//				foreach (Transform thing in strutsOne) {
//						
//						Debug.Log (thing.name);
//				}
			
				foreach (Transform child in unitTwo.transform) {
						strutsTwo.Add (child);
						//	Debug.Log(child.name);
				}
		
		
		
				for (int i = 0; i < strutsOne.Count; i ++) {
						for (int j = 0; j < strutsTwo.Count; j ++) {
								float actualDist = Vector3.Distance (strutsOne [i].position, strutsTwo [j].position);
								if (actualDist == currentDistance) {
										eqStrutsOne.Add (strutOne);
										eqStrutsTwo.Add (strutTwo);
								} else if (actualDist < currentDistance) {		
										currentDistance = actualDist;
										strutOne = strutsOne [i];
										strutTwo = strutsTwo [j];
										eqStrutsOne = new List<Transform> ();
										eqStrutsTwo = new List<Transform> ();
										eqStrutsOne.Add (strutOne);
										eqStrutsTwo.Add (strutTwo);
								}
				
				
						}
			
				}
		
		
				strutOne = eqStrutsOne [0];
				strutTwo = eqStrutsTwo [0];
				for (int i = 0; i<eqStrutsOne.Count; i++) {
			
						if (eqStrutsOne [i].gameObject.name == "1000" && 
								eqStrutsTwo [i].gameObject.name == "1000") {
								strutOne = eqStrutsOne [i];
								strutTwo = eqStrutsTwo [i];
				
						}
				}
				returnStruts [0] = strutOne;
				returnStruts [1] = strutTwo;
				return returnStruts;


		}

		private void affixStruts (GameObject strutOne, GameObject strutTwo)
		{
				ConfigurableJoint joint1;
				ConfigurableJoint joint2;


				//add configurable joints to both gameobjects
				joint1 = (ConfigurableJoint)strutOne.AddComponent ("ConfigurableJoint");
				joint2 = (ConfigurableJoint)strutTwo.AddComponent ("ConfigurableJoint");
				//connect joints from opposite objects to one another
				joint1.connectedBody = (Rigidbody)strutTwo.GetComponent ("Rigidbody");
				joint2.connectedBody = (Rigidbody)strutOne.GetComponent ("Rigidbody");
				//locking all rotation for joint 1
				joint1.angularXMotion = ConfigurableJointMotion.Locked;
				joint1.angularYMotion = ConfigurableJointMotion.Locked;
				joint1.angularZMotion = ConfigurableJointMotion.Locked;
				joint1.xMotion = ConfigurableJointMotion.Locked;
				joint1.yMotion = ConfigurableJointMotion.Locked;
				joint1.zMotion = ConfigurableJointMotion.Locked;
				//locking all rotation for joint 2
				joint2.angularXMotion = ConfigurableJointMotion.Locked;
				joint2.angularYMotion = ConfigurableJointMotion.Locked;
				joint2.angularZMotion = ConfigurableJointMotion.Locked;
				joint2.xMotion = ConfigurableJointMotion.Locked;
				joint2.yMotion = ConfigurableJointMotion.Locked;
				joint2.zMotion = ConfigurableJointMotion.Locked;
				joint2.anchor = new Vector3 (0, -1.0f, 0);

		}

		private int[] stringToIntArray (String input)
		{
				String[] inputArray = input.Split (',');
				int[] outputArray = new int[inputArray.Length];
				for (int i = 0; i < inputArray.Length; i++) {
						if (! int.TryParse (inputArray [i], out outputArray [i])) {
								Debug.Log ("Parse of comma delimmited values FAILED, Check your input file");
						}
				}
				return outputArray;
		}

		private GameObject findUnit (string ID)
		{
				foreach (GameObject element in FUArray) {				
						if (String.Compare (ID, element.transform.name) == 0) {
								return element;
						}
				}
				return null;
		}



}
