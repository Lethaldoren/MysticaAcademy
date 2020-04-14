using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vision : MonoBehaviour {

    public GameObject owner;
    [SerializeField]
    List<GameObject> objectsInVolume;
    public List<GameObject> visibleObjects;
    public List<string> seeableTags;

    public Color debugOccludedColor;
    public Color debugSightColor;

    public string visionEnterMessage = "OnVisionEnter";
    public string visionExitMessage = "OnVisionExit";

    void Awake() {
        // initialize both lists
        objectsInVolume = new List<GameObject>();
        visibleObjects = new List<GameObject>();
    }

    void OnTriggerEnter(Collider other) {
        if (!seeableTags.Contains(other.gameObject.tag)) return;
        objectsInVolume.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other) {
        objectsInVolume.Remove(other.gameObject);
        LostSight(other.gameObject);
    }


    void LostSight(GameObject other) {
        if (visibleObjects.Contains(other)) {
            visibleObjects.Remove(other);
            owner.SendMessage(visionExitMessage, other, SendMessageOptions.DontRequireReceiver);
        }
    }

    void Update() {
        for (int i = objectsInVolume.Count - 1; i >= 0; i--) {
            GameObject objectInVolume = objectsInVolume[i];

            // if object has been disabled or deleted, remove it from both lists
            if (objectInVolume == null || !objectInVolume.activeSelf) {
                LostSight(objectInVolume);
                objectsInVolume.Remove(objectInVolume);
                continue;
            }

            // raycast from the owner toward object in volume
            Vector3 rayDirection = objectInVolume.transform.position - owner.transform.position;
            Ray ray = new Ray(owner.transform.position, rayDirection);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 100);


            // see object
            if (hit.collider.gameObject == objectInVolume) {

                Debug.DrawLine(owner.transform.position, objectInVolume.transform.position, debugSightColor);

                // skip if already in visible list
                if (visibleObjects.Contains(objectInVolume)) continue;

                // add to visible list 
                visibleObjects.Add(objectInVolume);
                owner.SendMessage(visionEnterMessage, objectInVolume, SendMessageOptions.DontRequireReceiver);
            } else {
                Debug.DrawLine(ray.origin, hit.point, debugOccludedColor);
                // can't see object
                LostSight(objectInVolume);
            }

        }
    }

}
