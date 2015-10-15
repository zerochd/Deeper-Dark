using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource enemySource;
	public AudioSource playerSource;
	public AudioSource bkSoundSource;

//	public AudioSource musicSource;
	public static SoundManager instance = null;
	
	public float lowPitchRange = 0.95f;
	public float highPitchRange = 1.05f;
	
	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
		
	}
	
	public void PlaySingle(string characterType,AudioClip clip){
		switch(characterType)
		{

			case "player":
				playerSource.clip = clip;
				playerSource.Play();
				break;

			case "enemy":
				enemySource.clip = clip;
				enemySource.Play();
				break;

			case "background":
				bkSoundSource.clip = clip;
				bkSoundSource.Play();
				break;

			default:
			Debug.Log ("no this characterType");break;

		}

	}
	
	public void RandomizeSfx(string characterType,params AudioClip[] clips){
		int randomIndex = Random.Range (0, clips.Length);

//		float randomPitch = Random.Range (lowPitchRange, highPitchRange);

		PlaySingle(characterType,clips [randomIndex]);
		
	}
}
