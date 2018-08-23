using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class script : MonoBehaviour {

	public KMAudio Audio;
	public KMBombModule Module;
	public KMBombInfo Info;
	public KMSelectable LeftBtn, Submit, RightBtn;
	public TextMesh StageInd, Screen;
	public MeshRenderer Symbol1, Symbol2, Symbol3;
	public Material[] Symbols;

	//array of symbols to use for each flash entry via numerical id as given in flash-listings.txt
	private int[,] flashArray = new int[91,3] {{0, 13, 13}, {0, 8, 14}, {1, 13, 13}, {0, 15, 16}, {2, 13, 13}, {1, 8, 14}, {2, 3, 22}, {22, 16, 22}, {3, 13, 13}, {2, 8, 14}, {2, 8, 45}, {3, 8, 14}, {1, 9, 9}, {3, 11, 14}, {3, 13, 18}, {2, 8, 10}, {1, 15, 16}, {24, 25, 26}, {0, 16, 23}, {1, 16, 16}, {2, 16, 16}, {24, 19, 1}, {1, 2, 13}, {17, 22, 24}, {0, 20, 31}, {2, 24, 3}, {35, 34, 28}, {31, 0, 16}, {3, 39, 19}, {3, 11, 24}, {0, 22, 17}, {28, 35, 29}, {37, 38, 35}, {0, 1, 24}, {34, 35, 14}, {34, 35, 37}, {37, 31, 14}, {0, 3, 24}, {22, 23, 24}, {1, 2, 20}, {24, 25, 14}, {3, 11, 17}, {3, 23, 27}, {27, 30, 15}, {1, 2, 23}, {27, 43, 26}, {16, 4, 41}, {31, 31, 31}, {23, 4, 18}, {5, 19, 1}, {7, 6, 19}, {4, 15, 16}, {4, 16, 20}, {4, 20, 12}, {20, 7, 3}, {6, 40, 5}, {6, 4, 7}, {41, 42, 43}, {40, 16, 16}, {40, 35, 41}, {7, 5, 6}, {7, 30, 38}, {36, 12, 6}, {4, 37, 21}, {0, 30, 38}, {42, 13, 13}, {3, 4, 42}, {42, 14, 14}, {0, 13, 16}, {0, 5, 32}, {42, 42, 13}, {44, 42, 10}, {35, 15, 15}, {36, 32, 45}, {34, 35, 43}, {14, 31, 43}, {0, 1, 33}, {7, 26, 14}, {4, 8, 9}, {2, 6, 34}, {43, 40, 14}, {17, 4, 14}, {3, 24, 23}, {8, 7, 26}, {2, 6, 24}, {5, 19, 14}, {17, 16, 16}, {41, 16, 43}, {35, 16, 43}, {42, 16, 14}, {16, 16, 44}};
	//corresponding song titles for each entry
	private string[] songArray = new string[91] {
		"Showtime (Piano Refrain)",
		"Showtime (Original Mix)",
		"Aggrieve (Violin Refrain)",
		"Sburban Countdown",
		"(Several Drum Tracks by\r\nDifferent Composers)",
		"Aggrieve",
		"Upward Movement\r\n(Dave Owns)",
		"Explore",
		"Gardener",
		"Beatdown (Strider Style)",
		"Beatdown Round 2",
		"Dissension (Original)",
		"Chorale for Jaspers",
		"An Unbreakable Union",
		"The Beginning of Something\r\nReally Excellent",
		"Versus",
		"Sburban Jungle",
		"Three in the Morning",
		"Doctor",
		"Endless Climb",
		"Atomyk Ebonpyre",
		"Black",
		"Unsheath'd",
		"Skaian Skirmish",
		"How Do I Live (Bunny\r\nBack in the Box Version)",
		"Descend",
		"Crystalmanthequins",
		"Homestuck",
		"Let the Squiddles Sleep\r\n(End Theme)",
		"Umbral Ultimatum",
		"Savior of the\r\nWaking World",
		"MeGaLoVania",
		"Trollian Standoff",
		"At the Price of Oblivion",
		"Terezi Owns",
		"BL1ND JUST1C3 :\r\n1NV3ST1G4T1ON !!",
		"The Carnival",
		"Cascade (Beta)",
		"Cascade (Beta)",
		"Flare",
		"Flare",
		"Savior of the\r\nDreaming Dead",
		"Savior of the\r\nDreaming Dead",
		"Black Hole /\r\nGreen Sun",
		"Black Hole /\r\nGreen Sun",
		"English",
		"Homestuck Anthem",
		"Frustracean",
		"Infinity Mechanism",
		"Even in Death",
		"Time on My Side",
		"Another Jungle",
		"Rain",
		"Ruins (With Strings)",
		"I Don't Want to Miss a Thing",
		"Unite Synchronization",
		"Unite Synchronization",
		"Eternity Served Cold",
		"Fuchsia Ruler",
		"Killed by BR8K Spider!!!!!!!!",
		"Even in Death\r\n(T'Morra's Belly Mix)",
		"A Taste for Adventure",
		"Horschestra\r\nSTRONG Version",
		"Trickster Mode",
		"A Taste for Adventure",
		"Homosuck Anthem",
		"Gold Pilot",
		"Carne Vale",
		"Pipeorgankind",
		"Elevatorstuck,\r\nwith Meows",
		"Homosuck Swan Song",
		"Hello Zepp",
		"Moonsetter",
		"Horsecatska",
		"Do You Remem8er Me",
		"Creata (Canon Edit)",
		"Oppa Toby Style",
		"Oppa Toby Style",
		"Oppa Toby Style",
		"Oppa Toby Style",
		"Eternity Served Cold\r\n(Canon Edit)",
		"Heir of Grief",
		"Heir of Grief",
		"Heir of Grief",
		"Heir of Grief",
		"Heir of Grief",
		"Overture (Canon Edit)",
		"Overture (Canon Edit)",
		"Overture (Canon Edit)",
		"Overture (Canon Edit)",
		"Ascend"
	};

	
	private static int _moduleIdCounter = 1;
	private int _moduleId = 0;
	private bool _isSolved = false, _lightsOn = false;
	private int stageNum = 1, randomFlashChoice = 0, totalStageNum = 3;
	private int[] stageSongArray = new int[7] {0, 0, 0, 0, 0, 0, 0};
	//currentSSAIndex tracks the index of the song displayed on the module in stageSongArray (SSA)
	private int currentSSAIndex = 0;


	// Use this for initialization
	void Start () {
		_moduleId = _moduleIdCounter++;
		Module.OnActivate += Activate;
		//set all symbols to john (material 0) momentarily before generating stage
		Symbol1.materials = new[] {Symbols[0]};
		Symbol2.materials = new[] {Symbols[0]};
		Symbol3.materials = new[] {Symbols[0]};
	}

	//room is shown but lights are not on
	private void Awake() {
		//buttons handling
		LeftBtn.OnInteract += delegate () {
			handleLeftBtn ();
			return false;
		};
		RightBtn.OnInteract += delegate () {
			handleRightBtn ();
			return false;
		};
		Submit.OnInteract += delegate () {
			handleSubmit ();
			return false;
		};
	}

	//bomb activates/lights turn on
	void Activate () {
		Init ();
		_lightsOn = true;
		StageInd.text = "STAGE 1 OF " + totalStageNum.ToString();
		generateStage ();
	}

	//module initialization
	void Init () {
		Debug.LogFormat ("[Homestuck Flashes #{0}] This module is initializing.", _moduleId);
	}

	//runs for each stage of module
	void generateStage() {
		Debug.LogFormat ("[Homestuck Flashes #{0}] Generating stage {1} of 3.", _moduleId, stageNum);
		randomFlashChoice = UnityEngine.Random.Range (0, songArray.Length);
		Debug.LogFormat ("[Homestuck Flashes #{0}] Variable randomFlashChoice is equal to {1}.", _moduleId, randomFlashChoice.ToString());

		//set symbols on module
		int FARFC0 = flashArray [randomFlashChoice, 0];
		int FARFC1 = flashArray [randomFlashChoice, 1];
		int FARFC2 = flashArray [randomFlashChoice, 2];
		Symbol1.material = Symbols [FARFC0];
		Symbol2.material = Symbols [FARFC1];
		Symbol3.material = Symbols [FARFC2];

		//set songs to display
		stageSongArray[0] = randomFlashChoice;
		int counter = 1;

		//in the event the while loop runs 1,000 times, set stageSongArray to {randomFlashChoice,1,2,3,4,5,6}
		int timeOutCounter = 1;
		while (counter < stageSongArray.Length) {
			int randomSongID = UnityEngine.Random.Range (0, songArray.Length);
			int forCounter = counter;

			//variable used to store whether or not the randomSongID chosen is already in stageSongArray
			bool randomSongIDEqual = false;
			for (int i = 0; i < forCounter; i++) {
				int j = i;
				if (songArray[randomSongID] == songArray[stageSongArray [j]]) {
					randomSongIDEqual = true;
					break;
				}
			}
			if (!randomSongIDEqual) {
				stageSongArray [forCounter] = randomSongID;
				counter++;
			}
			if (timeOutCounter == 1000) {
				for (int i = 1; i < stageSongArray.Length; i++) {
					int j = i;
					stageSongArray [j] = j;
				}
				counter = stageSongArray.Length;
			}
			timeOutCounter++;
		}

		//choose the index of which song to be displayed first on module
		currentSSAIndex = UnityEngine.Random.Range (0, stageSongArray.Length);
		int SSACSSAI = stageSongArray [currentSSAIndex];
		Screen.text = songArray[SSACSSAI];

		//output to logfile info on the current stage
		Debug.LogFormat ("[Homestuck Flashes #{0}] Correct song is {1}. Displaying symbols {2}, {3}, and {4}." , _moduleId, songArray[stageSongArray[0]], Symbol1.material.name, Symbol2.material.name, Symbol3.material.name);
	}

	//buttons handling
	//if (currentSSAIndex ...) in handleLeftBtn() and handleRightBtn() test for if the index value is at the extremem ends of stageSongArray to avoid index errors (attempting to find the value of indices -1 and stageSongArray.Length)
	void handleLeftBtn() {
		Audio.PlayGameSoundAtTransform (KMSoundOverride.SoundEffect.ButtonPress, LeftBtn.transform);
		LeftBtn.AddInteractionPunch ();

		if (!_lightsOn || _isSolved) {
			return;
		}

		if (currentSSAIndex == 0) {
			currentSSAIndex = stageSongArray.Length - 1;
			Screen.text = songArray [stageSongArray [currentSSAIndex]];
		} else {
			currentSSAIndex = currentSSAIndex - 1;
			Screen.text = songArray [stageSongArray [currentSSAIndex]];
		}
	}

	void handleRightBtn() {
		Audio.PlayGameSoundAtTransform (KMSoundOverride.SoundEffect.ButtonPress, RightBtn.transform);
		RightBtn.AddInteractionPunch ();      

		if (!_lightsOn || _isSolved) {
			return;
		}

		if (currentSSAIndex == stageSongArray.Length - 1) {
			currentSSAIndex = 0;
			Screen.text = songArray [stageSongArray [currentSSAIndex]];
		} else {
			currentSSAIndex = currentSSAIndex + 1;
			Screen.text = songArray [stageSongArray [currentSSAIndex]];
		}
	}

	void handleSubmit() {
		Debug.LogFormat ("[Homestuck Flashes #{0}] Checking submission, {1}.", _moduleId, songArray [stageSongArray [currentSSAIndex]]);
		Audio.PlayGameSoundAtTransform (KMSoundOverride.SoundEffect.ButtonPress, Submit.transform);
		Submit.AddInteractionPunch ();

		if (!_lightsOn || _isSolved) {
			return;
		}

		if (currentSSAIndex == 0) {
			Debug.LogFormat ("[Homestuck Flashes #{0}] Stage is complete.", _moduleId);
			if (stageNum == totalStageNum) {
				_isSolved = true;
				Module.HandlePass ();
				StageInd.text = "DEFUSED";
				Debug.LogFormat ("[Homestuck Flashes #{0}] Module is defused.", _moduleId);
			} else {
				int newStageNum = stageNum + 1;
				StageInd.text = "STAGE " + newStageNum.ToString () +" OF " + totalStageNum.ToString ();
				generateStage ();
			}
			stageNum++;
		} else {
			Debug.LogFormat ("[Homestuck Flashes #{0}] The inputted answer is incorrect.", _moduleId);
			Module.HandleStrike ();
		}
	}
}
