using UnityEngine;
using System.Collections;

public class Hacks {
	public static string LEVEL_GLOBALS = "level_globals";

	public class MouseButton {
		/* should be enum, but this particular api sucks and we don't want to cast */
		public const int LEFT   = 0;
		public const int RIGHT  = 1;
		public const int MIDDLE = 2;
	};

	public static Object LoadPrefab(string path)
	{
		return UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
	}
}
