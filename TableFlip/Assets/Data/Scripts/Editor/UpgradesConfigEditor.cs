using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(UpgradesConfigEditor))]
public class UpgradesConfigEditor : EditorWindow {

	private enum CurrentWindow{
		Null, //When there's no Upgradable Config (Lists of lists)
		UpgradesList, //Show the upgradeable config (lists of list)
		CreateUpgrades, //Create a list
		UpgradesEditor, //Edit the list
		ItemEditor // Create an item on the list
	}

	private CurrentWindow currentWindow;

	private ReorderableList upgradesReorderableList;
	private ReorderableList upgradesItemReorderableList;
	private int upgradesIndex;

	private Upgrades upgrades = new Upgrades();
	private struct EditUpgradesItem
	{
		public int index;
		public UpgradesItemConfig upgradeItemConfig;
	}
	private EditUpgradesItem editUpgradesItem;
	private UpgradesItemConfig.UpgradesItem upgradesItem;

	//static UpgradablesConfigEditor window;
	[MenuItem("Custom/Upgrades Config")]
	static void Init(){
		//window = (UpgradablesConfigEditor)EditorWindow.GetWindow (typeof(UpgradablesConfigEditor));
		//window.Show ();

		EditorWindow.GetWindow (typeof(UpgradesConfigEditor));
	}

	private void OnEnable(){
		if (UpgradesConfig.Instance == null) {
			currentWindow = CurrentWindow.Null;
		} else {
			currentWindow = CurrentWindow.UpgradesList;
			UpgradesReorderableListInit ();
		}
	}

	private void UpgradesReorderableListInit(){
		upgradesReorderableList = new ReorderableList (UpgradesConfig.Instance.upgrades, typeof(Upgrades), true, false, true, true);

		upgradesReorderableList.drawElementCallback = 
			(Rect rect, int index, bool isActive, bool isFocused) => {
			Upgrades upgrades = UpgradesConfig.Instance.upgrades [index];
			rect.y += 2;
			if (GUI.Button (new Rect (rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), upgrades.name)) {
				upgradesIndex = index;
				UpgradesItemReorderableListInit ();
				currentWindow = CurrentWindow.UpgradesEditor;
			}
		};
		upgradesReorderableList.onAddCallback = (ReorderableList l) => {
			currentWindow = CurrentWindow.CreateUpgrades;
		};
	}

