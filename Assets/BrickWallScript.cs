
using UnityEngine;
using TMPro;

public class BrickWallScript : MonoBehaviour
{
    Rigidbody[] Rgbodies;
    [SerializeField] int PointReq;
    [SerializeField] bool CanKill;
    [SerializeField] TextMeshProUGUI InfoText;
    PlayerController player;


    void Start()
    {
        
        Rgbodies = new Rigidbody[transform.childCount];
        for (int i = 0; i < transform.childCount-1; i++)
        {
            Rgbodies[i] = transform.GetChild(i).GetComponent<Rigidbody>();
        }
        Debug.Log("rgbodies: " + Rgbodies.Length+" child count: "+transform.childCount);
        InfoText.text = PointReq.ToString() + "x";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        player = other.GetComponent<PlayerController>();
        if (player.GamePoints >= PointReq) {for (int w = 0; w < Rgbodies.Length-1; w++) Rgbodies[w].isKinematic = false; }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        for (int w = 0; w < Rgbodies.Length-1; w++) Rgbodies[w].isKinematic = true;
    }
}
