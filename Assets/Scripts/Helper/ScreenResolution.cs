using UnityEngine;
using System.Collections;

public class ScreenResolution : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//分辨率设置成960X480,窗口模式
		Screen.SetResolution (960, 480, false);
	}

}
