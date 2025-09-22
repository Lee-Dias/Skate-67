using System.Collections;
using UnityEngine;

public class MyMessageListener : MonoBehaviour
{
    [SerializeField]private skateController skateController;
    [SerializeField]private Spawner spawner;

    private bool[] buttonPressed = new bool[8];
    private bool[] buttonReleased = new bool[8];

    private bool IstryingTailSlide;
    private bool IstryingNoseSlide;

    void OnMessageArrived(string msg)
    {
        Debug.Log(msg);
        if (string.IsNullOrEmpty(msg))
        {
            return;
        }
        

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
        if (buttonPressed[2])
        {
            spawner.SetHoldingState(true);
        }
        else
        if (buttonPressed[3])
        {
            spawner.SetHoldingState(true);
        }
        else
        if (buttonPressed[4])
        {
            spawner.SetHoldingState(true);
        }
        else
        if (buttonPressed[5])
        {
            spawner.SetHoldingState(true);
        }
        else
        if (buttonPressed[6])
        {

            spawner.SetHoldingState(true);
            if (IstryingNoseSlide == false)
            {
                skateController.NoseSlide();
                IstryingNoseSlide = true;
            }

        }
        else
        if (buttonPressed[7])
        {
            spawner.SetHoldingState(true);
            if (IstryingTailSlide == false)
            {
                skateController.TailSlide();
                IstryingTailSlide = true;
            }

        }
        else
        {
            spawner.SetHoldingState(false);
        }



        // --- 2-button Release Combos ---
        if (buttonReleased[0] && buttonReleased[2]) { skateController.TreFlip(); buttonReleased[0] = buttonReleased[2] = false; }
        if (buttonReleased[0] && buttonReleased[4]) { skateController.FsTreFlip(); buttonReleased[0] = buttonReleased[4] = false; }
        if (buttonReleased[0] && buttonReleased[5]) { skateController.KickFlip(); buttonReleased[0] = buttonReleased[5] = false; }
        if (buttonReleased[1] && buttonReleased[2]) { skateController.TreHeelFlip(); buttonReleased[1] = buttonReleased[2] = false; }
        if (buttonReleased[1] && buttonReleased[4]) { skateController.FsTreHeelFlip(); buttonReleased[1] = buttonReleased[4] = false; }
        if (buttonReleased[1] && buttonReleased[5]) { skateController.HeelFlip(); buttonReleased[1] = buttonReleased[5] = false; }
        if (buttonReleased[2] && buttonReleased[3]) { skateController.BigSpin(); buttonReleased[2] = buttonReleased[3] = false; }
        if (buttonReleased[3] && buttonReleased[4]) { skateController.FsBigSpin(); buttonReleased[3] = buttonReleased[4] = false; }
        if (buttonReleased[3] && buttonReleased[5]) { skateController.Ollie(); buttonReleased[3] = buttonReleased[5] = false; }
        if (buttonReleased[6])
        {
            IstryingNoseSlide = false;
        }
        if (buttonReleased[7])
        {
            IstryingTailSlide = false;
        }
    }

    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Device connected" : "Device disconnected");
    }
}
