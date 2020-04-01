using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishCheck : MonoBehaviour
{
    bool started;

    // Start is called before the first frame update
    void Start()
    {
        started = false;
    }

    private void OnTriggerEnter(Collision collision) {

        if (collision.gameObject.tag == "Start") {
            started = true;
        }

        if (started && collision.gameObject.tag == "Finish") {
            started = false;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
