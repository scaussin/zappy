﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player movement. Each Player object handles its own movement, but none of them
/// activate their methods by theirselves.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
	public bool				IsAdvancing;
	public bool				IsRotating;
	public Vector3			TargetPos;
	public Vector3			TargetRotation;
	private Quaternion		TargetRotationQuat;

	// Private setters variables.
	private Vector3			ObjectEulerAngle;
	private int				dir;
	private float			AdvanceDistance;

	private float			moveStartTime;
	private float			timeSinceStarted;
	private float			fracComplete;
	private float			timeUnit = 1.0f;
	private float			advanceDelay = 7.0f;

	private PlayerObject	playerObjInstance;
	private Animator		playerAnimator;
	private GameObject		boardZeroPoint;

	// Init vars at spawn.
	void OnEnable()
	{
		AdvanceDistance = (float)GameManager.instance.WorldSettings.BlockSize
						+ GameManager.instance.WorldSettings.BlockSpacing;
		playerObjInstance = GetComponent<PlayerObject> ();
		playerAnimator =  GetComponent<Animator> ();
		boardZeroPoint = transform.parent.transform.parent.GetComponent<ActorSpawner> ().BoardZeroPoint;
		timeUnit = GameManager.instance.WorldSettings.TimeUnit;
	}

	void FixedUpdate ()
	{
		if (IsAdvancing)
		{
			playerAnimator.SetBool ("IsMoving", true);
			timeSinceStarted = Time.time - moveStartTime;
			fracComplete = timeSinceStarted / (advanceDelay * timeUnit);
			//transform.position = TargetPos;
			transform.position = Vector3.Lerp (transform.position, TargetPos, fracComplete);
			if (Vector3.Distance (transform.position, TargetPos) < 0.02f)
			{
				IsAdvancing = false;
				playerAnimator.SetBool ("IsMoving", false);
			}
		}
		if (IsRotating)
		{
			timeSinceStarted = Time.time - moveStartTime;
			fracComplete = timeSinceStarted / (advanceDelay * timeUnit);
			TargetRotationQuat = Quaternion.Euler (TargetRotation);
			transform.localRotation = Quaternion.Lerp (transform.localRotation, TargetRotationQuat, fracComplete);
			if (Vector3.Distance (transform.localEulerAngles, TargetRotation) < 0.02f) {
				IsRotating = false;
			}
		}
	}

	/// <summary>
	/// Initiate the movement. The public function that will make the player move
	/// smoothly to its target.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="dir">Dir.</param>
	public void StartMovement(int x, int y, int dir)
	{
		moveStartTime = Time.time;
		SetTargetWorldPos (x, y, dir);
		IsAdvancing = true;
		IsRotating = false;
	}

	/// <summary>
	/// Initiate the smooth rotation.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="dir">Dir.</param>
	public void StartRotation(int x, int y, int dir)
	{
		moveStartTime = Time.time;
		SetTargetWorldRot (x, y, dir);
		IsAdvancing = false;
		IsRotating = true;
	}

	/// <summary>
	/// Teleport the player to the specified x, y and dir. This method makes an instant
	/// displacement. Useful for map boundary teleportation.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="dir">Dir.</param>
	public void Teleport(int x, int y, int dir)
	{
		TargetPos = boardZeroPoint.transform.position;

		TargetPos.x += (GameManager.instance.WorldSettings.BlockSize +
			GameManager.instance.WorldSettings.BlockSpacing) * x;
		TargetPos.z += (GameManager.instance.WorldSettings.BlockSize +
			GameManager.instance.WorldSettings.BlockSpacing) * y;
		
		transform.position = TargetPos;

	}

	/// <summary>
	/// Update the orientation of the player model from its Dir variable.
	/// Orientation (N:1, E:2, S:3, O:4)
	/// For the moment, the rotation is in euler (unrecommended);
	/// This sets the orientation instantly. Another method does it with a transition.
	/// </summary>
	public void	UpdateOrientation()
	{
		dir = playerObjInstance.Dir;
		ObjectEulerAngle = transform.localEulerAngles;
		if (dir == 1)
		{
			ObjectEulerAngle.y = -90;
		}
		else if (dir == 2)
		{
			ObjectEulerAngle.y = 0;
		}
		else if (dir == 3)
		{
			ObjectEulerAngle.y = 90;
		}
		else if (dir == 4)
		{
			ObjectEulerAngle.y = 180;
		}
		transform.localEulerAngles = ObjectEulerAngle;
	}

	/// <summary>
	/// Sets the target world position. From the server's XY map position, set a XYZ unity 3d position.
	/// Remember: Y server == Z unity.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="dir">Dir.</param>
	private void SetTargetWorldPos(int x, int y, int dir)
	{
		// TargetPos = transform.forward + new Vector3(AdvanceDistance, 0.0f, AdvanceDistance);
		TargetPos = transform.position;

		// If new val == old val, ne rien faire.
		// Set x target position;
		if (x > playerObjInstance.X)
			TargetPos.x += AdvanceDistance;
		else if (x < playerObjInstance.X)
			TargetPos.x -= AdvanceDistance;

		// Set y target position;
		if (y > playerObjInstance.Y)
			TargetPos.z += AdvanceDistance;
		else if (y < playerObjInstance.Y)
			TargetPos.z -= AdvanceDistance;
	}

	/// <summary>
	/// Sets the target world rotation for the gauche and droite cmds.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="dir">Dir.</param>
	private void SetTargetWorldRot(int x, int y, int dir)
	{
		TargetRotation = transform.localEulerAngles;
		if (dir == 1)
		{
			TargetRotation.y = -90;
		}
		else if (dir == 2)
		{
			TargetRotation.y = 0;
		}
		else if (dir == 3)
		{
			TargetRotation.y = 90;
		}
		else if (dir == 4)
		{
			TargetRotation.y = 180;
		}
	}
}