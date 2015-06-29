using UnityEngine;
using System.Collections;

public class Enemy : AllCharacter
{
    private bool hasSpawn;
	// Use this for initialization
	void Start () {

        hasSpawn = false;
        //关闭一切组件
	}
	
	// Update is called once per frame
	void Update () {
        if (hasSpawn == false)
        {
            if (GetComponent<Renderer>().IsVisibleFrom(Camera.main))
            {
                Spawn();
            }
        }
        else
        {
            if (GetComponent<Renderer>().IsVisibleFrom(Camera.main) == false)
            {
                Destroy(gameObject);
            }
        }
	}

    void Spawn()
    {
        hasSpawn = true;
        this.gameObject.SetActive(true);
        //开启一切组件
    }
}
