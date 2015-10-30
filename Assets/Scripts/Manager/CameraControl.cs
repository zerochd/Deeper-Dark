using UnityEngine;
using System.Collections;

public class CameraControl : UnitySingleton<CameraControl> {

//	//地图照相机
//	public Camera mapCamera;

	//board中心
	private Vector3 boardCenter;

	//camera上一次固定的位置
	private Vector3 cameraStartPosition;

	private float sizeLasted;

    private Room roomStart;
    //移动时间
    public float moveTime = 0.1f;
    //滑动速度
    private float inverseMoveTime;
//    //单例这个脚本

	// Use this for initialization
	void Start () {
		cameraStartPosition = this.transform.position;
        //Debug.Log(transform.position);
        inverseMoveTime = 1f / moveTime;

		//注册SwitchMapEvent事件
//		GameManager.SwitchMapEvent += CameraMap;

	}

    public void CameraMoveNextRoom(Vector3 DoorVector)
    {
//        Debug.Log("camea move");
        Vector3 moveEnd;
        //Debug.Log("DoorVector" + DoorVector + " cameraVector" + transform.position);

        float yMove = (DoorVector.y - transform.position.y) * 2;
        float xMove = (DoorVector.x - transform.position.x) * 2;
        moveEnd = transform.position + new Vector3(xMove, yMove, 0);
		StartCoroutine(CameraSmoothMove(moveEnd,inverseMoveTime));
    }

    //相机平滑移动
	IEnumerator CameraSmoothMove(Vector3 moveEnd,float inverseime)
    {
        //求出从起始点到终点的距离
        float sqrRemainingDistance = (transform.position - moveEnd).sqrMagnitude;

        //如果距离大于0
        while (sqrRemainingDistance > float.Epsilon)
        {
			Vector3 newPosition = Vector3.MoveTowards(transform.position, moveEnd, inverseime * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - moveEnd).sqrMagnitude;
            yield return null;
        }
        //切换门
//		Debug.Log ("move over");
		//当相机移动完成后，开启怪物AI
    }


//	void CameraMap(){
//		if(!mapCamera.enabled)
//		{
//			Bag.Instance.DisableBag();
//			GameManager.Instance.StopTime();
//			Globe.game_state = Globe.GAME_STATE.MAPING;
//			mapCamera.enabled = true;
//		}
//		else{
//			GameManager.Instance.StartTime();
////			Globe.game_state = Globe.GAME_STATE.RUNNING;
//			mapCamera.enabled = false;
//
//		}
//	}
//
//	public void DisableCamerMap(){
//		mapCamera.enabled = false;
//	}

	public void ResetCameraPosition(){
		this.transform.position = cameraStartPosition;
	}
	
}
