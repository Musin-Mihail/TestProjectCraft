using UnityEngine;
using UnityEngine.UI;
public class UIControler
{
    FindTransform findTransform;
    Transform firstText;
    Transform buttonMenu;
    Text cristalsPointText;
    public int cristalsPoint = 0;
    public bool menu = false;
    public void Initialization()
    {
        findTransform = new GameObject().AddComponent<FindTransform>();
        firstText = findTransform.FindFirstText();
        buttonMenu = findTransform.FindButtonMenu();
        cristalsPointText = findTransform.FindCristalsPoint().GetComponent<Text>();
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
        menu = false;
        buttonMenu.gameObject.SetActive(false);
    }
    public void ShowMenu()
    {
        menu = true;
        buttonMenu.gameObject.SetActive(true);
    }
    public void AddOnePoint()
    {
        cristalsPoint++;
        cristalsPointText.text = cristalsPoint.ToString();
    }
}
public class FindTransform : MonoBehaviour
{
    public Transform FindFirstText()
    {
        return FindObjectOfType<TagFirstText>(true).transform;
    }
    public Transform FindButtonMenu()
    {
        return FindObjectOfType<TagButtonMenu>(true).transform;
    }
    public Transform FindCristalsPoint()
    {
        return FindObjectOfType<TagCristalsPoint>(true).transform;
    }
}