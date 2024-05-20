using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngineButton : MonoBehaviour
{
    private Image outline;
    [SerializeField]
    private int level;
    private Color none = new Color(255f, 255f, 255f, 0f);
    [SerializeField]
    private GameObject engineInfo;
    private bool isSelected = false;

    void Awake() {
        outline = GetComponent<Image>();
    }
    private void OnMouseEnter() {
        if(isSelected == false)
            outline.color = Color.white;
    }

    private void OnMouseExit() {
        if(isSelected == false)
            outline.color = none;
    }

    public void DeactivateEngine() {
        if(outline == null)
            outline = GetComponent<Image>();
            
        outline.color = none;
        engineInfo.SetActive(false);
        isSelected = false;
    }

    public void ActivateEngine() {
        if(outline == null)
            outline = GetComponent<Image>();

        outline.color = Color.red;
        engineInfo.SetActive(true);
        isSelected = true;

        if(tag == "Left")
            GameManager.instance.leftTeam.SetEngineLevel(level);
        else if(tag == "Right")
            GameManager.instance.rightTeam.SetEngineLevel(level);
    }

    public int ReturnLevel() {
        return level;
    }
}
