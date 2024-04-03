using UnityEngine;
using System.Collections;

public class HitStop : MonoBehaviour
{
    public float stopDuration = 0.05f; // Duration of the hit stop effect in seconds
    public int stopCount = 3;
    // Function to trigger hit stop effect
    public void TriggerHitStop()
    {
        StartCoroutine(HitStopCoroutine());
    }

    // Coroutine for hit stop effect
    IEnumerator HitStopCoroutine()
    {
        int counter = 1;
        while (counter<= stopCount)
        {

            // Restore the normal time scale
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(stopDuration *0.3f);
            Time.timeScale = 1;
            counter++;
            
        }

    }
}
