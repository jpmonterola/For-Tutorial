using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
////using RogoDigital.Lipsync;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

[CustomEditor(typeof(MallConfigEditor))]
public class MallConfigEditor : EditorWindow {

    private enum CurrentWindow {
        MainMenu, //0
        CreateAStore, // 1.5
        StoreList, // 1
        StoreEditor, //2
        ItemEditor //2.5
    }
    private CurrentWindow currentWindow;
    private MallStore mallStore = new MallStore();

    private ReorderableList storeReordableList;
    private ReorderableList storeItemReordableList;
    private int storeIndexToEdit;
    private bool boughtOnceOnly = false;

    private MallStoreItemConfig.Item item;

    private struct EditMallStoreItem {
        public int index;
        public MallStoreItemConfig mallStoreItemConfig;
    }
    private EditMallStoreItem mallItemToEdit;

    private Vector2 scrollPos;
    static MallConfigEditor window;

    
    // Add menu named "My Window" to the Window menu
    [MenuItem("Custom/View/Mall Config", false, 10)]
    static void Init() {
        // Get existing open window or if none, make a new one:
        window = (MallConfigEditor)EditorWindow.GetWindow(typeof(MallConfigEditor));
        window.Show();
    }

    private void OnEnable() {
		currentWindow = CurrentWindow.StoreList;

        StoreReordableListInit();
    }

    private void StoreReordableListInit() {
		storeReordableList = new UnityEditorInternal.ReorderableList(MallConfig.Instance.mallStores, typeof(MallStore), true, false, true, true);
        storeReordableList.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Store List");
        };

        //Check if have items to show
        if (MallConfig.Instance.mallStores.Count <= 0) return;

