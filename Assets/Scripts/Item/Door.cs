using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    bool doorOpen = true;

	// Use this for initialization
	void Start () {
	    
	}

    //检测到角色碰撞到门
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("you have enter");
            //将玩家的碰撞器改为触发器
            other.transform.GetComponent<BoxCollider2D>().isTrigger = true;
            CameraControl.cameraInstance.CameraMoveNextRoom(transform.position);
            other.transform.GetComponent<Player>().MoveNextRoom(CameraControl.cameraInstance.transform.position,transform.position);
        }
    }

    
}
