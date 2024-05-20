using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using MySql.Data.MySqlClient;

public class GameRecord {
    public string id;
    public string team;
    public string racingTime;
    public string score;
    public string repeats;
    public string myRank;
    public string gamesCount;
}

public class DB_Requester : MonoBehaviour
{
    //"{}" 형식은 안에 실제 변수 입력 
    //ex. "{id}" -> "root"
    private MySqlConnection connection;
    private string server = "127.0.0.1"; // MySQL 서버 주소
    private string database = "fast_and_furious"; // 데이터베이스 이름
    private string uid = "root"; // MySQL 사용자 이름
    private string password = "wsnt4375!"; // MySQL 비밀번호

    private void Start() {
        string connectionString = $"Server={server};Database={database};User ID={uid};Password={password};";
        connection = new MySqlConnection(connectionString);

        try {
            connection.Open();
            Debug.Log("MySQL 연결 성공");

            string query = "SELECT * FROM games;";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read()) {
                if (dataReader["team"] != DBNull.Value) {
                    string id = dataReader["team"].ToString();
                    //Debug.Log("데이터: " + id);
                } 
                else {
                    Debug.LogWarning("데이터가 없음");
                }
            }

            dataReader.Close();
        }
        catch (Exception ex) {
            Debug.LogWarning("MySQL 연결 오류: " + ex.Message);
        }
    }

    private string GetPropertyString(MySqlDataReader dataReader, string columnName) {
        for (int i = 0; i < dataReader.FieldCount; i++) {
            if (dataReader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                return dataReader[columnName].ToString();
        }
        return string.Empty;
    }

    private List<GameRecord> RequestQuery(string query) {
        try {
            List<GameRecord> gameRecords = new List<GameRecord>();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read()) {
                MySqlDataReader data = dataReader; // 열 이름으로 데이터 가져오기
                GameRecord newRecord = new GameRecord {
                    id = GetPropertyString(data, "id"),
                    team = GetPropertyString(data, "team"),
                    racingTime = GetPropertyString(data, "racing_time"),
                    score = GetPropertyString(data, "score"),
                    repeats = GetPropertyString(data, "repeats"),
                    myRank = GetPropertyString(data, "my_rank"),
                    gamesCount = GetPropertyString(data, "games_count")
                };
                gameRecords.Add(newRecord);
            }
            dataReader.Close();
            return gameRecords;
        }
        catch(Exception e) {
            Debug.LogWarning("MySQL 연결 오류: " + e.Message);
            return null;
        }
    }

    public void DebugAll() {
        List<GameRecord> gameRecords = RequestQuery("SELECT * FROM games;");
        for(int i = 0; i < gameRecords.Count; ++i) {
            Debug.Log(gameRecords[i].id);
        }
    }

    private string GetSavingQuery(Team team) {
        int goalLaps = GameManager.instance.goalLaps;
        return string.Format("INSERT INTO games(id, team, racing_time, score, repeats, laps) " +
                            "SELECT FLOOR(COUNT(*) / 2 + 1), '{0}', {1}, {2}, {3}, {4} FROM games;",
                            team.ReturnName(), team.ReturnLapTime(goalLaps - 1).TotalSeconds,
                            team.ReturnScore(), team.ReturnRepeats(), goalLaps);
    }

    public void SaveTeamsStats() {
        Team leftTeam = GameManager.instance.leftTeam;
        Team rightTeam = GameManager.instance.rightTeam;
        RequestQuery(GetSavingQuery(leftTeam));
        RequestQuery(GetSavingQuery(rightTeam));
    }

    public GameRecord GetAverageRecord() {
        string query = "SELECT MAX(id) AS id, AVG(racing_time) AS racing_time, " +
                        "AVG(score) AS score, AVG(repeats) AS repeats FROM games;";
        List<GameRecord> gameRecords = RequestQuery(query);
        if(gameRecords == null)
            return null;
        return gameRecords[0];
    }

    public GameRecord GetTeamRecord(int id, Team team) {
        string query = string.Format("SELECT t.*, t.my_rank, t.games_count FROM (SELECT games.*, DENSE_RANK() OVER " +
                                    "(ORDER BY racing_time ASC) AS my_rank, COUNT(*) OVER () AS games_count FROM games) " +
                                    "t WHERE t.id = {0} AND t.team = '{1}';", id, team.ReturnName());
        List<GameRecord> gameRecords = RequestQuery(query);
        if(gameRecords == null)
            return null;
        return gameRecords[0];
    }

    public List<GameRecord> GetTopRecords() {
        string query = "SELECT * FROM games ORDER BY racing_time ASC LIMIT 10;";
        List<GameRecord> gameRecords = RequestQuery(query);
        if(gameRecords == null)
            return null;
        return gameRecords;
    }

    private void OnApplicationQuit()
    {
        if (connection != null && connection.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }
}
