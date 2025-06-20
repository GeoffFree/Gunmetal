using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public float nextSceneInterval;
    private float nextSceneTimer;
    public int sceneToLoad;

    void Update()
    {
        if (nextSceneTimer > Time.time)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
