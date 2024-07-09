using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RunInfo : MonoBehaviour
{
    public TMP_Text FPSText;


    float currentEvadedTime = 0f;

    // Update is called once per frame
    void Update()
    {
        currentEvadedTime += Time.deltaTime;
        if (currentEvadedTime >= 0.2f) {  
            currentEvadedTime = 0f;
            FPSText.text = $"FPS: {1 / Time.deltaTime * 1000}";
        }
    }
}
