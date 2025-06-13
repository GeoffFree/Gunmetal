using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public TMP_Text waves;
    public TMP_Text enemies;

    void Start()
    {
        waves.text = "Waves Survived: " + SaveData.wavesSurvived;
        enemies.text = "Score: " + SaveData.totalScore;
    }
}
