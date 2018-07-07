using System;
using System.Collections.Generic;
using UnityEngine;

public class MallStoreItemConfig  : ScriptableObject {

    [System.Serializable]
    public class Item {
        public string itemName;
        public float itemPrice;
        public Sprite itemImage;
        public int itemStock;

        public bool HasStock {
            get {
                return (itemStock > 0) ? true : false;
            }
        }
    }

    public List<Item> itemsSelling;

}
