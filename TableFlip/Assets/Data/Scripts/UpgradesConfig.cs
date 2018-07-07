using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesConfig : ScriptableObject {
	private static UpgradesConfig instance = null;
	public static UpgradesConfig Instance {
		get {
			if (instance == null) {
				instance = Resources.Load<UpgradesConfig>("Configs/UpgradesConfig");
			}
			return instance;
		}
	}

	public List<Upgrades> upgrades;
}
