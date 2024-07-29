using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFollowMouse : MonoBehaviour {

	

	
	// Update is called once per frame
	void LateUpdate() {

		// Debug.Log("Hi"+rectTransform.rect.height.ToString());
        transform.position = Input.mousePosition;
	}
}