	private void UpgradesItemReorderableListInit(){
		upgradesItemReorderableList = new UnityEditorInternal.ReorderableList(UpgradesConfig.Instance.upgrades[upgradesIndex].upgradesItemConfig.items, typeof(UpgradesItemConfig.UpgradesItem), true, false, true, true);

		upgradesItemReorderableList.drawElementCallback =
			(Rect rect, int index, bool isActive, bool isFocused) => {
			rect.y += 2;
			if (GUI.Button(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), UpgradesConfig.Instance.upgrades[upgradesIndex].upgradesItemConfig.items[index].name)) {
				editUpgradesItem.index = index;
				Upgrades upgrades = UpgradesConfig.Instance.upgrades[upgradesIndex];
				UpgradesItemConfig origUpgradesItemConfig = Resources.Load(string.Format("Configs/Upgrades/Type_{0}", upgrades.name)) as UpgradesItemConfig;

				if (upgradesItem == null) upgradesItem = new UpgradesItemConfig.UpgradesItem();
				upgradesItem.name = origUpgradesItemConfig.items[editUpgradesItem.index].name;
				upgradesItem.logo = origUpgradesItemConfig.items[editUpgradesItem.index].logo;

				currentWindow = CurrentWindow.ItemEditor;
			}
		};
	}


	public void OnGUI(){
		GUILayout.Box (currentWindow.ToString (), GUILayout.Width (position.width - 10));

		switch (currentWindow) {
		case CurrentWindow.Null:
			Null ();
			break;
		case CurrentWindow.UpgradesList:
			UpgradesList ();
			break;
		case CurrentWindow.CreateUpgrades:
			CreateUpgrades();
			break;
		case CurrentWindow.UpgradesEditor:
			UpgradesEditor ();
			break;
		case CurrentWindow.ItemEditor:
			ItemEditor ();
			break;
		}
	}

	private void Null(){
		GUILayout.BeginVertical ();
		{
			GUILayout.Box ("No Config Found", GUILayout.Width (position.width - 10));
			if (GUI.Button (new Rect (20, 75, position.width - 40, 30), "Create Config")) {
				UpgradesConfig upgradesConfig = new UpgradesConfig ();
				upgradesConfig.upgrades = new List<Upgrades> ();
				string savePath = "Assets/Data/Resources/Configs/UpgradesConfig.asset";
				AssetDatabase.CreateAsset (upgradesConfig, savePath);
				currentWindow = CurrentWindow.UpgradesList;
				UpgradesReorderableListInit ();
			}

		}
	}

	private void UpgradesList(){
		upgradesReorderableList.DoLayoutList ();
	}

	private void CreateUpgrades(){
		UpgradesConfig upgradesConfig= UpgradesConfig.Instance;

		upgrades.name = EditorGUI.TextField(new Rect(10, 25, position.width - 20, 20), "Upgrades Name:", upgrades.name);
		upgrades.logo = (Sprite)EditorGUI.ObjectField(new Rect(10, 50, 240, 240), "Upgrades Logo:", upgrades.logo, typeof(Sprite), false);

		EditorGUILayout.BeginHorizontal();
		{
			if (GUI.Button(new Rect(10, 150, position.width - 20, 20), "Create Upgrades")) {
				UpgradesItemConfig upgradesItemConfig = new UpgradesItemConfig ();
				upgradesItemConfig.items = new List<UpgradesItemConfig.UpgradesItem> ();
				string savePath = string.Format("Assets/Data/Resources/Configs/Upgrades/Type_{0}.asset", upgrades.name);
				AssetDatabase.CreateAsset(upgradesItemConfig, savePath);
				upgrades.upgradesItemConfig = Resources.Load (string.Format ("Configs/Upgrades/Type_{0}.asset", upgrades.name)) as UpgradesItemConfig;
				upgradesConfig.upgrades.Add (upgrades);
				upgrades = new Upgrades ();

				currentWindow = CurrentWindow.UpgradesList;
			}

			if (GUI.Button(new Rect(10, 175, position.width - 20, 20), "Back")) {
				currentWindow = CurrentWindow.UpgradesList;
			}
		}
	}

	private void UpgradesEditor(){
		UpgradesConfig upgradesConfig = UpgradesConfig.Instance;
		Upgrades upgrades = new Upgrades ();
		for (int i = 0; i < upgradesConfig.upgrades.Count; i++) {
			if (i == upgradesIndex) {
				upgrades = upgradesConfig.upgrades [i];
			}
		}

		upgrades.name = EditorGUI.TextField(new Rect(10, 25, position.width - 20, 20), "Upgrades Name: ", upgrades.name);
		upgrades.logo = (Sprite)EditorGUI.ObjectField(new Rect(10, 50, 240, 240), "Upgrades Logo:", upgrades.logo, typeof(Sprite), false);

		if (GUI.Button(new Rect(10, 155, (position.width / 2) - 20, 20), "Save")) {
			EditorUtility.SetDirty(upgradesConfig);
			currentWindow = CurrentWindow.UpgradesList;
		}
		if (GUI.Button(new Rect(10 + (position.width / 2), 155, (position.width / 2) - 20, 20), "Back")) {
			currentWindow = CurrentWindow.UpgradesList;
		}

		EditorGUILayout.BeginVertical();
		{
			
			GUILayout.BeginArea(new Rect(10, 250, position.width - 20, position.height));
			upgradesItemReorderableList.DoLayoutList();
			GUILayout.EndArea();
		}
	}

	private void ItemEditor(){
		EditorGUILayout.BeginVertical();
		{
			Upgrades upgrades = UpgradesConfig.Instance.upgrades [upgradesIndex];
			UpgradesItemConfig origUpgradesItemConfig = Resources.Load (string.Format ("Configs/Upgrades/Type_{0}", upgrades.name)) as UpgradesItemConfig;

			upgradesItem.name = EditorGUI.TextField(new Rect(10, 30, position.width - 20, 20), "Item Name:", upgradesItem.name);
			upgradesItem.logo = (Sprite)EditorGUI.ObjectField(new Rect(10, 85, 240, 240), "Item Image:", upgradesItem.logo, typeof(Sprite), false);

			if (GUI.Button(new Rect(10, 205, position.width - 20, 20), "Update Item")) {
				origUpgradesItemConfig.items[editUpgradesItem.index] = upgradesItem;
				EditorUtility.SetDirty(origUpgradesItemConfig);
				currentWindow = CurrentWindow.UpgradesEditor;
			}
			if (GUI.Button(new Rect(10, 230, position.width - 20, 20), "Back")) {
				currentWindow = CurrentWindow.UpgradesEditor;
			}
		}
	}

}



















