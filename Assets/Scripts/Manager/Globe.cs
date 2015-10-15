using UnityEngine;
using System.Collections;

public class Globe{

    public static string sceneName;

	public static GAME_STATE game_state = GAME_STATE.STOP;

	public enum GAME_STATE{
		STOP = 0,
		RUNNING,
		BAGING,
		MAPING
	}
}
