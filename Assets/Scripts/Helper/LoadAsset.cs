using UnityEngine;
using System.Collections;

public class LoadAsset : MonoBehaviour {

	//不同平台下StreamingAssets的路径是不同的
	public static readonly string PathURL =
		#if UNITY_ANDROID
		"jar:file://" + Application.dataPath + "!/assets/";
	#elif UNITY_IPHONE
	Application.dataPath + "/Raw/";
	#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	"file://" + Application.dataPath + "/ABs/";
	#else
	string.Empty;
	#endif

	void Start(){
//		StartCoroutine(LoadAssetBundle());
	}

	IEnumerator LoadAssetBundle(){
		string assetBundlePath = PathURL + "Abs";
		WWW mwww = WWW.LoadFromCacheOrDownload(assetBundlePath, 0);
		yield return mwww;
		if (!string.IsNullOrEmpty(mwww.error))
		{
			Debug.Log(mwww.error);
		}
		else
		{
			AssetBundle mab = mwww.assetBundle;
			AssetBundleManifest mainfest = (AssetBundleManifest)mab.LoadAsset("AssetBundleManifest");
			mab.Unload(false);
			string[] absName = mainfest.GetAllAssetBundles();
			string url = Application.dataPath + "/ABs/";
			foreach(string name in absName){
				//CreateFromFile的path不加file://
				AssetBundle ab = AssetBundle.CreateFromFile(url+name);
				GameObject[] gos = ab.LoadAllAssets<GameObject>();

				//给Chest的道具列表填充
				Chest.itemsList.AddRange(gos);

				//关闭AssetBundle但没有摧毁创建的对象和引用,假
				ab.Unload(false);
			}
//			WWW www = WWW.LoadFromCacheOrDownload(assetBundlePath, mainfest.GetAssetBundleHash("items"), 0);
//			WWW www = new WWW(PathURL+"items");
//			yield return www;
//			if (!string.IsNullOrEmpty(www.error))
//			{
//				Debug.Log(www.error);
//			}
//			else
//			{
//				AssetBundle ab = www.assetBundle;
//				Object[] ob =ab.LoadAllAssets();
//				Debug.Log ("count:"+ob[0].name);
//			}

		}
	}
}
