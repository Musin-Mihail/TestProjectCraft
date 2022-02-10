using UnityEngine;
using UnityEngine.UI;
public class UIControler : MonoBehaviour
{
    protected Transform firstText;
    protected Transform buttonMenu;
    protected Text cristalsPointText;
    public int cristalsPoint = 0;
    public void Initialization()
    {
        firstText = FindObjectOfType<TagFirstText>(true).transform;
        buttonMenu = FindObjectOfType<TagButtonMenu>(true).transform;
        cristalsPointText = FindObjectOfType<TagCristalsPoint>(true).transform.GetComponent<Text>();
    }
    public void HideFirstText()
    {
        firstText.gameObject.SetActive(false);
    }
    public void ShowFirstText()
    {
        firstText.gameObject.SetActive(true);
    }
    public void HideMenu()
    {
        buttonMenu.gameObject.SetActive(false);
    }
    public void ShowMenu()
    {
        buttonMenu.gameObject.SetActive(true);
    }
    public void AddOnePoint()
    {
        cristalsPoint++;
        cristalsPointText.text = cristalsPoint.ToString();
    }
    public void Restart()
    {
        cristalsPoint = 0;
        cristalsPointText.text = cristalsPoint.ToString();
    }
}