using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownTimer : MonoBehaviour {

    Image image; // cache image component
    bool coolingDown;
    float CD;

	// Use this for initialization
	void Awake ()
    {
        image = GetComponent<Image>();     

    }    
    void Update()
    {
        if(coolingDown)
        {
            if ((image.fillAmount += 1f / CD * Time.deltaTime) >= 1f)
                coolingDown = false; 
        }
    }

    public void Spin(float cd)
    {
        coolingDown = true;
        image.fillAmount = 0f;
        CD = cd;        
    }

    
	
	
}
