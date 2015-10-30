using UnityEngine;
using System.Collections;

public class MapCamera : MonoBehaviour {

	//地图照相机
	public static Camera mapCamera;

	// Use this for initialization
	void Start () {
		//注册SwitchMapEvent事件
		mapCamera = GetComponent<Camera>();
		GameManager.SwitchMapEvent += CameraMap;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CameraMap(){
		if(!mapCamera.enabled)
		{
			Bag.Instance.DisableBag();
			GameManager.Instance.StopTime();
			Globe.game_state = Globe.GAME_STATE.MAPING;
			mapCamera.enabled = true;
		}
		else{
			GameManager.Instance.StartTime();
			//			Globe.game_state = Globe.GAME_STATE.RUNNING;
			mapCamera.enabled = false;
			
		}
	}
	
	public static void DisableCamerMap(){
		mapCamera.enabled = false;
	}
}
