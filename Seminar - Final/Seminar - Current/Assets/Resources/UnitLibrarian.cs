using UnityEngine;
using System.Collections;
using System.IO;

public class Librarian : MonoBehaviour
{
		int maxStruts = 3;
		int connectionAngles = 5;
		int connectionSides = 2;
		//int connectionTypes = 2;
		string FILE_NAME;
		int initialStrutID = 1000;


		// Use this for initialization
		void generateLibrary (int numStruts)
		{
				int identityStrut = initialStrutID;
			//	int currentStrut;
				FILE_NAME = "strutLibrary - " + maxStruts + ".txt";
				if (File.Exists (FILE_NAME)) {
						Debug.Log ("File already exists. Delete it or something.");
						return;
				}
				StreamWriter sr = File.CreateText (FILE_NAME);
				int FUID = 1000;
				for (int strutSideOne = 0; strutSideOne < connectionSides; strutSideOne++) {
						for (int strutAngleOne = 0; strutAngleOne < connectionAngles; strutAngleOne++)
								for (int strutSideTwo = 0; strutSideTwo < connectionSides; strutSideTwo++) { 
										for (int strutAngleTwo = 0; strutAngleTwo < connectionAngles; strutAngleTwo++) {
												if (strutSideOne == strutSideTwo && strutAngleOne == strutAngleTwo) {
										
												} else {
														
														sr.WriteLine ("N" + FUID);
														sr.WriteLine (identityStrut + "," + (identityStrut + 1) + "," + strutSideOne + "," + strutAngleOne + ",0");
														sr.WriteLine (identityStrut + "," + (identityStrut + 2) + "," + strutSideTwo + "," + strutAngleTwo + ",0");
														FUID++;
														sr.WriteLine ("N" + FUID);
														sr.WriteLine (identityStrut + "," + (identityStrut + 1) + "," + strutSideOne + "," + strutAngleOne + ",1");
														sr.WriteLine (identityStrut + "," + (identityStrut + 2) + "," + strutSideTwo + "," + strutAngleTwo + ",0");
														FUID++;
														sr.WriteLine ("N" + FUID);
														sr.WriteLine (identityStrut + "," + (identityStrut + 1) + "," + strutSideOne + "," + strutAngleOne + ",1");
														sr.WriteLine (identityStrut + "," + (identityStrut + 2) + "," + strutSideTwo + "," + strutAngleTwo + ",1");
														FUID++;
												}

										}
								}
				}
				sr.Close ();
		}

		void Start ()
		{
				//generateLibrary (maxStruts);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
