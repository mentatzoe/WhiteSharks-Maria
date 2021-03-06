﻿using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	playerScript player;

	// Can get these values from Box Collider 2D
	private float mapX = 27.31f;
	private float mapY = 12.0f;
	//private float halfX = 27.31f / 2.0f;
	private float quarterX = 27.31f / 4.0f;

//	public Transform leftmostObject;
//	public Transform rightmostObject;
	private float minX;
	private float maxX;
	private float minY;
	private float maxY;

	private float bounds = .1f;

	// Use this for initialization
	void Start () {

		float vertExtent = Camera.main.camera.orthographicSize;
		float horzExtent = vertExtent * Screen.width / Screen.height;

		// Calcs assume map is positioned at the origin
		minX = horzExtent - mapX / 2.0f;
		maxX = mapX / 2.0f - horzExtent;
		minY = vertExtent - mapY / 2.0f;
		maxY = mapY / 2.0f - vertExtent;
		SoundManager.Instance.MoveSpeaker(Camera.main);
	}

	void LateUpdate() {
		// Can't put this in start because start is run before player instantiated
		if (player == null) {
			player = (playerScript)FindObjectOfType(typeof(playerScript));
		}

		Vector3 playerPos = new Vector3 (0, 0, -10);
			if (player != null)
				playerPos = player.transform.position;
		Vector3 nudgeVal = transform.position;
		// negative
		if (playerPos.x < 0 && Mathf.Abs(playerPos.x) > quarterX * bounds) {
			//nudgeVal.x += -0.05f;
			nudgeVal.x = Mathf.Clamp(nudgeVal.x - 0.1f, -quarterX, quarterX);
			transform.position = nudgeVal;
		}
		else if (playerPos.x > 0 && Mathf.Abs (playerPos.x) > quarterX * bounds) {
			//nudgeVal.x += 0.05f;
			nudgeVal.x = Mathf.Clamp (nudgeVal.x + 0.1f, -quarterX, quarterX);
			transform.position = nudgeVal;
		}
	
		// Prevents camera from going through map bounds
		Vector3 v3 = transform.position;
		v3.x = Mathf.Clamp (v3.x, minX, maxX);
		v3.y = Mathf.Clamp (v3.y, minY, maxY);
		transform.position = v3;
		SoundManager.Instance.MoveSpeaker(Camera.main);
	}
}