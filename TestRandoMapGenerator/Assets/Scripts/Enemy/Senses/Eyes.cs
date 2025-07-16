using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{


    [field: SerializeField] public float Fow { get; private set; } = 45;
    [field: SerializeField] public float EyeHeight { get; private set; } = 1.6f;

    [field: SerializeField] public float ViewingDistance { get; private set; } = 5;

    public LayerMask LayerMaskForRaycast;

    public bool CheckIsInView(Player player)
    {
        //Debug.Log("CheckPlayerInView" + gameObject.name);
        //create a Vector 3 as an direction vector between Mousey and Player
        //Vector3 eyePosittion = transform.position;
        //eyePosittion.y += EyeHeight;
        Vector3 eyePos = new Vector3(transform.position.x, transform.position.y + EyeHeight, transform.position.z);

        Vector3 playerPos = player.playerCenter.position;

        Vector3 vectorBetween = playerPos - eyePos;
        //calculate the angle
        float angle = Vector3.Angle(vectorBetween, transform.forward);

        //Check if the player is in Mouseys FieldOfView
        if (Fow > angle)
        {
            //Debug.Log("CheckPlayerInViewTrue");

            //Check if player can be seen
            if (playerHitByRaycast(player.gameObject, vectorBetween))
            {
                Debug.Log("isInView");
                return true;
            }

        }
        //Debug.Log("CheckPlayerInViewFalse");
        return false;

    }

    private bool playerHitByRaycast(GameObject player, Vector3 vectorToPlayer)
    {
        //Debug.Log("playerHitbyRaycasttest");

        //calculate Raycast origin 
        Vector3 rayCastOrigin = new Vector3(transform.position.x, transform.position.y + EyeHeight, transform.position.z);

        RaycastHit hit;


        if (Physics.Raycast(rayCastOrigin, vectorToPlayer, out hit, ViewingDistance, LayerMaskForRaycast))
        {
            if (hit.collider != null)
            {
                //Debug.Log("HitSomething");
                GameObject hitObject = hit.transform.gameObject;

                //check if the hit is the player
                if (hitObject.CompareTag("Player") || hit.transform.root.CompareTag("Player"))
                {
                    //Debug.Log("Hitplayer");
                    Debug.DrawRay(rayCastOrigin, (vectorToPlayer.normalized * hit.distance), Color.green);
                    return true;
                }
                else
                {
                    //Debug.Log("Ray hit: " + hit.collider.name + " (Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer) + ")");
                }

            }
        }
        Debug.DrawRay(rayCastOrigin, (vectorToPlayer.normalized * ViewingDistance), Color.red);

        return false;
    }
}
