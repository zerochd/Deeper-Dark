using UnityEngine;
using System.Collections;
using UnityEditor;

public class AssetbundlesMenuItems : MonoBehaviour {

	//tips:prefab打包会将prefab依赖的资源（脚本，图像）一起打包，所以打包的内容会比prefab本身要大

	[MenuItem("Custom Editor/Create AssetBunldes ITEMS")]
	static void CreateAssetBunldesALL ()
	{
		
		Caching.CleanCache ();
		
		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);

		if(SelectedAsset.Length == 0){
			Debug.LogWarning("no selectAsset");
			return ;
		}

		AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
		buildMap[0].assetBundleName = "ITEMS";
		string[] assets = new string[SelectedAsset.Length];
		int tempIndex = 0;
		foreach (Object obj in SelectedAsset) 
		{

			string path = AssetDatabase.GetAssetPath(obj);
			// 过滤掉meta文件和文件夹
			if(path.Contains(".meta") || path.Contains(".") == false)
				continue;
//			Debug.Log ("Create AssetBunldes name :" + path);
			assets[tempIndex++] = path;
		}
		buildMap[0].assetNames = assets;


//		assets[0]="Assets/Prefab/Item/equip_light.prefab";
//		assets[1]="Assets/Prefab/Item/med_HP_small.prefab";
//		buildMap[0].assetNames = assets;

		//outputpath要求当前项目路径下必须有同名的文件夹，同时在该文件夹中生成同名的文件
		if(BuildPipeline.BuildAssetBundles("Assets/ABs",buildMap,BuildAssetBundleOptions.UncompressedAssetBundle,BuildTarget.StandaloneWindows))
		//刷新资源管理器
		AssetDatabase.Refresh ();
	}
}
