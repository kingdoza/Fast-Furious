using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRanking : MonoBehaviour
{
    [SerializeField]
    private RankingBox[] rankingBoxes;

    private void OnEnable() {
        List<GameRecord> gameRecords = GameManager.instance.sql_Requester.GetTopRecords();
        int index = gameRecords.Count > rankingBoxes.Length ? rankingBoxes.Length : gameRecords.Count;
        for(int i = 0; i < index; ++i) {
            rankingBoxes[i].SetTopRecord(gameRecords[i]);
        }
    }
}
