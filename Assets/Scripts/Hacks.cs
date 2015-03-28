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

	public static Object LoadPrefab(string path) {
		return UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
	}

	public delegate bool RaycastMatches(RaycastHit hit);
	public static RaycastHit GetRaycastClosest(Ray ray, Hacks.RaycastMatches matches=null) {
		RaycastHit[] hits;
		RaycastHit best_hit = default(RaycastHit);
		float min_distance;
		bool found;
		
		hits = Physics.RaycastAll(ray);
		if (hits.Length == 0) {
			return default(RaycastHit);
		}
		
		found = false;
		min_distance = float.MaxValue;
		foreach (RaycastHit hit in hits) {
			if ((matches != null) && !matches(hit)) {
				continue;
			}
			
			if (!found) {
				best_hit = hit;
				min_distance = hit.distance;
				found = true;
			} else if (hit.distance < min_distance) {
				best_hit = hit;
				min_distance = hit.distance;
			}
		}
		
		if (!found) {
			return default(RaycastHit);
		}
		
		return best_hit;
	}
}
