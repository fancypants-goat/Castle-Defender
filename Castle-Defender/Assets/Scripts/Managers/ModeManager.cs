using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public bool buildUI;
    public bool walkMode;

    [SerializeField] private GameObject panel, buildButton,expansionSilhouettes;
    [SerializeField] private Camera kingdomCamera;
    public void Build()
    {
        buildUI = !buildUI;
        panel.SetActive(buildUI);
        expansionSilhouettes.SetActive(!buildUI);
        Time.timeScale = buildUI ? 0 : 1;
    }
    public void Walk()
    {
        // Switches between City Builder and Exploration mode
        walkMode = !walkMode;
        // Sets whether only the outer sprite is active or the city
        buildButton.SetActive(!walkMode);
        expansionSilhouettes.SetActive(!walkMode);
        // Sets Camera to show outer sprite
    }

    void Update()
    {
        if (walkMode)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,kingdomCamera.orthographicSize+1,Time.deltaTime * 5);
        }
    }
}
