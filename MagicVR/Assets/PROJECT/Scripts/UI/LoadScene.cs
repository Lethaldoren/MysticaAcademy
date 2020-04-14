using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

//Create an empty gameobject, attach this script and place it in the scene.
//Make sure to turn it into a prefab too. Name is SceneManager
//In the inspector of a UI Button: in the "On Click ()" section, hit the +
//   drag in the SceneManager prefab, Runtime Only, and LoadScene > LoadScene.SceneLoader.
//   Remember to add all scenes to build, and check when number (int) each scene is.

public class LoadScene : MonoBehaviour
{
   public void SceneLoader(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
