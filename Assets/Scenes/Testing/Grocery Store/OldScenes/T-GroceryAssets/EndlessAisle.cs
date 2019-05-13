using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO change name from Z to X
public class EndlessAisle : MonoBehaviour
{

    public GameObject player; //set the player
    private Transform playerLocation; //idenitfy the player's location in game
    private float startZ; //player start depth
    private float endZ; // player end depth
    private float startTeleTarget; //start location player teleportation trigger target depth
    private float endTeleTarget; //end location player teleportation target trigger depth
    private Vector3 resetStartLocation; //used to reset player location

    void Start()
    {
        //assignments
        startZ = 20;
        startTeleTarget = 21.0f; //in world space
        resetStartLocation.Set(0, 0, 0);
        endZ = 16.2f;
        endTeleTarget = 15.9f;
    }

    // Update is called once per frame
    void Update()
    {
        playerLocation = player.transform; //updating player location in realtime

        if (playerLocation.position.x < endTeleTarget) //the value we are evaluating against may need to be adjusted based on level design
        {
            //we only want to reset Z depth so we adapt to player's current position for X and Y
            resetStartLocation.x = startZ;
            resetStartLocation.y = playerLocation.position.y;
            resetStartLocation.z = playerLocation.position.z;
            player.transform.position = resetStartLocation; //Move the player to start
        }
        else if (playerLocation.position.x > startTeleTarget)
        {
            resetStartLocation.x = endZ;
            resetStartLocation.y = playerLocation.position.y;
            resetStartLocation.z = playerLocation.position.z;
            player.transform.position = resetStartLocation; //Move the player to end
        }
    }

    public void SetEndValue(float val)
    {
        endTeleTarget = val;
        endZ = val + 0.5f;
    }
}
