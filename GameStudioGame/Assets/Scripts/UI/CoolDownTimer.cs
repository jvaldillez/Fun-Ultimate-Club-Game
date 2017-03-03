using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownTimer : MonoBehaviour {

    Image image; // cache image component

	// Use this for initialization
	void Awake ()
    {
        image = GetComponent<Image>();     

    }

    void Update()
    {
        image.fillAmount -= 0.01f;
    }
	
	
}
