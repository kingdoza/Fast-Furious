using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyCanvas : MonoBehaviour
{
    public Image leftButton;
    public Image rightButton;
    GameManager gameManager;
    private Color red = new Color(0.8f, 0, 0, 1);
    private Color green = new Color(0, 0.8f, 0, 1);
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        leftButton.color = red;
        rightButton.color = red;
    }
}
