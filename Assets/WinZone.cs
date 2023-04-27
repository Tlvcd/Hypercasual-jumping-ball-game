using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    [SerializeField] Transform RefPoint;
    PlayerController player;
    public delegate void WinEvent();
    public static event WinEvent OnWin;
    public bool Measuring;
    float currDistance;
    public int FinalPoints;

    private void OnEnable()
    {
        PlayerController.OnDeath += MeasureScore;
    }
    private void OnDisable()
    {
        PlayerController.OnDeath -= MeasureScore;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        player = other.GetComponent<PlayerController>();
        player.InWinZone = true;
        Measuring = true;
        StartCoroutine(WinMeasure());
    }
    //start measuring distance from end to players pos, and return score;
    IEnumerator WinMeasure()
    {


        while (Measuring)
        {
            RefPoint.transform.position = new Vector3(RefPoint.transform.position.x, player.transform.position.y, transform.position.z);
            currDistance = Vector3.Distance(RefPoint.transform.position, player.transform.position);
            Debug.Log(currDistance + " distance from end point to player");


            yield return 0;
        }
    }
    void MeasureScore()
    {
        if (!Measuring) return;
        Measuring = false;
        switch (currDistance)
        {
            case float n when n<10:
                FinalPoints = 0;
                break;
            case float n when n > 10&& n<20:
                FinalPoints = 1;
                break;
            case float n when n > 20 && n < 30:
                FinalPoints = 2;
                break;
            case float n when n > 30 && n < 40:
                FinalPoints = 3;
                break;
            case float n when n > 40 && n < 50:
                FinalPoints = 4;
                break;
            case float n when n > 50:
                FinalPoints = 5;
                break;
        }
        Debug.Log(FinalPoints + " final points");
        Win();
    }
    public void Win()
    {
        OnWin();
        player.StopPlayer();
        if (!player.IsDead) player.WinCelebration();
    }
}
