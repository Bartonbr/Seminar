using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEditor;

public class Mechanic : MonoBehaviour
{
		public List<GameObject> partArray = new List<GameObject> ();
		//public int partCount = 0;
		public Vector3 spawnLocation = new Vector3 (0, 10, 0);
		public string strutToUse = "defaultStrut";

		private bool load (string fileName)
		{
	
				try {
						string line;
						GameObject previousInstance = null;
						GameObject instance1 = null;
						GameObject instance2 = null;
						StreamReader reader = new StreamReader (fileName, Encoding.Default);
						int[] instructions = new int[5];
						String FUID;
						GameObject parent = new GameObject ();
	
						using (reader) {
									
								do {
										line = reader.ReadLine ();
										if (line != null) {
												if (line.StartsWith ("N")) {
														FUID = line;
														spawnLocation = spawnLocation + new Vector3 (4, 0, 0);
														partArray = new List<GameObject> ();
														previousInstance = null;
														parent = new GameObject ();
														parent.transform.name = FUID;
												} else {
														instructions = stringToIntArray (line);
														// note that the instructions array is as follows:
														// Locations:
														// 0 --> ID of strut 1 -- this should be a pre existing strut
														// 1 --> ID of strut 2 -- this strut may or may not exist already
														// 2 --> side of strut 1 to connect strut 2 to
														// 3 --> angle relative to strut 1 at which strut 2 will be placed
														// 4 --> type of connection between struts

														//if the previous instance is null this means we are starting a new robot:
														// we begin by creating the first strut, this is a special case... a very special one as 
														// all subsequent creations should have one existing part and one new part OR two existing parts
														if (previousInstance == null) {
																instance1 = (GameObject)Instantiate (Resources.Load (strutToUse), spawnLocation, Quaternion.identity);
																instance1.transform.name = instructions [0].ToString ();
																partArray.Add (instance1);			
							

																parent.transform.position = instance1.transform.position;
																instance1.transform.parent = parent.transform;





														} 
													//	String potentialStrut = instructions [1].ToString ();
														if (findStrut (instructions [1].ToString ()) != null) {
																instance2 = findStrut (instructions [1].ToString ());
																affixStruts (instance1, instance2, instructions [4]);
														} else {
																instance1 = findStrut (instructions [0].ToString ());
																instance2 = (GameObject)Instantiate (Resources.Load (strutToUse), spawnLocation, Quaternion.identity);
																instance2.transform.name = instructions [1].ToString ();
																partArray.Add (instance2);
																parent.transform.position = instance2.transform.position;
																instance2.transform.parent = parent.transform;
																placeStrut (instance1, instance2, instructions [3], instructions [2]);
																affixStruts (instance1, instance2, instructions [4]);

														}
														previousInstance = instance1;
												}
										}
										createPrefab (parent);
								} while(line != null);
								reader.Close ();
								return true;
						}
				} catch (IOException e) {
						Debug.Log (e.Message); //dont work for sum reeson
						return false;
				}
		}

		private void createPrefab (GameObject o)
		{
				String path = "Assets/Resources/FunctionalUnits/" + o.transform.name + ".prefab";
				UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab (path);
				PrefabUtility.ReplacePrefab (o, prefab, ReplacePrefabOptions.ConnectToPrefab);



		}

		private void affixStruts (GameObject strutOne, GameObject strutTwo, int jointType)
		{
				ConfigurableJoint joint1;
				ConfigurableJoint joint2;
				switch (jointType) {
				case 0:
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
						break;
				case 1:

			//add configurable joint
						joint2 = (ConfigurableJoint)strutTwo.AddComponent ("ConfigurableJoint");
							strutTwo.AddComponent("actuate");
			//connecting the other strut
						joint2.connectedBody = (Rigidbody)strutOne.GetComponent ("Rigidbody");	
			//locking and limiting specific axis of rotation (this one is a hinge)
						joint2.angularXMotion = ConfigurableJointMotion.Limited;
						joint2.angularYMotion = ConfigurableJointMotion.Locked;
						joint2.angularZMotion = ConfigurableJointMotion.Locked;
						joint2.xMotion = ConfigurableJointMotion.Locked;
						joint2.yMotion = ConfigurableJointMotion.Locked;
						joint2.zMotion = ConfigurableJointMotion.Locked;
						joint2.anchor = new Vector3 (0, -1.0f, 0);
			//set up angular limits
						joint2.lowAngularXLimit = new SoftJointLimit (){ limit = -90, bounciness = 0, damper = 0, spring = 0 };
						joint2.highAngularXLimit = new SoftJointLimit (){ limit = 90, bounciness = 0, damper = 0, spring = 0 };
						joint2.angularXDrive = new JointDrive () {maximumForce = Mathf.Infinity, mode = JointDriveMode.Position, positionDamper = 150, positionSpring = 1000};  		
		
						break;
				default:
						Debug.Log ("joint type not found");
						break;
				}

	
		}

		private void placeStrut (GameObject initialStrut, GameObject placingStrut, 
	                             int relativeAngle, int side)
		{
				//get the colliders so we can see how big it is
				CapsuleCollider collider = (CapsuleCollider)initialStrut.GetComponent ("CapsuleCollider");

				int sideModifier = 1;
				//set the side modifier so the strut will be placed appropriately
				if (side == 0) {
						sideModifier = -1;
				}
				if (relativeAngle == 0) {
						//if the relative angle is zero we simply move it above or below the current strut;
						placingStrut.transform.rotation = initialStrut.transform.rotation * Quaternion.Euler (0, 0, 180 * side);		
						placingStrut.transform.position = initialStrut.transform.position;
						placingStrut.transform.Translate (0, (collider.height) * 0.85f, 0);						
				} else {	
						Debug.Log (collider.height);
						//if it is anything else we must do a little math to both rotate it and move it into the proper position
						placingStrut.transform.rotation = initialStrut.transform.rotation * Quaternion.Euler (0, relativeAngle * 90, 90);
						placingStrut.transform.position = initialStrut.transform.position;
						placingStrut.transform.Translate (0, ((collider.height) / 2) * 0.85f, 0); //
						placingStrut.transform.Translate ((collider.height / 2) * sideModifier * 0.85f, 0, 0);
				}
		}

		private GameObject findStrut (string ID)
		{
				foreach (GameObject element in partArray) {				
						if (String.Compare (ID, element.transform.name) == 0) {
								return element;
						}
				}
				return null;
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

		private IEnumerator Waiter (float seconds)
		{
				Debug.Log ("waiting!");
				yield return new WaitForSeconds (seconds);
		
		}

		// Use this for initialization
		void Start ()
		{
				//Debug.Log ("i got this far! promise!");
				load ("Assets/Resources/strutLibrary - 3.txt");

		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}