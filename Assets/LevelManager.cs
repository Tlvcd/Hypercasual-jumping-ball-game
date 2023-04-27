using UnityEngine;

namespace LvlManager
{
public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] LevelPrefabs;
    GameObject CurrLvl;
    public int CurrLvlIndex;
    void Start()
    {
            ChangeLevel(1);
    }
    public void ChangeLevel(int index)
    {
            index -= 1;
        if (CurrLvl != null) Destroy(CurrLvl);
        CurrLvl= Instantiate(LevelPrefabs[index]);
        CurrLvlIndex = index;
    }
    public void NextLevel()
        {
            if (CurrLvl != null) Destroy(CurrLvl);
            CurrLvlIndex += 1;
            CurrLvl = Instantiate(LevelPrefabs[CurrLvlIndex]);

        }
    public void RestartCurrent()
        {
            if (CurrLvl != null) Destroy(CurrLvl);
            CurrLvl = Instantiate(LevelPrefabs[CurrLvlIndex]);
        }
}

}
