using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class NumScript : MonoBehaviour
{
    TextMeshProUGUI text;
    float alpha;
    async void Start()
    {
        text = this.GetComponent<TextMeshProUGUI>();
        LeanTween.moveLocalY(gameObject, 420, 0.5f);
        Fade(5f, 1);
        await Task.Delay(500);
        Destroy(gameObject);

    }
    async void Fade(float ti,float fadeto)
    {
        while (text.alpha !=fadeto)
        {
            text.alpha = Mathf.Lerp(text.alpha, fadeto, ti * Time.deltaTime);
            await Task.Yield();
        }
    }
}
