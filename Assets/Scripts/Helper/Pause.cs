using UnityEngine;
using System.Collections;

//用于暂停Unity运行
public class Pause : MonoBehaviour {

    //一个bool变量来标志是否暂停
    bool isGamePause = false;

	// Use this for initialization
	void Start () {
        //不销毁这个gameObject
        Object.DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.P)){
            if(!isGamePause){
                Time.timeScale = 0.0f;
            }
            else{
                Time.timeScale = 1.0f;
            }
            isGamePause = !isGamePause;
        }
	
	}
}
