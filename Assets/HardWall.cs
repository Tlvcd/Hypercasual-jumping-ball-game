using UnityEngine;

public class HardWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) other.GetComponent<PlayerController>().PlayerDeath();
            
    }
}
