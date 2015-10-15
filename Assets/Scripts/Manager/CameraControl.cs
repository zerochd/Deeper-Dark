using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public Camera mapCamera;

	//board中心
	private Vector3 boardCenter;

	//camera上一次固定的位置
	private Vector3 cameraVecLasted;

	private float sizeLasted;

    private Room roomStart;
    //移动时间
    public float moveTime = 0.1f;
    //滑动速度
    private float inverseMoveTime;
    //单例这个脚本
    public static CameraControl cameraInstance = null;

    void Awake()
    {
        if (cameraInstance == null)
        {
            cameraInstance = this;
        }
        else if (cameraInstance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        
    }

	// Use this for initialization
	void Start () {

        //Debug.Log(transform.position);
        inverseMoveTime = 1f / moveTime;

		//注册SwitchMapEvent事件
		GameManager.SwitchMapEvent += CameraMap;

	}

//	void Update(){
//		if(Input.GetKeyDown(KeyCode.M)){
//
//			CameraMap();
//		}
//	}

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

//	//相机size调整
//	IEnumerator CameraSizeFix(float size){
//		float sizeBefore = GetComponent<Camera>().orthographicSize;
//
//		Debug.Log ("befroe:"+sizeBefore);
//		Debug.Log ("gotosize:"+size);
////		float acc;
////		if(size < sizeBefore)
////			acc = (-sizeBefore) / size;
////		acc = size / sizeBefore;
//
//		while(Mathf.Abs(sizeBefore - size) > float.Epsilon){
//
////			float sizeNow = sizeBefore + acc * 0.005f;
////			GetComponent<Camera>().orthographicSize = sizeNow;
////			sizeBefore = sizeNow;
//			float sizeNow = sizeBefore;
//			if(sizeBefore < size){
//				sizeNow = Mathf.Lerp(sizeBefore,size,Time.fixedTime * 0.045f);
//			}
//			else{
//				Debug.Log ("low");
//				sizeNow = size;
//			}
//			GetComponent<Camera>().orthographicSize = sizeNow;
//			sizeBefore = sizeNow;
//
//			yield return null;
//		}
//
//	}


	void CameraMap(){

//		if(Globe.game_state != Globe.GAME_STATE.MAPING){
//
//			Globe.game_state = Globe.GAME_STATE.MAPING;
//
//			cameraVecLasted = this.transform.position;
//		
//
//			Vector3 moveEnd = GetBoardCenter();
//			int size_x = (int)((BoardManager.xy_max.x-BoardManager.xy_min.x)/(BoardManager.COLUMNS - 1));
//			int size_y = (int)((BoardManager.xy_max.y-BoardManager.xy_min.y)/(BoardManager.ROWS - 1));
//			float size_final = (size_x > size_y ? size_x  : size_y) * this.GetComponent<Camera>().orthographicSize;
//			//		Debug.Log ("size:"+size_final);
//			//镜头坐标移动
//			StartCoroutine(CameraSizeFix(size_final));
//			StartCoroutine(CameraSmoothMove(moveEnd,inverseMoveTime*5));
//			//镜头size放大
//		}
//		else{
//			Globe.game_state = Globe.GAME_STATE.RUNNING;
//			StartCoroutine(CameraSmoothMove(cameraVecLasted,inverseMoveTime*5));
//			StartCoroutine(CameraSizeFix(sizeLasted));
//		}

//		if(Globe.game_state != Globe.GAME_STATE.MAPING){
//			Globe.game_state = Globe.GAME_STATE.MAPING;
//			if(!mapCamera.enabled){
//				mapCamera.enabled = true;
//			}
//		}
//		else{
//			Globe.game_state = Globe.GAME_STATE.RUNNING;
//			if(mapCamera.enabled){
//				mapCamera.enabled = false;
//			}
//		}
		if(!mapCamera.enabled)
		{
			Globe.game_state = Globe.GAME_STATE.MAPING;
			mapCamera.enabled = true;
		}
		else{
			Globe.game_state = Globe.GAME_STATE.RUNNING;
			mapCamera.enabled = false;
		}

	}

//	Vector3 GetBoardCenter(){
//		boardCenter = (BoardManager.xy_max + BoardManager.xy_min) / 2 - Vector3.forward * 5;
////		Debug.Log ("boardCenter"+boardCenter);
//		return boardCenter;
//	}
}
