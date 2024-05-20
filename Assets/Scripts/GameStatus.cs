using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStatus : MonoBehaviour
{
    [SerializeField]
    private TeamDashboard[] teamDashboards = new TeamDashboard[2];
    [SerializeField]
    private GameObject[] itemObjs = new GameObject[3];
    [SerializeField]
    private GameObject[] stateObjs = new GameObject[3];
    [SerializeField]
    private GameObject countdownObj;
    [SerializeField]
    private GameObject loadingObj;
    // Start is called before the first frame update
    void OnEnable() {
        StartCoroutine(countdownObj.GetComponent<Countdown>().CountdownCoroutine());
    }

    public void StartLoading() {
        StartCoroutine(loadingObj.GetComponent<Loading>().LoadingCoroutine());
    }

    public TeamDashboard GetTeamDashboard(int id) {
        return teamDashboards[id - 1];
    }

    public GameObject GetItemObj(int itemId) {
        return itemObjs[itemId - 1];
    }

    public GameObject GetStateObj(int itemId) {
        return stateObjs[itemId - 1];
    }
}
