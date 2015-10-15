using UnityEngine;
using System.Collections;

public class ItemAdapter: MonoBehaviour {

	//道具类型
	public ITEM_TYPE itemType = ITEM_TYPE.NONE;

	//道具参数
	Item item;

	BoxCollider2D boxCollider2D;

	public AudioClip pickUpSound;

	public enum ITEM_TYPE{
		NONE = -1,
		MED_HP_SMALL = 0,
		MED_HP_BIG = 1,
		EQUIP_LIGHT
	}


	// Use this for initialization
	void Start () {

		boxCollider2D = GetComponent<BoxCollider2D>();

		boxCollider2D.enabled = false;

		//根据选择的道具类型决定实例化什么道具属性
		switch(itemType){
			case ITEM_TYPE.MED_HP_SMALL:item = Med_HP_Small.CreateInstance();break;
			case ITEM_TYPE.EQUIP_LIGHT:item = Equip_Light.CreateInstance();break;
		}

		//调用实例化的item的strat方法
//		item.start();
		StartCoroutine(BornMove());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//初始抛物线移动	
	IEnumerator BornMove(){
		Vector3 moveEnd = new Vector3(this.transform.position.x + 1f,this.transform.position.y,this.transform.position.z);
		float sqrRemainingDistance = Mathf.Abs(moveEnd.x - this.transform.position.x);
		float damp = 1.5f;
		while(sqrRemainingDistance >= float.Epsilon){
			Vector3 newPosition = Vector3.MoveTowards(this.transform.position, moveEnd, 4f * Time.deltaTime);
			newPosition.y += damp * Time.deltaTime;
			damp -= 2f * Time.deltaTime;
			transform.position = newPosition;
			sqrRemainingDistance = Mathf.Abs(moveEnd.x - this.transform.position.x);
			yield return null;
		}
		//当初始移动完成开启触发器
		boxCollider2D.enabled = true;
	}

	//当碰触到玩家时，将该物品添加到道具里,并销毁该物体
	void OnTriggerEnter2D(Collider2D other){
		if(other.tag.Equals("Player")){
//			Debug.Log("attach Player");

//			Bag.instance.AddBag(item);

			Bag.instance.AddBag(item,GetComponent<SpriteRenderer>().sprite);

			//在画面底部显示得到文字
			TextManager.instance.showMessage("获得了"+item.name);

			//播放一次音效
			AudioSource.PlayClipAtPoint(pickUpSound,Vector3.zero);

			Destroy(this.gameObject);

		}
	}

	//设置出生方向
//	void SetBornDirection(bool direction){
//	
//	}


}
