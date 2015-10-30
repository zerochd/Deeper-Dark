using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	//房间进入后云颜色的变化
	public Color enterColor = new Color(1f,1f,1f,1f);

	// Update is called once per frame
	void Update () {
		if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
		{
			GetComponent<SpriteRenderer>().color = enterColor;
		}
	}
}
