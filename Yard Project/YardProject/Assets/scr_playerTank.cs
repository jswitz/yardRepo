using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_playerTank : MonoBehaviour
{

    public scr_playerTank myPlayersFlock; //this script
    public GameObject[] playerPrefabs; //player prefabs
    public Vector3 movementLimits = new Vector3(5, 0, 5); //tank size
    public int numPlayers = 2; //number of players spawned
    public GameObject[] allPlayers;
    public Vector3 goalPos; //initially will be the centre; 0
    private Vector3 movementLimitsCentre = new Vector3 (0, 0, 0);
    public Vector3 movementGoalPos;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f); //red 0.5alpha
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y +5, transform.position.z), new Vector3(movementLimits.x * 2, movementLimits.y * 2, movementLimits.z * 2)); //draw red cube indicator
        Gizmos.color = new Color(0, 1, 0, 1); //goal pos drawn in green
        Gizmos.DrawSphere(goalPos, 0.1f); //draw green sphere indicator
    }

    private void Awake()
    {
        goalPos = transform.position; //draws goal pos at centre on awake
        movementGoalPos = goalPos;
    }

    private void Start()
    {

        myPlayersFlock = this;
        goalPos = this.transform.position;
        for (int i = 0; i < numPlayers; i++)
        {
            //Debug.Log(i);
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-movementLimits.x, movementLimits.x),
                                                                Random.Range(-movementLimits.y, movementLimits.y),
                                                                Random.Range(-movementLimits.z, movementLimits.z)); //spawn at random within tank
            allPlayers[i] = (GameObject)Instantiate(playerPrefabs[i], pos, Quaternion.identity);
            allPlayers[i].GetComponent<scr_movePlayers>().playerTankScript = this;
            
        }
    }

    void Update()
    {
        movementLimitsCentre = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z); //set variable for goal pos updater Y
        if (Random.Range(0, 10000) < 5) //randomly choose new goal pos within specified chance
        {
            goalPos = movementLimitsCentre + new Vector3(Random.Range(-movementLimits.x, movementLimits.x),
                                                            Random.Range(-movementLimits.y, movementLimits.y),
                                                            Random.Range(-movementLimits.z, movementLimits.z));
            //print(goalPos);
        }

        //Vector3 position = new Vector3(goalPos.transform.position.x, transform.position.y, transform.position.z);

        RaycastHit hit;

        if (Physics.Raycast(goalPos, Vector3.down, out hit))
        {
            Debug.DrawLine(goalPos, hit.point, Color.green);
            if (hit.collider.tag == "Ground")
            {
                movementGoalPos = hit.point;
                //Debug.Log(movementGoalPos);
            }
        }
    }
}
