using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public int wavesSurvived;
    public int totalScore;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
