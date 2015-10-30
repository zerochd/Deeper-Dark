using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Globe{

    public static string sceneName;

	//是否有遇到boss
	public static bool MeetBoss = false;

	public static GAME_STATE game_state = GAME_STATE.STOP;

	//静态对象
	public static List<GameObject> instanceGameObjects = new List<GameObject>();

	public enum GAME_STATE{
		STOP = 0,
		RUNNING,
		BAGING,
		MAPING,
		OPTION,
		OVER
	}
}
