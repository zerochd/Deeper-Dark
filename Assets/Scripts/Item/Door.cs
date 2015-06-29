using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    bool doorOpen = true;

	// Use this for initialization
	void Start () {
	    
	}

    public void Close()
    {
        doorOpen = false;
    }

    public void Open()
    {
        doorOpen = true;
    }


    //检测到角色碰撞到门
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player") && doorOpen)
        {
            Debug.Log("you have enter");
            //将玩家的方形碰撞器改为触发器
            //other.transform.GetComponent<BoxCollider2D>().isTrigger = true;
            //将玩家的圆形碰撞器改为触发器
            other.transform.GetComponent<CircleCollider2D>().isTrigger = true;
            //将玩家的速度减为0
            other.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //移动摄像头
            CameraControl.cameraInstance.CameraMoveNextRoom(transform.position);
            //移动玩家
            other.transform.GetComponent<Player>().MoveNextRoom(CameraControl.cameraInstance.transform.position,transform.position);
           
        }
    }

    
}
