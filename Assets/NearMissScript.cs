using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearMissScript : MonoBehaviour
{
    public delegate void NearMiss();
    public static event NearMiss OnMiss;
    public static event NearMiss OnMissExit;
    public static event NearMiss OnWallHit;
    public static event NearMiss OnWallHitExit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacles")) OnMiss();
        else if (other.CompareTag("Wall")) OnWallHit();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacles")) OnMissExit();
        else if (other.CompareTag("Wall")) OnWallHitExit();
    }
}
