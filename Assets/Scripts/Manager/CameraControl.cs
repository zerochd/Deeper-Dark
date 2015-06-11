using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    //获取相机transform
//    private Transform cameraTransform;

    public float moveTime = 0.1f;
    //滑动速度
//    private float smooth = 8f;
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
 //       transform = GetComponent<Transform>();
        //Debug.Log(transform.position);
        inverseMoveTime = 1f / moveTime;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CameraMoveNextRoom(Vector3 DoorVector)
    {
        Debug.Log("camea move");
        Vector3 moveEnd;
        //Debug.Log("DoorVector" + DoorVector + " cameraVector" + transform.position);

        float yMove = (DoorVector.y - transform.position.y) * 2;
        float xMove = (DoorVector.x - transform.position.x) * 2;
        moveEnd = transform.position + new Vector3(xMove, yMove, 0);
        

        StartCoroutine(CameraSmoothMove(moveEnd));
    }

    //相机平滑移动
    IEnumerator CameraSmoothMove(Vector3 moveEnd)
    {
        //求出从起始点到终点的距离
        float sqrRemainingDistance = (transform.position - moveEnd).sqrMagnitude;

        //如果距离大于0
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, moveEnd, inverseMoveTime * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - moveEnd).sqrMagnitude;
            yield return null;
        }
    }
}
