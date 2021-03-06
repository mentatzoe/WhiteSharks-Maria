﻿// @author Anthony Lim
// Manages core gameplay elements such as scenes and states

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {
	// Declare properties


	//Variables for case control
	public static int currentEpisode = 0;

	public static bool dialogueJustFinished = false;
	public static bool firstTimeOffice = true;
	public static bool may = true;

	public bool playerInScene = false;
	private static GameManager instance;
	private string _currLevel;			// Current level
	private string _name;				// Character name
	public static List<string> roomList = new List<string>();
	public static List<NPC> npcList = new List<NPC>();
	public static List<NPC> witnessList = new List<NPC>();
	public static List<CaseObject> weaponList= new List<CaseObject>();
	public ArrayList roomIDList;
	public string currRoom;
	public int currentRoomIndex;
	private string currentMainCharacter;
	public static Case theCase = new Case(); //Generate this!
	public float nextX, nextY;
	public static NPC guilty;
	public static CaseObject weapon;
	public static string room;
	public float maxDist = 2f;

	//Shammy 0, Noel 1, Carlos 2
	public static int[] npcConversations = new int[7]{0, 0, 0, 0, 0, 0, 0};

	//Handles mouse cursor information
	public static int cursorSize = 32;
	public string defaultIcon = "Walk_Icon";		//The standard mouse icon when not hovering over an object
	public static List<Texture2D> mouseSprites;
	public static string[] spriteIndex;
	public static Texture2D currMouse;
	public static int offset = 0;
	public static int pickUpConvo = 36;


	public static GameManager Instance {
		get {
			if (instance == null) {
				Debug.Log("Instance null, creating new GameManager");
				instance = new GameObject("GameManager").AddComponent<GameManager>();
			}
			return instance;
		}
	}

	// Sets instance to null when the application quits
	public void OnApplicationQuit() {
		instance = null;
	}
	//set the next x value
	public void SetNextX(float x){
		nextX = x;
	}
	//return the nextX value
	public float GetNextX(){
		//Debug.Log ("Next X is " + nextX);
		return nextX;
	}
	//set the nextY value
	public void SetNextY(float y){
		nextY = y;
	}
	//return the nextY value
	public float GetNextY(){
		//Debug.Log ("Next Y is " + nextY);
		return nextY;
	}
	//set the main character that the player is currently playing
	public void SetMainCharacter(string main){
		Debug.Log ("Setting main character to " + main);
		currentMainCharacter = main;
	}
	//return the main character.
	public string GetMainCharacter(){
		return currentMainCharacter;
	}
	// get player script of main character
 	public playerScript getPlayerScript(){
 		return (playerScript)GameObject.Find (this.GetMainCharacter() + "(Clone)").GetComponent<playerScript> ();
 	}

	/// <summary>
	/// Starts the game state and sets initial values
	/// Should be called during gameStart
	/// </summary>
	public void startState(bool test_Mode, string sceneName) {

		Debug.Log("Creating a new game state");
		//CaseGenerator c = new CaseGenerator("","");
		//GameManager.setNPCS = 

		// Set default properties
		_currLevel = "Level 1";
		_name = "My Character";

		if (!test_Mode){
			// Load character select screen
			Application.LoadLevel ("chapter1intronarration");
		}
		else{
			GameManager.Instance.SetMainCharacter("Jane");
			GameManager.Instance.playerInScene = false;
			Application.LoadLevel(sceneName);
		}
	}

	/// <summary>
	/// Generates the case
	/// </summary>
//	public void generateCase() {
//		//Debug.Log (theCase.getRoom());
//		theCase = generator.generateCase();
//		//Debug.Log ("the case in GM " + guilty + " " + weapon + " " + room);
//		//Debug.Log (theCase.getRoom());
//	}

	/// <summary>
	/// Calculates the score
	/// </summary>
	/// <returns>The score.</returns>
	public int calculateScore() {
		// calcs score
		return 0;
	}

	/// <summary>
	/// Draws the score
	/// </summary>
	public void drawScore() {
		// draws the score on the screen
	}
	
	/// <summary>
	/// Quits the game
	/// </summary>
	public void quitGame() {
		Debug.Log("Qutting the game");
		Application.Quit ();
	}

	/// <summary>
	/// Get the current level
	/// </summary>
	/// <returns>The level.</returns>
	public string getLevel() {
		if (_currLevel != null)
			return _currLevel;
		else
			return "currLevel is null!";
	}

	/// <summary>
	/// Set the currLevel
	/// </summary>
	/// <param name="level">Level.</param>
	public void setLevel(string level) {
		_currLevel = level;
	}

	/// <summary>
	/// Gets the name
	/// </summary>
	/// <returns>The name.</returns>
	public string getName() {
		if (_name != null)
			return _name;
		else
			return "name is null!";
	}

	/// <summary>
	/// Sets the name
	/// </summary>
	/// <param name="newName">New name.</param>
	public void setName(string newName) {
		_name = newName;
	}

	public void OnGUI() {
//		GUI.Label (new Rect (Screen.width / 3.5f, Screen.height - 20f, Screen.width, 50f),
//		           "BETA - NOT REPRESENTATIVE OF FINAL GAMEPLAY");

		//Handle mouse updates here

		GUI.DrawTexture (new Rect (Input.mousePosition.x - 16, (Screen.height - Input.mousePosition.y) - cursorSize / 8,
		                           cursorSize, cursorSize), currMouse);


	}


	//Initialize the sprite array for the mouse to draw
	//Loads in from Sprites/Mouse Icons
	//Sets the initial icon to Walk
	void setIcons(){

		mouseSprites = new List<Texture2D>();
		Screen.showCursor = false;

		foreach (object o in Resources.LoadAll("MouseIcons", typeof(Texture2D))) {
			mouseSprites.Add(o as Texture2D);
		}

		spriteIndex = new string[mouseSprites.Count];
		
		for(int i=0; i< spriteIndex.Length; i++) {
			spriteIndex[i] = mouseSprites[i].name;
		}

		currMouse = (Texture2D) mouseSprites[Array.IndexOf(spriteIndex, "Walk_Icon")];
	}

	//Updates the current sprite when called by another game object
	//Takes in a string based on what kind of object it is that signifies the icon the cursor should be
	public void updateMouseIcon(string whichSprite){
		currMouse = (Texture2D)mouseSprites [Array.IndexOf (spriteIndex, whichSprite)];

		//print (currMouse.ToString () + " WHEEEE");
	}

	//Testing purposes
	void Start(){
		//start location
		nextX = -6.440672f;
		nextY = -5.890769f;
		//For a changing cursor, load in all of its sprites into the list
		setIcons ();

		// Used to create the Dialoguer system
		gameObject.AddComponent<DialogueGUI_Test> ();

		//dGUI.setSkin(Resources.Load ("OldSchool") as GUISkin);

		npcList.Add(Resources.Load<NPC>("LiamOShea"));
		npcList.Add(Resources.Load<NPC>("NinaWalker"));
		npcList.Add(Resources.Load<NPC>("JoshSusach"));
		witnessList.Add(Resources.Load<NPC>("NoelAlt"));
		witnessList.Add(Resources.Load<NPC>("PeijunShi"));
		witnessList.Add(Resources.Load<NPC>("CarlosFranco"));
		weaponList.Add(Resources.Load<CaseObject>("LaserPistol"));
		weaponList.Add(Resources.Load<CaseObject>("eSword"));
		weaponList.Add(Resources.Load<CaseObject>("MetalPipe"));
		weaponList.Add(Resources.Load<CaseObject>("RadioactiveIceCubes"));
		weaponList.Add(Resources.Load<CaseObject>("VSs"));

	}

	public void finishEpisode(){
		currentEpisode++;
	}

}

public enum gameStates {
	INTRO,
	MAINMENU,
	INGAME,
	JOURNAL,
	CONVERSATION
}