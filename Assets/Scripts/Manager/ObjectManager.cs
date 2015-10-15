
using System.Collections.Generic;
using UnityEngine;

class ObjectManager : MonoBehaviour
{
	//--------------------------------------------------
	// 变量

	public BaseObject[] m_initialDeactiveObjects = null;

	private Dictionary< string, BaseObject > m_deactiveObjects = new Dictionary< string, BaseObject >(); 

	//-------------------------------------------------
	//方法
	

	private void Start()
	{
		foreach(BaseObject bo in m_initialDeactiveObjects)
		{
			deactivate( bo );
		}
	}

	public BaseObject find(string name)
	{
	

		//先从Hierarchy中寻找bo
		GameObject go = GameObject.Find( name );
		BaseObject bo = ( go != null ) ? go.GetComponent< BaseObject >() : null;

		//之前找不到bo的话就从m_deactiveObjects中尝试获取bo
		if ( bo == null )
		{

			if ( !m_deactiveObjects.TryGetValue( name, out bo ) )
			{

				return null;
			}
		}
		

		return bo;

	}

	//设置非活动状态
	public bool deactivate( BaseObject baseObject )
	{
		BaseObject boInDictionary;
		if ( m_deactiveObjects.TryGetValue( baseObject.name, out boInDictionary ) )
		{

			
			if ( baseObject == boInDictionary )
			{
				//名字相同，对象也一样，提示警告
				Debug.LogWarning( "\"" + baseObject.name + "\" has already deactivated." );
				
				baseObject.gameObject.SetActive( false );	
				return true;
			}
			else
			{
				//名字相同，但对象不相同，出错
				Debug.LogError( "There is already a same name object in the dictionary." );
				return false;
			}
		}
		//设置baseObject为非活动状态
		baseObject.gameObject.SetActive( false );
		//将baseObject添加到dictionary
		m_deactiveObjects.Add( baseObject.name, baseObject );
		return true;
	}

	//设置活动状态
	public bool activate(BaseObject baseObject)
	{
		//当且仅当dictionary中含有baseObject的时候才将该物体改为活动状态
		if ( m_deactiveObjects.ContainsKey( baseObject.name ) )
		{
			baseObject.gameObject.SetActive( true );
			m_deactiveObjects.Remove( baseObject.name );
			
			return true;
		}

		//当在dictionary中找不到的话就警告该物体并没有设置成deactivated
		Debug.LogWarning( "\"" + baseObject.name + "\" is NOT deactivated." );
		return false;
	}

}