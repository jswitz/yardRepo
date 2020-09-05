using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_movePlayers : MonoBehaviour
{

    public scr_playerTank playerTankScript;
    public float speed = 0.5f;
    public float rotationSpeed = 20.0f;
    public float neighbourDistance = 4.0f;
    public GameObject playerObj;
    public GameObject[] players;
    
    void Start()
    {
        
    }

    void Update()
    {
        ApplyRules();
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = players;

        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        float dist;
        //Vector3 goalPos = playerObj.GetComponent<scr_playerTank>().movementGoalPos;
        Vector3 goalPos = playerTankScript.movementGoalPos;
        
                foreach (GameObject go in gos)
                { 
                    if(go != this.gameObject)
                    {
                        dist = Vector3.Distance(go.transform.position, this.transform.position);
                        if (dist <= neighbourDistance)
                        {
                            vAvoid = vAvoid + (this.transform.position - go.transform.position);
                            Debug.Log("avoided");
                        }
                    }
                }

                vCentre = vCentre + (goalPos - this.transform.position);
                //print(goalPos);
                Vector3 direction = (vCentre + vAvoid) - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      rotationSpeed * Time.deltaTime);
    }
}
