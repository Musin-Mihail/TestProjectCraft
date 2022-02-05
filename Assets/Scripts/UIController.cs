using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    int collectedCrystals;
    public Text collectedCrystalsText;
    public GameObject FirstText;
    public GameObject GameDifficulty;
    public void AddOnePoint()
    {
        collectedCrystals++;
        collectedCrystalsText.text = collectedCrystals.ToString();
    }
    public void GameOver()
    {
        FirstText.SetActive(true);
        GameDifficulty.SetActive(false);
        collectedCrystals = 0;
        collectedCrystalsText.text = collectedCrystals.ToString();
    }
    public void StartGame()
    {
        FirstText.SetActive(false);
    }
    public void ChangeGameDifficulty()
    {
        Time.timeScale = 0;
        FirstText.SetActive(false);
        GameDifficulty.SetActive(true);
    }
}