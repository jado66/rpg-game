using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipFollowMouse : MonoBehaviour {
	RectTransform rectTransform;
	float height;

	void Start(){
		rectTransform.GetComponent<RectTransform>();
		
	}

	void OnEnable(){
		// GameObject.Find("PlayerUI").GetComponent<Canvas>().ForceUpdateCanvase();
		rectTransform = transform.GetComponent<RectTransform>();
		// width = rt.rect.width;
	}
	// Update is called once per frame
	void LateUpdate() {
		height = 4*rectTransform.sizeDelta.y * rectTransform.localScale.y;

		// Debug.Log("Hi"+rectTransform.rect.height.ToString());
        transform.position = Input.mousePosition + new Vector3(75,height,0);
	}
}
