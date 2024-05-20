using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialManager : MonoBehaviour
{
    public static SerialManager instance; // 싱글톤 인스턴스
    
    [SerializeField]
    private SerialPath leftSerialPath;
    [SerializeField]
    private SerialPath rightSerialPath;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 다른 씬으로 넘어갈 때도 유지
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있다면 새로 생성된 인스턴스 파괴
        }
    }

    public void SetSerialPathTeam() {
        leftSerialPath.SetTeam();
        rightSerialPath.SetTeam();
    }

    public void SendToLeft(string message) {
        leftSerialPath.serialController.SendSerialMessage(message);
    }

    public void SendToRight(string message) {
        rightSerialPath.serialController.SendSerialMessage(message);
    }

    public void SendToTeam(int teamId, string message) {
        if(teamId == 1)
            SendToLeft(message);
        else
            SendToRight(message);
    }
}
