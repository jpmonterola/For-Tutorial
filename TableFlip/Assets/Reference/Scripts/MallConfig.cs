using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MallConfig", menuName = "Congkalog/Mall Config")]
public class MallConfig : ScriptableObject {
    private static MallConfig instance;
    public static MallConfig Instance {
        get {
            if (instance == null) {
                instance = Resources.Load<MallConfig>("Configs/MallConfig");
            }
            return instance;
        }
    }

    public List<MallStore> mallStores;
}