        storeReordableList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                MallStore store = MallConfig.Instance.mallStores[index];
                store.indexID = index;
                rect.y += 2;
                if (GUI.Button(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), store.storeName)) {
                    storeIndexToEdit = index;
                    StoreItemReordableListInit();
                    currentWindow = CurrentWindow.StoreEditor;
                }
            };

        storeReordableList.onAddCallback = (ReorderableList l) => {
            currentWindow = CurrentWindow.CreateAStore;

        };
    }

    private void StoreItemReordableListInit() {
        if (MallConfig.Instance.mallStores[storeIndexToEdit].mallStoreItemConfig.itemsSelling == null) MallConfig.Instance.mallStores[storeIndexToEdit].mallStoreItemConfig.itemsSelling = new List<MallStoreItemConfig.Item>();

        storeItemReordableList = new UnityEditorInternal.ReorderableList(MallConfig.Instance.mallStores[storeIndexToEdit].mallStoreItemConfig.itemsSelling, typeof(MallStoreItemConfig.Item), true, true, true, true);
        storeItemReordableList.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Store Item List");
        };

        //Check if have items to show
        
        if (MallConfig.Instance.mallStores[storeIndexToEdit].mallStoreItemConfig.itemsSelling.Count <= 0) return;

        storeItemReordableList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                rect.y += 2;
                if (GUI.Button(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), MallConfig.Instance.mallStores[storeIndexToEdit].mallStoreItemConfig.itemsSelling[index].itemName)) {
                    mallItemToEdit.index = index;
                    MallStore store = MallConfig.Instance.mallStores[storeIndexToEdit];
                    MallStoreItemConfig originalMallStoreItemConfig = Resources.Load(string.Format("Configs/MallStores/Store_{0}", store.storeName)) as MallStoreItemConfig;

                    if (item == null) item = new MallStoreItemConfig.Item();
                    item.itemName = originalMallStoreItemConfig.itemsSelling[mallItemToEdit.index].itemName;
                    item.itemPrice = originalMallStoreItemConfig.itemsSelling[mallItemToEdit.index].itemPrice;
                    item.itemImage = originalMallStoreItemConfig.itemsSelling[mallItemToEdit.index].itemImage;
                    item.itemStock = originalMallStoreItemConfig.itemsSelling[mallItemToEdit.index].itemStock;

                    currentWindow = CurrentWindow.ItemEditor;
                }
            };
    }

    public void OnGUI() {
        switch (currentWindow) {
            case CurrentWindow.MainMenu:
                MainMenu();
                break;
            case CurrentWindow.CreateAStore:
                CreateAStore();
                break;
            case CurrentWindow.StoreList:
                StoreList();
                break;
            case CurrentWindow.StoreEditor:
                StoreEditor();
                break;
            case CurrentWindow.ItemEditor:
                ItemEditor();
                break;
        }
     }

    private void CreateAStore() {
        CreateHeaderBox("STORE CREATION", new Color32(51, 145, 255, 255));

        MallConfig mallConfig = MallConfig.Instance;
        //mallStore.storeID = EditorGUI.IntField(new Rect(10, 25, position.width - 20, 20), "StoreID:", mallStore.storeID);
        mallStore.storeName = EditorGUI.TextField(new Rect(10, 25, position.width - 20, 20), "Store Name:", mallStore.storeName);
        mallStore.storeLogo = (Sprite)EditorGUI.ObjectField(new Rect(10, 50, 240, 240), "Store Logo:", mallStore.storeLogo, typeof(Sprite), false);

        EditorGUILayout.BeginHorizontal();
        {
            if (GUI.Button(new Rect(10, 150, position.width - 20, 20), "Create Store")) {
                MallStoreItemConfig mallStoreItemConfig = new MallStoreItemConfig();
                string savePath = string.Format("Assets/Reference/Resources/Configs/MallStores/Store_{0}.asset", mallStore.storeName);
                AssetDatabase.CreateAsset(mallStoreItemConfig, savePath);
                mallStore.mallStoreItemConfig = Resources.Load(string.Format("Configs/MallStores/Store_{0}", mallStore.storeName)) as MallStoreItemConfig;
                mallConfig.mallStores.Add(mallStore);
                mallStore = new MallStore();
                currentWindow = CurrentWindow.StoreList;
            }

            if (GUI.Button(new Rect(10, 175, position.width - 20, 20), "Back")) {
                currentWindow = CurrentWindow.StoreList;
            }
        }
       
    }

    private void StoreList() {
        CreateHeaderBox("PAYAMAN STORE EDITOR", new Color32(51, 145, 255, 255));

        storeReordableList.DoLayoutList();
    }

    private void MainMenu() {
        EditorGUILayout.BeginVertical();
        {
            CreateHeaderBox("MAIN MENU", new Color32(51, 145, 255, 255));
            if (GUI.Button(new Rect(10, 35, position.width - 20, 20), "Create A Store")) {
                currentWindow = CurrentWindow.CreateAStore;
            }
            if (GUI.Button(new Rect(10, 60, position.width - 20, 20), "Store List")) {
                currentWindow = CurrentWindow.StoreList;
            }
        }
    }

    private void StoreEditor() {
        CreateHeaderBox("STORE EDITOR", new Color32(51, 145, 255, 255));

        MallConfig mallConfig = MallConfig.Instance;
        MallStore store = new MallStore();
        for (int i = 0; i < mallConfig.mallStores.Count; i++) {
            if (i == storeIndexToEdit) {
                store = mallConfig.mallStores[i];
            }
        }

        //store.storeID = EditorGUI.IntField(new Rect(10, 25, position.width - 20, 20), "StoreID:", store.storeID);
        store.storeName = EditorGUI.TextField(new Rect(10, 25, position.width - 20, 20), "Store Name:", store.storeName);
        store.storeLogo = (Sprite)EditorGUI.ObjectField(new Rect(10, 50, 240, 240), "Store Logo:", store.storeLogo, typeof(Sprite), false);

        if (GUI.Button(new Rect(10, 155, (position.width / 2) - 20, 20), "Save")) {
            EditorUtility.SetDirty(mallConfig);
            currentWindow = CurrentWindow.StoreList;
        }
        if (GUI.Button(new Rect(10 + (position.width / 2), 155, (position.width / 2) - 20, 20), "Back")) {
            currentWindow = CurrentWindow.StoreList;
        }

        //STORE ITEMS
        EditorGUILayout.BeginVertical();
        {
            GUILayout.BeginArea(new Rect(10, 230, position.width - 20, position.height));
            CreateHeaderBox("LIST OF ITEMS ON THE STORE", new Color32(112, 193, 54, 255));
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(10, 250, position.width - 20, position.height));
            storeItemReordableList.DoLayoutList();
            GUILayout.EndArea();
        }

        
    }

    private void ItemEditor() {
        EditorGUILayout.BeginVertical();
        {
            CreateHeaderBox("ITEM EDITOR", new Color32(51, 145, 255, 255));

            MallStore store = MallConfig.Instance.mallStores[storeIndexToEdit];
            MallStoreItemConfig originalMallStoreItemConfig = Resources.Load(string.Format("Configs/MallStores/Store_{0}", store.storeName)) as MallStoreItemConfig;

            item.itemName = EditorGUI.TextField(new Rect(10, 30, position.width - 20, 20), "Item Name:", item.itemName);
            item.itemPrice = EditorGUI.FloatField(new Rect(10, 55, position.width - 20, 20), "Item Price:", item.itemPrice);
            item.itemImage = (Sprite)EditorGUI.ObjectField(new Rect(10, 85, 240, 240), "Item Image:", item.itemImage, typeof(Sprite), false);
            boughtOnceOnly = EditorGUI.Toggle(new Rect(10, 180, position.width - 20, 20), "Buyable Once Only :", boughtOnceOnly);

            int addYPos = 0;
            if (!boughtOnceOnly) {
                addYPos = 25;
                item.itemStock = EditorGUI.IntField(new Rect(10, 175 + addYPos, position.width - 20, 20), "Item Stock:", item.itemStock);
            } else {
                item.itemStock = 1;
            }
           
            if (GUI.Button(new Rect(10, 205 + addYPos, position.width - 20, 20), "Update Item")) {
                originalMallStoreItemConfig.itemsSelling[mallItemToEdit.index] = item;
                EditorUtility.SetDirty(originalMallStoreItemConfig);
                currentWindow = CurrentWindow.StoreEditor;
            }
            if (GUI.Button(new Rect(10, 230 + addYPos, position.width - 20, 20), "Back")) {
                currentWindow = CurrentWindow.StoreEditor;
            }
        }
    }

    private Texture2D MakeTex(int width, int height, Color col) {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i) {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private void CreateHeaderBox(string headerText, Color32 color) {
        GUIStyle currentStyle = null;
        currentStyle = new GUIStyle(GUI.skin.box);
        currentStyle.normal.background = MakeTex(10, 10, color);
        currentStyle.normal.textColor = Color.white;
        currentStyle.fontStyle = FontStyle.Bold;
        GUILayout.Box(headerText, currentStyle, GUILayout.Width(position.width - 10));
    }

    //CREATE ENUM FILE
    /*private void CreateEnumList() {
        string dataScriptsPath = string.Format("{0}/Resources/Data/SpecialCongWords.cs", Application.dataPath);
        string scriptTemplateTexts = string.Format("{0}/Resources/ScriptTemplateTexts/SpecialWordEnumTemplate.txt", Application.dataPath);

        using (StreamWriter writer = new StreamWriter(dataScriptsPath)) {

            string newVariablesScript;

            using (TextReader reader = File.OpenText(scriptTemplateTexts)) {
                newVariablesScript = reader.ReadToEnd();
            }

            StringBuilder builder = new StringBuilder();

            List<IdleConfig.IdleCollection> wordCollection = new List<IdleConfig.IdleCollection>();
            for (int index = 0; index < SpecialWordConfig.Instance.SpecialWordCollection.Count; index++) {
                IdleConfig.IdleCollection word = IdleConfig.Instance.SpecialWordCollection[index];
                if (word.id != "") wordCollection.Add(word);
            }

            for (int index = 0; index < wordCollection.Count; index++) {
                IdleConfig.IdleCollection myWord = wordCollection[index];
                //EVENTS
                string endString = (index + 1 == wordCollection.Count) ? "" : ",";
                builder.Append("\t\t" + myWord.id.ToString().Replace(" ", "") + endString).AppendLine();
            }

            newVariablesScript = newVariablesScript.Replace("//ENUMS", builder.ToString());
            writer.Write(newVariablesScript);
        }
    }*/
}
