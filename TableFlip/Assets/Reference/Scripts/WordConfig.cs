////using RogoDigital.Lipsync;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WordType {
    yes,
    notsure,
    no
}

public enum PlacardGestureType {
    Default = 0,
    Paawer = 1,
    Ritwal = 2,
    yes_1 = 3,
    yes_2 = 4,
    Cong_Answering1 = 5,
    Cong_Answering2 = 6,
    gesture1 = 7,
    gesture2 = 8,
    gesture3 = 9,
    Cong_Duro = 10,
    Payaman = 11,
    hataw = 12,
    hoyhoyhoyChipsAhoy = 13,
    peace = 14,
    wooohhh = 15,
    Cong_UpYours = 16,
    Cong_WagWave2 = 17,
    Cong_WagWave1 = 18
}

public class WordConfig : ScriptableObject {
   /* private static WordConfig instance;
    public static WordConfig Instance {
        get {
            return Resources.Load<WordConfig>("Configs/WordConfig");
        }
    }

    [SerializeField]
    private TextAsset yesWordList;
    [SerializeField]
    private TextAsset notSureWordList;
    [SerializeField]
    private TextAsset noWordList;

    private string[] yesWords;
    private string[] notSureWords;
    private string[] noWords;

    private int previousDiceRoll1 = 99;
    private int previousDiceRoll2 = 99;

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
    private List<WordCollection> yesWordCollection;
    [SerializeField, HideInInspector]
    private List<WordCollection>  notSureWordCollection;
    [SerializeField, HideInInspector]
    private List<WordCollection> noWordCollection;

    public List<WordCollection> YesWordCollection { get { return yesWordCollection; } }
    public List<WordCollection> NotSureWordCollection { get { return notSureWordCollection; } }
    public List<WordCollection> NoWordCollection { get { return noWordCollection; } }

    public int WordCount(WordType wordType) {
        switch (wordType) {
            case WordType.yes:
                return yesWords != null ? yesWords.Length : 0;
            case WordType.notsure:
                return notSureWords != null ? notSureWords.Length : 0;
            case WordType.no:
                return noWords != null ? noWords.Length : 0;
        }
        return yesWords != null ? yesWords.Length : 0;
    }

    public string GetWord(WordType wordType, int wordIdx) {
        switch (wordType) {
            case WordType.yes:
                if (yesWords == null || wordIdx < 0 || wordIdx >= yesWords.Length) return string.Empty;
                return yesWords[wordIdx];
            case WordType.notsure:
                if (notSureWords == null || wordIdx < 0 || wordIdx >= notSureWords.Length) return string.Empty;
                return notSureWords[wordIdx];
            case WordType.no:
                if (noWords == null || wordIdx < 0 || wordIdx >= noWords.Length) return string.Empty;
                return noWords[wordIdx];
        }
        if (yesWords == null || wordIdx < 0 || wordIdx >= yesWords.Length) return string.Empty;
        return yesWords[wordIdx];
    }

    public WordCollection GetRandomWord() {
        List<WordCollection> fullWordCollection = new List<WordCollection>(YesWordCollection.Count +
                                    NotSureWordCollection.Count +
                                    NoWordCollection.Count);
        fullWordCollection.AddRange(YesWordCollection);
        fullWordCollection.AddRange(NotSureWordCollection);
        fullWordCollection.AddRange(NoWordCollection);

#if UNITY_EDITOR
        //fullWordCollection = RemoveNonTestWords(fullWordCollection);
        //if (fullWordCollection.Count <= 0) fullWordCollection.Add(YesWordCollection[0]);
#endif
        int dice = Random.Range(0, fullWordCollection.Count);
        do {
            dice = Random.Range(0, fullWordCollection.Count);
        } while (dice == previousDiceRoll1 ||  dice == previousDiceRoll2);
        previousDiceRoll2 = previousDiceRoll1;
        previousDiceRoll1 = dice;
        return fullWordCollection[dice]; //if null send index 0 data
    }

    private List<WordCollection> RemoveNonTestWords(List<WordCollection> fullWordCollection) {
        List<WordCollection> fullWordCollectionClean = new List<WordCollection>();
        for (int index = 0; index < fullWordCollection.Count;index ++) {
            if (fullWordCollection[index].isTestPlayable) fullWordCollectionClean.Add(fullWordCollection[index]);
        }
        return fullWordCollectionClean;
    }

    public List<WordCollection> GetWordCollection(WordType wordType) {
        switch (wordType) {
            case WordType.yes:
                return yesWordCollection;
            case WordType.notsure:
                return notSureWordCollection;
            case WordType.no:
                return noWordCollection;
        }

        return yesWordCollection;
    }

    public List<string> GenerateList(WordType wordType) {
        switch (wordType) {
            case WordType.yes:
                return yesWords.ToList();
            case WordType.notsure:
                return notSureWords.ToList();
            case WordType.no:
                return noWords.ToList();
        }
        return yesWords.ToList();
    }

    public string GetCongWord(WordType wordType, string id) {
        List<WordCollection> wordCollection = new List<WordCollection>();
        switch (wordType) {
            case WordType.yes:
                wordCollection = yesWordCollection;
                break;
            case WordType.notsure:
                wordCollection = notSureWordCollection;
                break;
            case WordType.no:
                wordCollection = noWordCollection;
                break;
        }

        for (int index = 0; index < wordCollection.Count; index++) {
            if (wordCollection[index].id == id) {
                return wordCollection[index].word;
            }
        }
        return null;
    }

    public LipSyncData GetCongLipSynData(WordType wordType, string id) {
        List<WordCollection> wordCollection = new List<WordCollection>();
        switch (wordType) {
            case WordType.yes:
                wordCollection = YesWordCollection;
                break;
            case WordType.notsure:
                wordCollection = NotSureWordCollection;
                break;
            case WordType.no:
                wordCollection = NoWordCollection;
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
        yesWords = yesWordList != null ? yesWordList.text.Split('\n') : new string[0];
        notSureWords = notSureWordList != null ? notSureWordList.text.Split('\n') : new string[0];
        noWords = noWordList != null ? noWordList.text.Split('\n') : new string[0];



        if (yesWordCollection.Count <= 0) yesWordCollection = new List<WordCollection>();
        for (int index = 0; index < yesWords.Length; index++) {
            if (!HasWord(yesWords[index], yesWordCollection)) {
                //ADD
                WordCollection wordCollection = new WordCollection();
                wordCollection.id = "";
                wordCollection.lipSyncData = new LipSyncData();
                wordCollection.word = GetWord(WordType.yes, index);
                yesWordCollection.Add(wordCollection);
            }
        }

        if (notSureWordCollection.Count <= 0) notSureWordCollection = new List<WordCollection>();
        for (int index = 0; index < notSureWords.Length; index++) {
            if (!HasWord(notSureWords[index], notSureWordCollection)) {
                //ADD
                WordCollection wordCollection = new WordCollection();
                wordCollection.id = "";
                wordCollection.lipSyncData = new LipSyncData();
                wordCollection.word = GetWord(WordType.notsure, index);
                notSureWordCollection.Add(wordCollection);
            }
        }

        if (noWordCollection.Count <= 0) noWordCollection = new List<WordCollection>();
        for (int index = 0; index < noWords.Length; index++) {
            if (!HasWord(noWords[index], noWordCollection)) {
                //ADD
                WordCollection wordCollection = new WordCollection();
                wordCollection.id = "";
                wordCollection.lipSyncData = new LipSyncData();
                wordCollection.word = GetWord(WordType.no, index);
                noWordCollection.Add(wordCollection);
            }
        }

        for (int index = 0; index < yesWordCollection.Count; index++) {
            if (yesWordCollection[index].word == null || yesWordCollection[index].word == "") {
                yesWordCollection.RemoveAt(index);
            }
        }
        for (int index2 = 0; index2 < notSureWordCollection.Count; index2++) {
            if (notSureWordCollection[index2].word == null || notSureWordCollection[index2].word == "") {
                notSureWordCollection.RemoveAt(index2);
            }
        }
        for (int index3 = 0; index3 < noWordCollection.Count; index3++) {
            if (noWordCollection[index3].word == null || noWordCollection[index3].word == "") {
                noWordCollection.RemoveAt(index3);
            }
        }

        //Remove duplicate
        RemoveDuplicate(yesWordCollection);
        RemoveDuplicate(notSureWordCollection);
        RemoveDuplicate(noWordCollection);

        //Remove non existential items
        RemoveNonExistential(yesWordCollection, yesWords);
        RemoveNonExistential(notSureWordCollection, notSureWords);
        RemoveNonExistential(noWordCollection, noWords);
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
        for (int count = wordCollection.Count-1; count >= 0; count--) {
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
