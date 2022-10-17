using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    public Text bestTimeText;

    // Start is called before the first frame update
    void Start()
    {
        if (PersistenceData.instance.bestTime > 0)
        {
            float minutes = Mathf.FloorToInt(PersistenceData.instance.bestTime / 60);
            float seconds = Mathf.FloorToInt(PersistenceData.instance.bestTime % 60);
            bestTimeText.text = string.Format("Best Score : {0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            bestTimeText.text = string.Empty;
        }
    }

    public void Exit()
    {
        PersistenceData.instance.Save();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
