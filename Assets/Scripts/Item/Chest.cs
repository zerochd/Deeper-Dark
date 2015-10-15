using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {
	

	private bool isOpened = false;

	//宝箱打开音效
	public AudioClip chestisOpenedClip;

	//宝箱打开图像
	public Sprite treasureisOpened;

	//宝箱里的道具
	public GameObject[] itemInChest;


	void OnTriggerEnter2D(Collider2D other){
	
		//检测到攻击判定且未被打开,转换sprite,播放音效,设置为打开状态
		if(other.name.Equals("attackPoint") && isOpened == false){
			GetComponent<SpriteRenderer>().sprite = treasureisOpened;
//			SoundManager.instance.PlaySingle("background",chestisOpenedClip);
			isOpened = true;
			GameObject item = GetRandomItem();
			GameObject go = Instantiate(item, this.transform.position, this.transform.rotation) as GameObject;
			go.name = item.name;
			if(!go.GetComponent<ItemAdapter>()){
				Debug.LogError("no ItemAdapter");
			}

		}
	}

	GameObject GetRandomItem(){
		if(itemInChest.Length == 0){
			Debug.LogError("no Item");
		}
		GameObject item = itemInChest[UnityEngine.Random.Range(0, itemInChest.Length)];
		return item;
	}




	
	

}
