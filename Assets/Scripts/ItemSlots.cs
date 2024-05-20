using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlots : MonoBehaviour
{
    private const int maxItems = 4;
    [SerializeField]
    private GameObject[] slots = new GameObject[maxItems];
    private GameObject[] itemPrefabs = new GameObject[maxItems];

    private void DestroyItemPrefab(int index) {
        Destroy(itemPrefabs[index]);
        itemPrefabs[index] = null;
    }

    private void CreateItemPrefab(int index, GameObject prefab) {
        itemPrefabs[index] = Instantiate(prefab, slots[index].transform);
    }

    public void SetItemSlots(List<int> itemList) {
        for(int i = 0; i < maxItems; ++i) {
            Destroy(itemPrefabs[i]);
            itemPrefabs[i] = null;
        }
        for(int i = 0; i < itemList.Count; ++i) {
            GameObject prefab = UI_Manager.instance.gameStatus.GetItemObj(itemList[i]);
            itemPrefabs[i] = Instantiate(prefab, slots[i].transform);
        }
    }
}
