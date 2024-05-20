using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSelection : MonoBehaviour
{
    [SerializeField]
    private EngineButton[] engineButtons;
    // Start is called before the first frame update
    void OnEnable()
    {
        ActivateEngine();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouseClick();
    }

    private void CheckMouseClick() {
        if(Input.GetMouseButtonDown(0)) {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.tag == tag) {
                GameObject engineObject = hit.collider.gameObject;
                int engineLevel = engineObject.GetComponent<EngineButton>().ReturnLevel();
                ActivateEngine(engineLevel);
            }
        }
    }

    public void ActivateEngine(int engineLevel = 2) {
        if(engineLevel == 0) return;
        for(int i = 0; i < engineButtons.Length; ++i) {
            if(engineButtons[i].ReturnLevel() == engineLevel)
                engineButtons[i].ActivateEngine();
            else
                engineButtons[i].DeactivateEngine();
        }
    }
}