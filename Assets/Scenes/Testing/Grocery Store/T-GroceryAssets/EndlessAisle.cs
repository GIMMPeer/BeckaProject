using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessAisle : MonoBehaviour {
	public GameObject player; //set the player
	private Transform playerLocation; //idenitfy the player's location in game
	private float startZ; //dynamically assign player start depth
	private Vector3 resetStartLocation; //used to reset player location

	void Start () {
		//assignments
		startZ = player.transform.position.z;
		resetStartLocation.Set(0, 0, startZ);
	}
	
	// Update is called once per frame
	void Update () {
		playerLocation = player.transform; //updating player location in realtime
		
		if (playerLocation.position.z < -20) //the value we are evaluating against may need to be adjusted based on level design
		{
			//we only want to reset Z depth so we adapt to player's current position for X and Y
			resetStartLocation.x = playerLocation.position.x;
			resetStartLocation.y = playerLocation.position.y;
			player.transform.position = resetStartLocation; //Move the player back to start
		}
	}
}
