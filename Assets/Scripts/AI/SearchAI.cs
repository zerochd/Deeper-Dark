using UnityEngine;
using System.Collections;

public class SearchAI : MonoBehaviour {

	//当碰触到layer为Player的且之前没有找到玩家的情况下，设置已经找到玩家
	void OnTriggerEnter2D(Collider2D other){
//		if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && !this.GetComponentInParent<Enemy>().IsFoundPlayer()){
//
//			this.transform.parent.GetComponent<Enemy>().HaveFoundPlayer(other.transform);
//		}
		if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && !this.GetComponentInParent<Enemy>().IsFoundPlayer()){
			this.transform.parent.GetComponent<Enemy>().HaveFoundPlayer(other.transform.parent);
		}
	}

	//当碰触到layer为Player的且之前没有找到玩家的情况下，设置已经找到玩家
	void OnTriggerStay2D(Collider2D other){
		//		if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && !this.GetComponentInParent<Enemy>().IsFoundPlayer()){
		//
		//			this.transform.parent.GetComponent<Enemy>().HaveFoundPlayer(other.transform);
		//		}
		if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && !this.GetComponentInParent<Enemy>().IsFoundPlayer()){
			this.transform.parent.GetComponent<Enemy>().HaveFoundPlayer(other.transform.parent);
		}

		if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Super")) && this.GetComponentInParent<Enemy>().IsFoundPlayer()){
			this.transform.parent.GetComponent<Enemy>().HaveFoundPlayer(null);
		}
	}
	

	void OnTriggerExit2D(Collider2D other){
//		if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && this.GetComponentInParent<Enemy>().IsFoundPlayer()){
//			
//			this.transform.parent.GetComponent<Enemy>().HaveFoundPlayer(null);
//			Debug.Log ("miss");
//		}
		if(other.gameObject.CompareTag("Player") && !this.GetComponentInParent<Enemy>().IsFoundPlayer()){	
			this.transform.parent.GetComponent<Enemy>().HaveFoundPlayer(null);

//			Debug.Log ("miss");
		}
	}
}
