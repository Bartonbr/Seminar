using UnityEngine;
using System.IO;
using System.Text;
using System;
using UnityEditor;

public class FUGenFromTemplates : MonoBehaviour
{
		Vector3 spawnLocation = new Vector3 (0, 0, 0);
		// Use this for initialization

		public void generateUnitFromTemplate (int numberOfTemplates)

		{
		int iterations = 0;

		for (int i = 1; i <= numberOfTemplates; i++) {
						GameObject temp = (GameObject)Instantiate (Resources.Load ("Templates/" + i), spawnLocation, Quaternion.identity);
						int y = 0, z = 0, tempx = 0, tempz = 0;
						for (z = 0; z < 6; z ++) {
						
								for (y = 0; y < 4; y++) {
										switch (z) {
										case 0:
										case 1:
										case 2:
										case 3:
												tempx = 0;
												tempz = z;
												break;
										case 4:
												tempx = 1;
												tempz = 0;
												break;
										case 5:
												tempx = 3;
												tempz = 0;	
												break;
										default:
												break;

										}
								
										rotateUnit2 (temp, tempx, y, tempz);		             	
										createPrefab (temp,"F" + iterations);	
										temp.transform.rotation = Quaternion.identity;
										iterations ++;
								}
						}
				}
		}

		public void rotateUnit (GameObject unitToRotate, int rotationX, int rotationY, int rotationZ)
		{
				unitToRotate.transform.rotation = unitToRotate.transform.rotation * Quaternion.Euler (90 * rotationX, 90 * rotationY, 90 * rotationZ);			

		}

		public void rotateUnit2 (GameObject unitToRotate, int rotationX, int rotationY, int rotationZ)
		{
		unitToRotate.transform.Rotate(90 * rotationX, 90 * rotationY, 90 * rotationZ);			
		
		}

		void Start ()
		{

						generateUnitFromTemplate (24);
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		private void createPrefab (GameObject o, String name)
		{
				String path = "Assets/Resources/FunctionalUnits/" + name + ".prefab";
				UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab (path);
				PrefabUtility.ReplacePrefab (o, prefab, ReplacePrefabOptions.ConnectToPrefab);
		
		
		
		}
}
	