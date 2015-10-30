using UnityEngine;
using System.Collections;

public class SearchBOSSAI : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		//		if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && !this.GetComponentInParent<Enemy>().IsFoundPlayer()){
		//
		//			this.transform.parent.GetComponent<Enemy>().HaveFoundPlayer(other.transform);
		//		}
		if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && !this.GetComponentInParent<Boss>().IsFoundPlayer()){
			this.transform.parent.GetComponent<Boss>().HaveFoundPlayer(other.transform);
		}
	}
	void OnTriggerStay2D(Collider2D other){
		//		if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && !this.GetComponentInParent<Enemy>().IsFoundPlayer()){
		//
		//			this.transform.parent.GetComponent<Enemy>().HaveFoundPlayer(other.transform);
		//		}
		if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && !this.GetComponentInParent<Boss>().IsFoundPlayer()){
			this.transform.parent.GetComponent<Boss>().HaveFoundPlayer(other.transform);
		}
	}
}
