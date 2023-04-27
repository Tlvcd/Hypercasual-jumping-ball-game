using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] bool RequireForce,Destroy;
    [SerializeField] float ForceReq,AddSpeed;
    [SerializeField] int ComboPoints, points;
    [SerializeField] GameObject explosion;
    [SerializeField] AudioClip sound1, sound2;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
            PlayerController player = other.GetComponent<PlayerController>();
            if ((RequireForce && player.SlamStrength() > ForceReq) || (!RequireForce))
            {
                
                player.ModifySpeed(AddSpeed);
                player.ModifyPoints(points);
                Instantiate(explosion,transform.position,transform.rotation);
                player.Combo(ComboPoints);
            if (RequireForce) player.PlaySound(sound1);
            else player.PlaySound(sound2);
            if ( Destroy )Destroy(gameObject);

            }
    }
}
