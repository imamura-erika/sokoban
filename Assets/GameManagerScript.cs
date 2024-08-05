using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Timeline;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject boxPrefab;
    // 配列の宣言
    int[,] map; // レベルデザイン用の配列
    GameObject[,] field; // ゲーム管理用の配列

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                // nullだったらタグを調べず次の要素へ移る
                if (field[y, x] == null) { continue; }
                // nullだったらcontinueしているので、
                // この行に辿りつく場合はnullでないことが確定。
                // タグの確認を行う。
                if (field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
            }
        }
            return new Vector2Int(-1, -1);
    }

    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {
        // 移動先が範囲外なら移動不可
        // 二次元配列に対応
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }

        // 移動先に2(箱)が居たら
        if (field[moveTo.y,moveTo.x] != null && field[moveTo.y,moveTo.x].tag=="Box")
        {
            // どの方向へ移動するかを算出
            Vector2Int velocity = moveTo - moveFrom;
            // プレイヤーの移動先から、更に先へ2(箱)を移動させる
            // 箱の移動処理。MoverNumberメソッド内でMoveNumberメソッドを
            // 呼び、処理が再帰している。移動可不可をboolで記憶
            bool success = MoveNumber(moveTo, moveTo + velocity);
            // もし箱が移動失敗したら、プレイヤーの移動も失敗
            if (!success) { return false; }
        }

        // プレイヤー・箱関わらずの移動処理
        field[moveFrom.y, moveFrom.x].transform.position =
            new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    void Start()
    {
        // 初期化
        map = new int[,]{
        { 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 2, 0, 0, 0 },
        { 0, 0, 0, 2, 1, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0 },
        };

        field = new GameObject[
            map.GetLength(0),
            map.GetLength(1)
            ];

        // 二重for文で二次元配列の情報を出力
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y,x] = Instantiate(
                        playerPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity);
                };
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                        );
                }
            }
        }
    }


    void Update()
    {
        // 右移動
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber
                (playerIndex,
                 playerIndex + new Vector2Int(1, 0));
        }
        // 左移動
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber
                (playerIndex, 
                 playerIndex + new Vector2Int(-1, 0));
        }
        // 上移動
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber
                (playerIndex,
                 playerIndex + new Vector2Int(0, -1));
        }
        // 下移動
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber
                (playerIndex,
                 playerIndex + new Vector2Int(0, 1));
        }
    }
}