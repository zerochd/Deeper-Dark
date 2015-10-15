using UnityEngine;
using System;

class EventActorText : EventActor {

	//--------------------------------------
	//变量

	private string text;

	//----------------------------------------
	//方法


	public EventActorText(string text){
		this.text = text;
	}

	
	public override void start(EventManager evman)
	{
		TextManager tad = evman.gameObject.GetComponent<TextManager>();
		tad.showTint(this.text);
	}


	public static EventActorText CreateInstance( string[] parameters, GameObject manager )
	{
		if ( parameters.Length >= 1 )
		{
			// 生成EventActorText步骤
			EventActorText actor = new EventActorText( String.Join( "\n", parameters ) );
			return actor;
		}
		
		Debug.LogError( "Failed to create an actor." );
		return null;
	}


}
