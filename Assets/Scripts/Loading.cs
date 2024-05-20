using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float delayTime;
    [SerializeField]
    private GameObject loadingCircle;

    public IEnumerator LoadingCoroutine() {
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(true);
        float loadingDuration = Random.Range(2f, 3f);
        while(loadingDuration >= 0.01f) {
            loadingDuration -= 0.01f;
            Vector3 currentRotation = loadingCircle.transform.eulerAngles;
            float newRotation = currentRotation.z + (rotationSpeed * Time.deltaTime);
            loadingCircle.transform.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, newRotation);
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.SetActive(false);
        UI_Manager.instance.ChangeScene();
        yield break;
    }
}
