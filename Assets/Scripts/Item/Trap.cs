using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	public TRAP_TYPE trapType = TRAP_TYPE.ROCK;


	public enum TRAP_TYPE{
		NONE = -1,
		ROCK = 0,//落石
		HOLE	 //落穴
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag.Equals("Player")){
			Debug.Log ("Enter "+ trapType.ToString());
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.tag.Equals("Player")){
			Debug.Log ("Stay "+ trapType.ToString());
		}
	}
}
