using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesItemConfig : ScriptableObject {

	[System.Serializable]
	public class UpgradesItem {
		public string name;
		public Sprite logo;
	}

	public List<UpgradesItem> items;
}
