using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using AssemblyCSharp;
using System.Text;
using System;

public class Gallium : MonoBehaviour
{
		// Use this for initialization
		int numberOfUnits = 5;
		int populationSize = 100;
		float initialSpawnLocation = 0;
		int currentGeneration = 0;
		int generations = 250;
		int maxValueOfUnit = 575;
		int unitsPerRun = 25;
		float timer = 0;
		float runTime = 10;
		float timeScale  = 2.0f;
		public bool disableRender = true;

		public bluePrint[] currentPopulation { get; set; }

		string FILE_NAME;
		int iteration = 0;
		bool runComplete = false;
		bool simulationComplete = false;
		public bluePrint[] robotsInScene;
		GameObject[] terrainInScene;
		int objectCount = 0;
		float runMean;
		float standardDeviation;

		void generateRun (string testArea)
		{
				float currentSpawnLocation = initialSpawnLocation;
				float bufferSpace = 30;

				float height = 10;
				//	float runTime = 10;

				GameObject controller = GameObject.Find ("Controller");
				Architect archy = (Architect)controller.GetComponent (typeof(Architect));

				for (int i = 0; i < unitsPerRun; i ++) {
						bluePrint robot = currentPopulation [iteration];
						GameObject instance1 = (GameObject)Instantiate (Resources.Load ("Terrain/" + testArea));
						terrainInScene [objectCount] = instance1;
						currentSpawnLocation += instance1.collider.bounds.size.y + bufferSpace;
						instance1.transform.position = new Vector3 (0, currentSpawnLocation, 0);
						robot.instantiation = (GameObject)archy.generateRobot (robot, new Vector3 ((instance1.transform.position.x), currentSpawnLocation + height, instance1.transform.position.z));
						robot.startLocation = robot.trackerObject.transform.position.x;
						robotsInScene [objectCount] = robot;
						objectCount ++;
						iteration ++;

				}
				MeshRenderer[] renderObjects = FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];
				if (disableRender) {
						foreach (MeshRenderer a in renderObjects) {
								a.enabled = false;
						}
				}
				objectCount = 0;





		}

		bluePrint[] evalPopulation (bluePrint[] pop)
		{
				//Debug.Log ("eval call");
				for (int i = 0; i < pop.Length; i ++) {
						pop [i].score = -(pop [i].endLocation - pop [i].startLocation);
						//Debug.Log(pop[i].score);
				}

				

				return pop;

		}

		bluePrint [] crossOver (bluePrint[] leaders)
		{
				//Debug.Log ("crossover call");
				ArrayList combinations = new ArrayList ();
				bluePrint[] children = new bluePrint[populationSize];
				int i = 0;
				int topleaders = (int) (populationSize * 0.01);
				float leaderVal = leaders [populationSize - 1].score;
				bluePrint parent1 = null, parent2 = null;
				bool parent1Selected = false, parent2Selected = false;
				
		for (int leads = populationSize-1; leads > populationSize - topleaders-1; leads--) {
			children[i] = leaders[leads];
			i++;
				}
				//int count = 0;

				while (i < populationSize) {
						while (!parent1Selected || !parent2Selected) {
								float selectionChance = UnityEngine.Random.Range (0.0f, 1.0f);
								int selectedBot = UnityEngine.Random.Range (0, populationSize);

								//	float normalVal = leaders [selectedBot].score / leaderVal;
								//count++;
								if (leaders [selectedBot].normalScore > selectionChance) {

//										if(parent1Selected){
//
//						foreach(string a in combinations){
//
//							if(a.Equals(parent1.ID + leaders[selectedBot])){
//
//								duplicate = true;
//							}
//
//
//
//										}
										//}

										if (!parent1Selected) {
												parent1 = leaders [selectedBot];
												//leaders[selectedBot].score = -100;
												parent1Selected = true;




										} else if (!combinations.Contains (parent1.ID + leaders [selectedBot].ID) && !parent1.ID.Equals (leaders [selectedBot].ID)) {
												parent2 = leaders [selectedBot];
												//leaders[selectedBot].score = -100;
												parent2Selected = true;
												//	Debug.Log(parent1.ID + parent2.ID);
												//	Debug.Log(parent2.ID + parent1.ID);
												combinations.Add (parent1.ID + parent2.ID);
												combinations.Add (parent2.ID + parent1.ID);


												//Debug.Log("p1 " + parent1.chromosomes[0]);
												//Debug.Log("p2 " + parent2.chromosomes[0]);
												//	Debug.Log("it took me: " + count + " tries to find the parents");
												//	count = 0;
												bluePrint[] newbies = breed (parent1, parent2);
												//Debug.Log(newbies.Length);

												for (int j = 0; j < newbies.Length && i < populationSize; j ++) {
														//	Debug.Log("newbiechrom: " +newbies[j].chromosomes[0]);
														children [i] = newbies [j];
														children [i].ID = "B" + i + "G" + currentGeneration;
														i ++;

												}



										} else {
												//Debug.Log ("parent combination already exists or same parents were selected");
										}


								}


						}
						parent1Selected = false;
						parent2Selected = false;

				}

	

				//	Debug.Log ("howmanychildren:" + i);
				return children;
		}

		bluePrint[] breed (bluePrint a, bluePrint b)
		{
				//	Debug.Log ("breed call");

				bluePrint[] children = new bluePrint[2];
				String [] aChroms = a.chromosomes;
				String [] bChroms = b.chromosomes;
				int chromA = UnityEngine.Random.Range (0, numberOfUnits);
				int chromB = UnityEngine.Random.Range (0, numberOfUnits);



				String[] newChromsA = swapChrom (aChroms, bChroms, chromA, chromB);
				//Debug.Log ("swappin B:");


				String[] newChromsB = swapChrom (bChroms, aChroms, chromB, chromA);




				children [0] = new bluePrint ("blank", newChromsA);

				children [1] = new bluePrint ("blank", newChromsB);

				//children [0] = new bluePrint ("blank", a.chromosomes);

				//children [1] = new bluePrint ("blank", b.chromosomes);


//		}
				return children;
		}

		String[] swapChrom (String[] a, String[] b, int indexA, int indexB)
		{
				string[] returnChrom = new string[a.Length];
				for (int i = 0; i < returnChrom.Length; i ++) {
						returnChrom [i] = a [i];
				}
				//Debug.Log ("swap call");

//		Debug.Log ("chroms before:");
//		foreach (string alpha in returnChrom) {
//			Debug.Log(alpha);
//		}

				string chrom1;
				string chrom2;
				if (indexA == 0) {
						chrom1 = returnChrom [indexA];
				} else {
						chrom1 = returnChrom [indexA - 1];
				}
				if (indexB == 0) {

						chrom2 = b [indexB];
				} else {
						chrom2 = b [indexB - 1];
				}
				String[] aChrom = chrom1.Split (',');
				String[] bChrom = chrom2.Split (',');
				int selectionIndexA = 2;
				int selectionIndexB = 2;
				if (indexA > 0) {
						selectionIndexA = 3;
				}
				if (indexB > 0) {
						selectionIndexB = 3;
				}

				aChrom [selectionIndexA] = bChrom [selectionIndexB];

				string buildStringA = "";
				//	string buildStringB = "";
				for (int i = 0; i < aChrom.Length-1; i ++) {
						buildStringA = buildStringA + aChrom [i] + ",";
						//	 buildStringB = buildStringB + bChrom[i] + ",";
				}
				buildStringA += aChrom [aChrom.Length - 1];
				//buildStringB += bChrom [bChrom.Length - 1];
				if (indexA == 0) {
						returnChrom [indexA] = buildStringA;
				} else {
						returnChrom [indexA - 1] = buildStringA;
				}
				//b [index] = buildStringB;
//		Debug.Log ("chroms after:");
//		foreach (string alpha in returnChrom) {
//					Debug.Log(alpha);
//				}





				return returnChrom;
		}

		void generateInitialPopulation ()
		{

				//			int initialStrutID = 1000;


				// Use this for initialization

				// int identityStrut = initialStrutID;
				//			int currentStrut;

				for (int i = 0; i < populationSize; i ++) {
						bluePrint robot;
						string[] chromosomes = new string[numberOfUnits - 1];
						string ID = "B" + i;
						//sr.WriteLine (ID);
//						string[] IDList = new string[numberOfUnits];
						//		int originID = 5000;
						robot = new bluePrint (ID);
						int originUnitType = UnityEngine.Random.Range (0, maxValueOfUnit + 1);

						functionalUnit origin = new functionalUnit ("5000", new point3 (0, 0, 0));
						robot.unitArray.Add (origin);
						for (int j = 0; j < numberOfUnits-1; j ++) {

								int id = 5001 + j;

								int side = UnityEngine.Random.Range (0, 6);
								int connectObject = 5000 + UnityEngine.Random.Range (0, j + 1);
								functionalUnit con = robot.getUnit ("" + connectObject);
								point3 newPoint = translatePoint (con.location, side);
								while (robot.unitAreaOccupied(newPoint)) {
										side = UnityEngine.Random.Range (0, 6);
										connectObject = 5000 + UnityEngine.Random.Range (0, j + 1);
										con = robot.getUnit ("" + connectObject);
										newPoint = translatePoint (con.location, side);

								}
								robot.unitArray.Add (new functionalUnit ("" + id, newPoint));

								int unitType = UnityEngine.Random.Range (0, maxValueOfUnit + 1);
								string chromosome = connectObject + "," + id + "," + originUnitType + "," + unitType + "," + side;
								chromosomes [j] = chromosome;


						}
						robot.chromosomes = chromosomes;
						currentPopulation [i] = robot;

				}


		}

		public point3 translatePoint (point3 point, int side)
		{
				switch (side) {
				case 0:
						return new point3 (point.x, point.y + 1, point.z);
				case 1:
						return new point3 (point.x, point.y - 1, point.z);
				case 2:
						return new point3 (point.x, point.y, point.z + 1);
				case 3:
						return new point3 (point.x + 1, point.y, point.z);
				case 4:
						return new point3 (point.x, point.y, point.z - 1);
				case 5:
						return new point3 (point.x - 1, point.y, point.z);
				default:
						return null;

				}

		}

		void Start ()
		{

				Time.timeScale = timeScale;
				robotsInScene = new bluePrint[unitsPerRun];
				terrainInScene = new GameObject[unitsPerRun];

				currentPopulation = new bluePrint[populationSize];
				generateInitialPopulation ();
				//			GameObject controller = GameObject.Find ("Controller");
				//Architect archy = (Architect)controller.GetComponent (typeof(Architect));
				timer = Time.time;


		}

		bluePrint[] merge (bluePrint[] left, bluePrint[] right)
		{
				bluePrint [] result = new bluePrint[left.Length + right.Length];
				int leftPointer = 0, rightPointer = 0, returnPointer = 0;

				while (leftPointer < left.Length || rightPointer < right.Length) {
						if (leftPointer < left.Length && rightPointer < right.Length) {
								if (left [leftPointer].score <= right [rightPointer].score) {
										result [returnPointer] = left [leftPointer];
										returnPointer ++;
										leftPointer++;
								} else {
										result [returnPointer] = right [rightPointer];
										returnPointer++;
										rightPointer++;
								}


						} else if (leftPointer < left.Length) {
								result [returnPointer] = left [leftPointer];
								returnPointer ++;
								leftPointer++;

						} else if (rightPointer < right.Length) {
								result [returnPointer] = right [rightPointer];
								returnPointer++;
								rightPointer++;

						}





				}

				return result;


		}

		bluePrint[] mergeSort (bluePrint[] a)
		{

				if (a.Length <= 1) {
						return a;

				}

				int middle = a.Length / 2;
				bluePrint[] left = new bluePrint[middle];
				bluePrint[] right = new bluePrint[a.Length - middle];

				for (int i = 0, j = 0; i < a.Length; i ++) {
						if (i < middle) {
								left [i] = a [i];
						} else {
								right [j] = a [i];
								j++;
						}

				}
				left = mergeSort (left);
				right = mergeSort (right);

				return merge (left, right);

		}

		// Update is called once per frame
		void Update ()
		{


				if (!simulationComplete) {
						if (Time.time >= timer && !runComplete) {

								clearScene ();
								//Debug.Log("clear0");
								generateRun ("terrainFlat");
								timer = Time.time + runTime;
								if (iteration >= populationSize) {
										runComplete = true;
										//Debug.Log("run complete0");
								}

						}
						if (runComplete && Time.time >= timer) {
								Debug.Log ("run complete");

								clearScene ();
								bluePrint [] pop = evalPopulation (currentPopulation);
								currentPopulation = mergeSort (pop);
								normalizeScores ();
								printInfo ();
								if (currentGeneration < generations) {
										bluePrint[] newpop = crossOver (currentPopulation);
										currentPopulation = newpop;
										//	Debug.Log("Generation: " + currentGeneration);
										currentGeneration++;
										runComplete = false;
										iteration = 0;
										robotsInScene = new bluePrint[unitsPerRun];

								}
								if (currentGeneration == generations){
										simulationComplete = true;
										writeGenerationToFile();
				}
						}


				}
		}
	void writeGenerationToFile(){
				FILE_NAME = "finalGeneration " + populationSize+ " - " + generations + ".txt";
				if (File.Exists (FILE_NAME)) {
						Debug.Log ("File already exists. Delete it or something.");
						return;
				}
				StreamWriter sr = File.CreateText (FILE_NAME);


		for (int i = 0; i < currentPopulation.Length; i ++) {
						sr.WriteLine (currentPopulation [i].ID);
						for (int j = 0; j < currentPopulation[i].chromosomes.Length; j ++) {
								sr.WriteLine (currentPopulation [i].chromosomes [j]);
						}
				}

		sr.Close ();
		
		
		
	}
	public void printInfo ()
		{
				Debug.Log ("Best raw score Of Generation " + currentGeneration + " -> " + currentPopulation [currentPopulation.Length - 1].score);
				Debug.Log ("Worst raw score Of Generation " + currentGeneration + " -> " + currentPopulation [0].score);
				Debug.Log ("Average raw score of Generation " + currentGeneration + " -> " + rawMean ());
				Debug.Log ("Average normalized score of Generation " + currentGeneration + " -> " + normalMean ());
				Debug.Log ("Standard deviation from " + rawMean () + " is " + standardDev ());
	}

		public float rawMean ()
		{
				float mean = 0;
				foreach (bluePrint a in currentPopulation) {
						mean += a.score;
				}
				return mean / currentPopulation.Length;

		}

		public float normalMean ()
		{

				float mean = 0;
				foreach (bluePrint a in currentPopulation) {
						mean += a.normalScore;
				}
				return mean / currentPopulation.Length;

		}

		public float standardDev ()
		{
				float tot = 0;
				foreach (bluePrint a in currentPopulation) {
						tot += Mathf.Pow (a.score - runMean, 2);
				}
				return Mathf.Sqrt (tot / currentPopulation.Length);
		}

		public void clearScene ()
		{

				for (int i = 0; i < robotsInScene.Length; i ++) {
						if (robotsInScene [i] != null && terrainInScene [i] != null) {
								robotsInScene [i].endLocation = robotsInScene [i].trackerObject.transform.position.x;

								Destroy (robotsInScene [i].instantiation);
								Destroy (terrainInScene [i]);
						}
				}
		}

		public void normalizeScores ()
		{
				float highval = currentPopulation [currentPopulation.Length - 1].score;
				float lowval = Mathf.Abs (currentPopulation [0].score);
				highval += lowval;
				foreach (bluePrint a in currentPopulation) {
						a.normalScore = a.score;
						a.normalScore += lowval;
						a.normalScore = a.normalScore / highval;
				}
		}
}
