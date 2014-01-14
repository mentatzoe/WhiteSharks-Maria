﻿/*
npc class. 

changes: added collision with the player. -John Mai

*/
using UnityEngine;
using System.Collections;

public class npcScript : MonoBehaviour {
	
	public GameObject conversation, playerObj;

	//enable conversation object if left mouse button is clicked.
	void OnMouseDown(){
		if(Input.GetMouseButton(0)){
			conversation.renderer.enabled = true;
			conversation.collider2D.enabled = true;
		}
	}
	//switch the displaying order of the npc. 
	void Update () {
		if (transform.position.y < playerObj.transform.position.y) {
			renderer.sortingLayerName= "foreground";
			renderer.sortingOrder = 2;
		}
		else{
			renderer.sortingLayerName= "middleground";
		}
	}
}
