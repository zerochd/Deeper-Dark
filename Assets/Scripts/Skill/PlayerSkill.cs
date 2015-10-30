using UnityEngine;
using System.Collections;

public class PlayerSkill : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Skill_Flash();
	}

	//瞬移
	void Skill_Flash(){

//		if(Input.GetKeyDown(KeyCode.Q)){
//			if(GetComponent<Player>()){
//				GetComponent<Rigidbody2D>().AddForce(Vector2.right *2500f,ForceMode2D.Force);
//			}
//			else{
//				Debug.LogError("no Player component");
//			}
//		}
	}
}
