using UnityEngine;
using System;

public class GlobalParam : MonoBehaviour {

	//-------------------------------------
	//变量

	private string[] startScripts;

	//单例变量
	private static GlobalParam instance = null;


	//-------------------------------------
	//方法

	//public
	public string[] getStartScriptFiles()
	{
		return startScripts;
	}

	public void setStartScriptFiles(params string[] files)
	{
		startScripts = new string[files.Length];
		Array.Copy(files,startScripts,files.Length);
	}

	//private
	private void create()
	{
		startScripts = new string[1];
		startScripts[0] = "Assets/Event/textEvent.txt";
	}

	//static
	public static GlobalParam getInstance()
	{
		if(instance == null)
		{
			GameObject go = new GameObject( "GlobalParam" );
			instance = go.AddComponent< GlobalParam >();
			instance.create();
			DontDestroyOnLoad(go);
		}
		return instance;
	}
}
