using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour
{
    public bool buildUI;
    [SerializeField] private GameObject panel, cursor;
    public void Build()
    {
        buildUI = !buildUI;
        panel.SetActive(buildUI);
        Time.timeScale = buildUI ? 0 : 1;
        if (!buildUI)
        {
            cursor.SetActive(false);
        }
    }
}
