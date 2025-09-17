using System.Collections;
using UnityEngine;

public class MyMessageListener : MonoBehaviour
{
    [SerializeField]private skateController skateController;
    [SerializeField]private Spawner spawner;

    private bool[] buttonPressed = new bool[6];
    private bool[] buttonReleased = new bool[6];

    void OnMessageArrived(string msg)
    {
        if (string.IsNullOrEmpty(msg)) return;

        if (msg.StartsWith("P")) // Press
        {
            int index;
            if (int.TryParse(msg.Substring(1), out index) && index >= 0 && index < buttonPressed.Length)
            {
                buttonPressed[index] = true; // stays true until release
            }
        }
        else if (msg.StartsWith("R")) // Release
        {
            int index;
            if (int.TryParse(msg.Substring(1), out index) && index >= 0 && index < buttonReleased.Length)
            {
                buttonReleased[index] = true;
                StartCoroutine(ResetBool(buttonReleased, index, 0.2f));

                // Also reset the pressed state
                buttonPressed[index] = false;
            }
        }
    }

    private IEnumerator ResetBool(bool[] arr, int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        arr[index] = false;
    }

    void Update()
    {
        // --- 2-button Press Combos ---
        if (buttonPressed[0] && buttonPressed[1])
        {
            /* do something */
        }
        else
        if (buttonPressed[0] && buttonPressed[2])
        {
            /* do something */
        }
        else
        if (buttonPressed[0] && buttonPressed[3])
        {
            /* do something */
        }
        else
        if (buttonPressed[0] && buttonPressed[4])
        {
            /* do something */
        }
        else
        if (buttonPressed[0] && buttonPressed[5])
        {
            /* do something */
        }
        else
        if (buttonPressed[1] && buttonPressed[2])
        {
            /* do something */
        }
        else
        if (buttonPressed[1] && buttonPressed[3])
        {
            /* do something */
        }
        else
        if (buttonPressed[1] && buttonPressed[4])
        {
            /* do something */
        }
        else
        if (buttonPressed[1] && buttonPressed[5])
        {
            /* do something */
        }
        else
        if (buttonPressed[2] && buttonPressed[3])
        {
            /* do something */
        }
        else
        if (buttonPressed[2] && buttonPressed[4])
        {
            /* do something */
        }
        else
        if (buttonPressed[2] && buttonPressed[5])
        {
            /* do something */
        }
        else
        if (buttonPressed[3] && buttonPressed[4])
        {
            /* do something */
        }
        else
        if (buttonPressed[3] && buttonPressed[5])
        {
            /* do something */
        }
        else
        if (buttonPressed[4] && buttonPressed[5])
        {
            /* do something */
        }
        else
        {
            spawner.SetHoldingState(false);
        }



        // --- 2-button Release Combos ---
        if (buttonReleased[0] && buttonReleased[1]) { /* do something */ buttonReleased[0] = buttonReleased[1] = false; }
        if (buttonReleased[0] && buttonReleased[2]) { /* do something */ buttonReleased[0] = buttonReleased[2] = false; }
        if (buttonReleased[0] && buttonReleased[3]) { /* do something */ buttonReleased[0] = buttonReleased[3] = false; }
        if (buttonReleased[0] && buttonReleased[4]) { /* do something */ buttonReleased[0] = buttonReleased[4] = false; }
        if (buttonReleased[0] && buttonReleased[5]) { /* do something */ buttonReleased[0] = buttonReleased[5] = false; }
        if (buttonReleased[1] && buttonReleased[2]) { /* do something */ buttonReleased[1] = buttonReleased[2] = false; }
        if (buttonReleased[1] && buttonReleased[3]) { /* do something */ buttonReleased[1] = buttonReleased[3] = false; }
        if (buttonReleased[1] && buttonReleased[4]) { /* do something */ buttonReleased[1] = buttonReleased[4] = false; }
        if (buttonReleased[1] && buttonReleased[5]) { /* do something */ buttonReleased[1] = buttonReleased[5] = false; }
        if (buttonReleased[2] && buttonReleased[3]) { /* do something */ buttonReleased[2] = buttonReleased[3] = false; }
        if (buttonReleased[2] && buttonReleased[4]) { /* do something */ buttonReleased[2] = buttonReleased[4] = false; }
        if (buttonReleased[2] && buttonReleased[5]) { /* do something */ buttonReleased[2] = buttonReleased[5] = false; }
        if (buttonReleased[3] && buttonReleased[4]) { /* do something */ buttonReleased[3] = buttonReleased[4] = false; }
        if (buttonReleased[3] && buttonReleased[5]) { /* do something */ buttonReleased[3] = buttonReleased[5] = false; }
        if (buttonReleased[4] && buttonReleased[5]) { /* do something */ buttonReleased[4] = buttonReleased[5] = false; }
    }

    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Device connected" : "Device disconnected");
    }
}
