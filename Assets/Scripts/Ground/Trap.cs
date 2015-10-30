using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	public TRAP_TYPE trapType = TRAP_TYPE.ROCK;

	//落石对象
	public Rock rock;

	//危险提示号
	public GameObject dangerPoint;

	private bool isUse = false;

	public enum TRAP_TYPE{
		NONE = -1,
		ROCK = 0,//落石
		HOLE	 //落穴
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag.Equals("Player")){
//			Debug.Log ("Enter "+ trapType.ToString());
			//开启陷阱提示
//			this.GetComponent<SpriteRenderer>().color = Color.red;
			//开启陷阱效果
			if(!isUse){
				isUse = true;
				switch(trapType){
				case TRAP_TYPE.ROCK:Instantiate(rock,GetRandromVec(), this.transform.rotation);break;
				default:Debug.LogError("no this type");break;
				}
			}
		}
	}


	void DangerPointDisable(){
		dangerPoint.SetActive(false);
	}

	Vector3 GetRandromVec(){
		float x = this.transform.position.x + Random.Range(-0.5f,0.5f);
		float y = this.transform.position.y + Random.Range(-0.5f,0.5f);
		return new Vector3(x,y,0f);
	}

}
