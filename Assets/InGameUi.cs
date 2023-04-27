using System.Collections;
using UnityEngine;

public class InGameUi : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject WinScreen;
    private void OnEnable()
    {
        WinZone.OnWin += EnableWinScreen;
    }
    private void OnDisable()
    {
        WinZone.OnWin -= EnableWinScreen;
    }
    void EnableWinScreen()
    {
        WinScreen.transform.localScale = Vector3.zero;
        WinScreen.SetActive(true);
        LeanTween.scale(WinScreen,Vector3.one,0.2f).setEase(LeanTweenType.easeOutBounce);
    }
    public void DisableWinScreen()
    {
        LeanTween.scale(WinScreen, Vector3.zero, 0.2f).setEase(LeanTweenType.easeOutBounce);
        StartCoroutine(DisableAfterTween(WinScreen,0.2f));
    }
    IEnumerator DisableAfterTween(GameObject obj, float ti)
    {
        yield return new WaitForSeconds(ti);
        obj.SetActive(false);
    }
}
