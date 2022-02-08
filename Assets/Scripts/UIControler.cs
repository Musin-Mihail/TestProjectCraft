using UnityEngine;
public class UIControler
{
    FindTransform findTransform;
    Transform firstText;
    Transform buttonMenu;
    public bool menu = false;
    public void Initialization()
    {
        findTransform = new GameObject().AddComponent<FindTransform>();
        firstText = findTransform.FindFirstText();
        buttonMenu = findTransform.FindButtonMenu();
    }
    public void HideUI()
    {
        firstText.gameObject.SetActive(false);
        buttonMenu.gameObject.SetActive(false);
        menu = false;
    }
    public void ShowFirstText()
    {
        firstText.gameObject.SetActive(true);
        buttonMenu.gameObject.SetActive(false);
        menu = false;
    }
    public void ShowMenu()
    {
        menu = true;
        buttonMenu.gameObject.SetActive(true);
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
}