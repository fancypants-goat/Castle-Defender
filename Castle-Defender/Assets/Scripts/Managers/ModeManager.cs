using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public bool buildUI;
    public bool walkMode;
    [SerializeField] private GameObject panel, buildButton,expansionSilhouettes;
    public void Build()
    {
        buildUI = !buildUI;
        panel.SetActive(buildUI);
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
        Vector3 offset = walkMode ? new Vector3(0,0,-1) : new Vector3(0,0,1);
        Camera.main.transform.position += offset;
    }
}
