using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    bool doorOpen = true;
	
	public Sprite doorCloseSprite;

	public Sprite doorOpenSprite;

	// Use this for initialization
	void Start () {
	    
	}

    public void Close()
    {
        doorOpen = false;
		GetComponent<SpriteRenderer>().sprite = doorCloseSprite;
//		this.transform.localEulerAngles = new Vector3(0f,0f,-90f);
    }

    public void Open()
    {
        doorOpen = true;
		GetComponent<SpriteRenderer>().sprite = doorOpenSprite;
    }

	void Update(){
		if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
		{
			if(Globe.MeetBoss == true){
				Close();
			}
			else{
				Open();
			}
		}
	}


    //检测到角色碰撞到门
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player") && doorOpen)
        {
//            Debug.Log("you have enter");
            //将玩家的圆形碰撞器改为触发器
            other.transform.GetComponent<CircleCollider2D>().isTrigger = true;
            //将玩家的速度减为0
            other.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //移动摄像头
            CameraControl.Instance.CameraMoveNextRoom(transform.position);
            //移动玩家
			other.transform.GetComponent<Player>().MoveNextRoom(CameraControl.Instance.transform.position,transform.position);
           
        }
    }

    
}
