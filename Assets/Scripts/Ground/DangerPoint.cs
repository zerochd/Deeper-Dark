using UnityEngine;
using System.Collections;

public class DangerPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		Invoke("DangerPointDisable",0.5f);
		Destroy(this.gameObject,0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
//	void DangerPointDisable(){
//
//		gameObject.SetActive(false);
//		Destroy(this.gameObject);
//	}
}
