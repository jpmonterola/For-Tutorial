////using RogoDigital.Lipsync;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SpecialWordType {
    special
}

public class SpecialWordConfig : ScriptableObject {
    /*private static SpecialWordConfig instance;
    public static SpecialWordConfig Instance {
        get {
            return Resources.Load<SpecialWordConfig>("Configs/SpecialWordConfig");
        }
    }

    [SerializeField]
    private TextAsset specialWordList;

    private string[] specialWords;

    [System.Serializable]
    public class WordCollection {
        public string id = "";
        public LipSyncData lipSyncData;
        public string word = "";
        public PlacardGestureType placardGestureType;
        public int gestureTypeIndex;
        public bool showPosition;
        public bool isTestPlayable; //EditorOnly
    }

    [SerializeField, HideInInspector]
    private List<WordCollection> specialWordCollection;

    public List<WordCollection> SpecialWordCollection { get { return specialWordCollection; } }


    public int WordCount(SpecialWordType specialWordType) {
        switch (specialWordType) {
            case SpecialWordType.special:
                return specialWords != null ? specialWords.Length : 0;
        }
        return specialWords != null ? specialWords.Length : 0;
    }

    public string GetWord(SpecialWordType specialWordType, int wordIdx) {
        switch (specialWordType) {
            case SpecialWordType.special:
                if (specialWords == null || wordIdx < 0 || wordIdx >= specialWords.Length) return string.Empty;
                return specialWords[wordIdx];
        }
        if (specialWords == null || wordIdx < 0 || wordIdx >= specialWords.Length) return string.Empty;
        return specialWords[wordIdx];
    }

    public WordCollection GetRandomWord() {
        List<WordCollection> fullWordCollection = new List<WordCollection>(SpecialWordCollection.Count);
        fullWordCollection.AddRange(SpecialWordCollection);

#if UNITY_EDITOR
        fullWordCollection = RemoveNonTestWords(fullWordCollection);
#endif
        int dice = Random.Range(0, fullWordCollection.Count);
        return fullWordCollection[dice]; //if null send index 0 data
    }

    private List<WordCollection> RemoveNonTestWords(List<WordCollection> fullWordCollection) {
        List<WordCollection> fullWordCollectionClean = new List<WordCollection>();
        for (int index = 0; index < fullWordCollection.Count; index++) {
            if (fullWordCollection[index].isTestPlayable) fullWordCollectionClean.Add(fullWordCollection[index]);
        }
        return fullWordCollectionClean;
    }

    public List<WordCollection> GetWordCollection(SpecialWordType specialWordType) {
        switch (specialWordType) {
            case SpecialWordType.special:
                return specialWordCollection;
        }

        return specialWordCollection;
    }

    public WordCollection GetWordCollectionSpecific(SpecialWordType specialWordType, string id) {
        List<WordCollection> wordCollection = new List<WordCollection>();
        switch (specialWordType) {
            case SpecialWordType.special:
                wordCollection = specialWordCollection;
                break;
        }

        for (int index = 0; index < wordCollection.Count; index++) {
            if (wordCollection[index].id == id) {
                return wordCollection[index];
            }
        }
        return null;
    }

    public List<string> GenerateList(SpecialWordType specialWordType) {
        switch (specialWordType) {
            case SpecialWordType.special:
                return specialWords.ToList();
        }
        return specialWords.ToList();
    }

    public string GetCongWord(SpecialWordType specialWordType, string id) {
        List<WordCollection> wordCollection = new List<WordCollection>();
        switch (specialWordType) {
            case SpecialWordType.special:
                wordCollection = specialWordCollection;
                break;
        }

        for (int index = 0; index < wordCollection.Count; index++) {
            if (wordCollection[index].id == id) {
                return wordCollection[index].word;
            }
        }
        return null;
    }

    public LipSyncData GetCongLipSynData(SpecialWordType specialWordType, string id) {
        List<WordCollection> wordCollection = new List<WordCollection>();
        switch (specialWordType) {
            case SpecialWordType.special:
                wordCollection = SpecialWordCollection;
                break;
        }

        for (int index = 0; index < wordCollection.Count; index++) {
            if (wordCollection[index].id == id) {
                Debug.LogFormat("<color=green>Found id {0}</color>", id);
                return wordCollection[index].lipSyncData;
            }
        }
        Debug.LogError(string.Format("Cannot find specified id {0}", id));
        return null;
    }

    public bool HasWord(string word, List<WordCollection> wordCollection) {
        for (int index = 0; index < wordCollection.Count; index++) {
            if (word == wordCollection[index].word) {
                return true;
            }
        }
        return false;
    }

    public void Initialize() {
        specialWords = specialWordList != null ? specialWordList.text.Split('\n') : new string[0];

        if (specialWordCollection.Count <= 0) specialWordCollection = new List<WordCollection>();
        for (int index = 0; index < specialWords.Length; index++) {
            if (!HasWord(specialWords[index], specialWordCollection)) {
                //ADD
                WordCollection wordCollection = new WordCollection();
                wordCollection.id = "";
                wordCollection.lipSyncData = new LipSyncData();
                wordCollection.word = GetWord(SpecialWordType.special, index);
                specialWordCollection.Add(wordCollection);
            }
        }

        for (int index = 0; index < specialWordCollection.Count; index++) {
            if (specialWordCollection[index].word == null || specialWordCollection[index].word == "") {
                specialWordCollection.RemoveAt(index);
            }
        }

        //Remove duplicate
        RemoveDuplicate(specialWordCollection);
      
        //Remove non existential items
        RemoveNonExistential(specialWordCollection, specialWords);
    }

    public void RemoveDuplicate(List<WordCollection> wordCollection) {
        for (int index = 0; index < wordCollection.Count; index++) {
            for (int index2 = 0; index2 < wordCollection.Count; index2++) {
                if (index == index2) continue;
                if (CleanString.UseRegex(wordCollection[index].word) == CleanString.UseRegex(wordCollection[index2].word)) {
                    wordCollection.RemoveAt(index2);
                }
            }
        }
    }

    public void RemoveNonExistential(List<WordCollection> wordCollection, string[] words) {
        for (int count = wordCollection.Count - 1; count >= 0; count--) {
            bool hasWord = false;
            for (int index = 0; index < words.Length; index++) {
                if (CleanString.UseRegex(wordCollection[count].word) == CleanString.UseRegex(words[index])) hasWord = true;
            }
            if (!hasWord) {
                wordCollection.RemoveAt(count);
            }
        }
    }*/
}
