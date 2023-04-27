using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finishZone : MonoBehaviour
{
    [SerializeField] WinZone Win;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Win.Win();
    }
}
