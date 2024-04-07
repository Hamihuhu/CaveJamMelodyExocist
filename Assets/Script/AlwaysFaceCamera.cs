using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    [SerializeField] private float updateInterval = 0.5f;

    private Camera mainCamera;
    WaitForSeconds waitForUpdateInterval;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        waitForUpdateInterval = new WaitForSeconds(updateInterval);
        StartCoroutine(ManualUpdate(updateInterval));
    }

    private IEnumerator ManualUpdate(float updateInterval)
    {
        // rotate transform every updateInterval to face camera
        // while also keeping y rotation to 0
        while (true)
        {
            Vector3 targetPosition = new Vector3(mainCamera.transform.position.x, transform.position.y, mainCamera.transform.position.z);
            transform.LookAt(targetPosition);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            yield return waitForUpdateInterval;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
