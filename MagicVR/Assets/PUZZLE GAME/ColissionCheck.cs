using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColissionCheck : MonoBehaviour
{
    public Transform handle;
    Transform handleSpawn;

    private void Start() {
        handleSpawn = handle;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Handle") {

            GameObject handle = collision.gameObject;

            handle = handle.transform.parent.gameObject;

            handle.transform.position = handleSpawn.transform.position;
        }
    }
}
