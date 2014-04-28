using UnityEngine;
using System.Collections;

public class genericScene : MonoBehaviour {
	//Implement when dialoguer/animation ends, go to first scene.

	public string debugMe;
	public AudioClip playMe;
	public float waitThisLong;
	public string whatCharacter;
	public string nextLevel;
	public bool isTherePlayer;
	public Vector2 spawnHereAfter;
	public string dialoguer = "chapter1";

	private bool done = false;
	public bool needGUI = false;
	public Convo[] dialogue;
	private int curDia = 0;

	public int setOffset;
	
	// Use this for initialization
	void Start () {
		GameManager.dialogueJustFinished = false;
		if (GameManager.offset == 0)
			GameManager.offset = setOffset;

		if (Dialoguer.isInitialized ())
			Dialoguer.StartDialogue ((int)dialogue[curDia]);
		else {
			
			Dialoguer.Initialize(dialoguer);
			Dialoguer.StartDialogue ((int)dialogue[curDia]);
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine ("wait");
		Debug.Log ("ALIVE");

	}

	void OnGUI() {
		if (needGUI) {
			GUI.Box (new Rect (0, 0, Screen.width / 2, Screen.height / 2), "DIALOGUER CONVERSATION FOR BRAIN ROOM.");
			if (done) {
					GUI.Box (new Rect (0, 0, Screen.width / 2, Screen.height / 2), "DONE.");
			}
		}
	}

	IEnumerator wait(){
		SoundManager.Instance.Play2DMusic(playMe);
		//Debug.Log (debugMe);
		if (GameManager.dialogueJustFinished && curDia < dialogue.Length - 1) {

			GameManager.dialogueJustFinished = false;
			curDia ++;
			Dialoguer.StartDialogue((int)dialogue[curDia]);

		} else 
		if ((int)dialogue[curDia] >= 0 && GameManager.dialogueJustFinished && curDia == dialogue.Length - 1) {
			yield return new WaitForSeconds (waitThisLong);
			GameManager.Instance.playerInScene = isTherePlayer;
			done = true;
			if (isTherePlayer) {
					GameManager.Instance.SetMainCharacter (whatCharacter);
					GameManager.Instance.SetNextX (spawnHereAfter.x);
					GameManager.Instance.SetNextX (spawnHereAfter.y);
			}
			GameManager.dialogueJustFinished = false;
			Application.LoadLevel (nextLevel);
		}
	}
}
