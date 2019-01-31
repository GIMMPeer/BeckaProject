using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessAisle : MonoBehaviour {
	public GameObject player; //set the player
	private Transform playerLocation; //idenitfy the player's location in game
	private float startZ; //player start depth
	private float endZ; // player end depth
	private float startTeleTarget; //start location player teleportation trigger target depth
	private float endTeleTarget; //end location player teleportation target trigger depth
	private Vector3 resetStartLocation; //used to reset player location

	void Start () {
		//assignments
		startZ = player.transform.position.z;
		startTeleTarget = 34.5f;
		resetStartLocation.Set(0, 0, 0);
		endZ = -20.0f;
		endTeleTarget = -20.5f;
	}
	
	// Update is called once per frame
	void Update () {
		playerLocation = player.transform; //updating player location in realtime
		
		if (playerLocation.position.z < endTeleTarget) //the value we are evaluating against may need to be adjusted based on level design
		{
			//we only want to reset Z depth so we adapt to player's current position for X and Y
			resetStartLocation.x = playerLocation.position.x;
			resetStartLocation.y = playerLocation.position.y;
			resetStartLocation.z = startZ;
			player.transform.position = resetStartLocation; //Move the player to start
		} 
		else if (playerLocation.position.z > startTeleTarget) 
		{
			resetStartLocation.x = playerLocation.position.x;
			resetStartLocation.y = playerLocation.position.y;
			resetStartLocation.z = endZ;
			player.transform.position = resetStartLocation; //Move the player to end
		}
	}
}
