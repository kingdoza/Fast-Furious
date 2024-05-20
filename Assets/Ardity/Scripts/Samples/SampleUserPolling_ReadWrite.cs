/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;

/**
 * Sample for reading using polling by yourself, and writing too.
 */
public class SampleUserPolling_ReadWrite : MonoBehaviour
{
    public SerialController serialController;
    private Team team;

    // Initialization
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Sending A");
            serialController.SendSerialMessage("A");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Sending Z");
            serialController.SendSerialMessage("Z");
        }


        //---------------------------------------------------------------------
        // Receive data
        //---------------------------------------------------------------------

        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else {
            Debug.Log("Message arrived: " + message);
            HandleMessage(message);
        }
    }

    void HandleMessage(string msg) {
        string[] msgParts = msg.Split(":");

        string action = msgParts[0];
        string value = msgParts[1];

        switch(action) {
        case "setSpeed":
            team.SetSpeed(int.Parse(value));
            break;
        case "increScore":
            team.IncreScore(int.Parse(value));
            break;
        case "getItem":
            team.GetItem(int.Parse(value));
            break;
        case "increLaps":
            team.IncreLaps();   
            break;
        case "pressButton":
            team.PressButton();
            break;
        }
    }
}
