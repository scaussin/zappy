﻿using UnityEngine;
using System.Collections;

public class PlayerObject : MonoBehaviour {
	[Header("Player Datas")]
	public int			AssignedNumber; // #n
	public int			X; // X
	public int			Y; // Y
	public int			Dir; // O
	public int			Level; // L
	public string		Team; // N

	public string		Inventory;

	[Header("Player points of interest")]
	public GameObject	ModelContainer;
	public GameObject	CameraPoint;



	// Use this for initialization. called just after it is spawned. Its datas are set.
	void Start ()
	{
		CameraPoint = transform.Find ("Points").transform.Find ("CameraPoint").gameObject;
		ModelContainer = transform.Find ("Model").gameObject;
		GetComponent<PlayerMovement> ().UpdateOrientation ();
	}


}