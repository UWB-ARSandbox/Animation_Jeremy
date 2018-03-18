using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleText : MonoBehaviour {

    public Button PauseButton;
    // Update is called once per frame
    void Update () {
		
	}

  

    void Start()
    {
        PauseButton.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        if (PauseButton.GetComponentInChildren<Text>().text == "||")
            PauseButton.GetComponentInChildren<Text>().text = ">";
        else if (PauseButton.GetComponentInChildren<Text>().text == ">")
            PauseButton.GetComponentInChildren<Text>().text = "||";

    }

}

