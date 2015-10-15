using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class ScriptParse : MonoBehaviour {


	//------------------------
	//方法


	//解析并生成事件
	public Event[] parseAndCreateEvents(string[] lines){

		//判断是否在Begin和End之间,如果在之间应该为true
		bool isInsideOfBlock = false;
		//过滤空格(1个或多个)
		Regex tabSplitter = new Regex("\\t+");
		//存放指令行
		List<string> commandLines = new List<string>();
		//存放指令行所在的行号
		List<int> commandLineNumbers = new List<int>();
		//存放事件
		List<Event> events = new List<Event>();

		string eventName = "";

		int lineCount = 0;
		//记录起始指令行号
		int beginLineCount = 0;

		//读取每一行直到最后
		foreach(string line in lines){
		
			lineCount++;

			//除去注释，例如;;;;;之类的
			int index = line.IndexOf(";;");
			string str = index < 0 ? line : line.Substring(0,index);
			str = str.Trim();

			if(str.Length < 1) continue;

			//获得事件名称
			if ( str.Length >= 3 )
			{
				if ( str[0] == '[' && str[ str.Length - 1 ] == ']' )
				{
					eventName = str.Substring( 1, str.Length - 2);
				}
			}

			//判断指令名称
			switch(str.ToLower())
			{
			//当检测到"begin"时，设置beginLineCount
			case "begin":
				if(isInsideOfBlock)
				{
					Debug.LogError("Unclosed Event (" +beginLineCount+ ")");
					return new Event[0];
				}
				beginLineCount = lineCount;
				isInsideOfBlock = true;
				break;

			//当检测到"end"时，
			case "end":

				List<string[]> commands = new List<string[]>();

				//迭代commandlines，将一行语句根据空格分隔然后存放在tabSplit中
				//并将tabSplit添加至commands中
				foreach(string c1 in commandLines)
				{

					string[] tabSplit = tabSplitter.Split(c1);
//					Debug.Log ("param:"+ tabSplit[0]);
					commands.Add(tabSplit);
				}

				//清除指令条，并用填满的commands创建event
				commandLines.Clear();

				Event ev = createEvent(commands.ToArray(),eventName,commandLineNumbers.ToArray(),beginLineCount);


				//如果事件不为空，为这个事件设定起始行号并把他添加到event集中
				if(ev !=null)
				{

					ev.setLineNumber(beginLineCount);
					events.Add (ev);
				}

				//重新设置值
				commandLineNumbers.Clear ();
				eventName = "";
				isInsideOfBlock = false;
				break;

			//当处于中间时，
			default:
				if ( isInsideOfBlock )
				{
					//往commandLines中添加str
					commandLines.Add( str );
					//往commandLineNumbers中添加lineCount
					commandLineNumbers.Add( lineCount );
				}
				break;
			}

		}
		//当走完一遍正常的Begin-End的过程，isInsideOfBlock应该为false
		if ( isInsideOfBlock )
		{
			Debug.LogError( "Unclosed Event ("  + beginLineCount + ")" );
		}

		//返回一系列事件
		return events.ToArray();

	}

	//创建事件
	private Event createEvent(string[][] commands,string eventName,int[] numbers,int beginLineCount)
	{
		List<string> 			targets = new List<string>();
		List<EventCondition> 	conditions = new List<EventCondition>();
		List<string[]> 			actions = new List<string[]>();
		//存放actions的行号
		List<int> 				LineNumbers = new List<int>();

		bool isPrologue = false;
		bool doContinue = false;

		int i=0;


		//迭代每一条命令行
		foreach( string[] commandsParams in commands){
			//switch 每条指令第一个字符串
			switch(commandsParams[0].ToLower())
			{

			//target类型指令设定事件参与对象
			case "target":
				if( commandsParams.Length >= 2)
				{
//					Debug.Log("commandsParams[1]:"+commandsParams[1]);
					targets.Add(commandsParams[1]);
				}
				else
				{
					Debug.LogError("Failed to add a target");
				}
				break;

			//prologue==开场白
			case "prologue":
				isPrologue = true;
				break;

			//condition类型指令
			case "condition":
				//condition类型的内容都长于4
				if(commandsParams.Length >= 4)
				{
					//通过condition该行第二个字段找到gameObject
					GameObject go = GameObject.Find (commandsParams[1]);
					BaseObject bo = go!= null ? go.GetComponent<BaseObject>() : null;

					//如果找到的gameObject有BaseObject
					if( bo != null)
					{
						EventCondition ec = new EventCondition(bo,commandsParams[2],commandsParams[3]);
						conditions.Add(ec);
					}
					else
					{
						Debug.LogError("Failed to add a condition.");
					}
				}
				else
				{
					Debug.LogError("Failed to add a condition.");
				}
				break;

			case "continue":
				doContinue = true;
				break;
			
			//将其余的命令行都填充到actions里
			default:
				actions.Add(commandsParams);
				LineNumbers.Add(numbers[i]);
				break;
			}
			++i;

		}
			if(isPrologue){
				targets.Clear();
			}
			else
			{
				if(targets.Count < 2)
				{
					Debug.LogError( "Failed to create an event." );
					return null;
				}
			}

			if(actions.Count < 1)
			{
				//当行为少于1是不合乎逻辑的
				Debug.LogError("Failed to create an event at " + beginLineCount +".");
				return null;

			}

			//实例化新的Event
			Event ev = new Event(targets.ToArray(),conditions.ToArray(),actions.ToArray(),isPrologue,doContinue,eventName);
			ev.setActionLineNumbers(LineNumbers.ToArray());
			return ev;

	}


}
