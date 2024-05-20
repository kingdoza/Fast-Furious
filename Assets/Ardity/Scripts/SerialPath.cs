using System;
using System.Collections;
using UnityEngine;

public class SerialPath : MonoBehaviour
{
    public SerialController serialController;
    private Team team;

    // Initialization
    public void SetTeam()
    {
        if(tag == "Left") {
            team = GameManager.instance.leftTeam;
        }
        else if(tag == "Right") {
            team = GameManager.instance.rightTeam;
        }
        //Debug.Log("Press A or Z to execute some valueNames" + " " + serialController.portName);
    }
    
    void Start()
    {
        if(tag == "Left") {
            team = GameManager.instance.leftTeam;
        }
        else if(tag == "Right") {
            team = GameManager.instance.rightTeam;
        }
        Debug.Log("Press A or Z to execute some actions" + " " + serialController.portName);
    }

    // Executed each frame
    void Update()
    {
        //---------------------------------------------------------------------
        // Send data
        //---------------------------------------------------------------------

        // If you press one of these keys send it to the serial device. A
        // sample serial device that accepts this input is given in the README.
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Sending N");
            serialController.SendSerialMessage("N");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Sending M");
            serialController.SendSerialMessage("M");
        }


        if (Input.GetKeyDown(KeyCode.Q) && tag == "Left")
        {
            //serialController.SendSerialMessage("M60");
            HandleMessage("M60");
        }

        if (Input.GetKeyDown(KeyCode.W) && tag == "Right")
        {
            //serialController.SendSerialMessage("M60");
            HandleMessage("M60");
        }


        if (Input.GetKeyDown(KeyCode.Alpha1) && tag == "Left") {
            //serialController.SendSerialMessage("O");
            HandleMessage("O");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && tag == "Right") {
            //serialController.SendSerialMessage("O");
            HandleMessage("O");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            //serialController.SendSerialMessage("S120");
            HandleMessage("S120");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            //serialController.SendSerialMessage("S140");
            HandleMessage("S140");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            //serialController.SendSerialMessage("S160");
            HandleMessage("S160");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            //serialController.SendSerialMessage("S180");
            HandleMessage("S180");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            //serialController.SendSerialMessage("S200");
            HandleMessage("S200");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            //serialController.SendSerialMessage("S220");
            HandleMessage("S220");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            //serialController.SendSerialMessage("S240");
            HandleMessage("S240");
        }

        if (Input.GetKeyDown(KeyCode.Z) && tag == "Left") {
            //serialController.SendSerialMessage("L");
            HandleMessage("L");
        }
        if (Input.GetKeyDown(KeyCode.X) && tag == "Right") {
            //serialController.SendSerialMessage("L");
            HandleMessage("L");
        }

        if (Input.GetKeyDown(KeyCode.A) && tag == "Left") {
            //serialController.SendSerialMessage("B");
            HandleMessage("B");
        }
        if (Input.GetKeyDown(KeyCode.S) && tag == "Right") {
            //serialController.SendSerialMessage("B");
            HandleMessage("B");
        }


        //---------------------------------------------------------------------
        // Receive data
        //---------------------------------------------------------------------

        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established : " + serialController.portName);
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected : " + serialController.portName);
        else {
            Debug.Log("Message arrived: " + message);
            HandleMessage(message);
        }
    }

    void HandleMessage(string msg) {
        char action = '\0';
        string value = "";
        try {
            action = msg[0];
            value = msg.Substring(1);
        }
        catch(Exception e) {
            Debug.LogError("String formatting error : " + e.Message);
        }

        if(action == 'B') {
            team.PressButton();
            return;
        }

        if(!GameManager.instance.isRacing)
            return;

        switch(action) {
        case 'S':
            team.SetSpeed(int.Parse(value));
            break;
        case 'M':
            team.IncreScore(int.Parse(value));
            break;
        case 'O':
            team.GetRandomItem();
            break;
        case 'L':
            team.IncreLaps();   
            break;
        }
    }
}
