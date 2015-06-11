using UnityEngine;
using System.Collections;

public class Player : AllCharacter {

    int exp;                        //经验值
    int level;                      //等级

    //人物移动至下一个房间的速度
    float inverseTime;

    bool canControl;
	// Use this for initialization
	void Start () {
        inverseTime = 10f;
        canControl = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (canControl)
        {
            Move();
        }
	}

    void Move()
    {
        speed = 3f;
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        GetComponent<Rigidbody2D>().velocity = new Vector2(h * speed, v * speed);
    }

    public void MoveNextRoom(Vector3 cameraVector,Vector3 DoorVector)
    {
        Debug.Log("正在进入房间");
        Vector3 playerMoveEnd;

        //float yMove = (DoorVector.y - transform.position.y) ;
        //float xMove = (DoorVector.x - transform.position.x);
        float yMove = 0f ;
        float xMove = 0f;

        if (Mathf.Abs(DoorVector.x - cameraVector.x) < float.Epsilon)
        {
            inverseTime = 3f;
            yMove += Mathf.Sign(DoorVector.y - cameraVector.y);
        }
        if (Mathf.Abs(DoorVector.y - cameraVector.y) < float.Epsilon)
        {
            inverseTime = 1.5f;
            xMove += Mathf.Sign(DoorVector.x - cameraVector.x);
        }
        Debug.Log(xMove + "" + yMove);
        playerMoveEnd = transform.position + new Vector3(xMove, yMove, 0);
        canControl = false;
        StartCoroutine(PlayerSmoothMove(playerMoveEnd, inverseTime));
       
    }

    //玩家平滑移动
    IEnumerator PlayerSmoothMove(Vector3 moveEnd,float inverseTime)
    {
        Debug.Log(inverseTime);
        //求出从起始点到终点的距离
        float sqrRemainingDistance = (transform.position - moveEnd).sqrMagnitude;
        Debug.Log("移动开始"); 
        //如果距离大于0
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Debug.Log("正在移动");
            Vector3 newPosition = Vector3.MoveTowards(transform.position, moveEnd, inverseTime * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - moveEnd).sqrMagnitude;
            yield return null;
        }      
            Debug.Log("已经移动完成");
        //将玩家的触发器改为碰撞器
           GetComponent<BoxCollider2D>().isTrigger = false;
           canControl = true;
    }
}
