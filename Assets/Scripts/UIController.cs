using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    int collectedCrystals;
    public Text collectedCrystalsText;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void AddOnePoint()
    {
        collectedCrystals++;
        collectedCrystalsText.text = collectedCrystals.ToString();
    }
    public void GameOver()
    {
        collectedCrystals = 0;
        collectedCrystalsText.text = collectedCrystals.ToString();
    }
}