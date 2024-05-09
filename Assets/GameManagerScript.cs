using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int[] map;

    // メソッド化
void PrintArray()
    {
        string debugText = "";
        for(int i = 0; i< map.Length; i++) // 1が今どのインデックスなのかを調べる
        {
            debugText += map[i].ToString() + ","; // 配列の内容を出力する処理
        }
        Debug.Log(debugText); // 要素数を一つずつ出力
    }

    // 1の値が格納されているインデックスを取得する処理のメソッド化
    int GetPlayerIndex()
    {
        for(int i=0;i<map.Length; i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }
        return -1;
    }

    // 移動の可不可を判断して移動させる処理のメソッド化
    bool MoveNumber(int number, int moveFrom, int moveTo)
    {
        // 移動先によって判断（プレイヤーの位置によって判断すると量が大変なことになる）
        // 移動先が範囲外なら移動不可
        if (moveTo < 0 || moveTo >= map.Length) { return false; }
        // 移動先に2(箱)が居たら
        if (map[moveTo] == 2)
        {
            // どの方向へ移動するかを算出
            int offset = moveTo - moveFrom; // 箱の行先を決めるための差分
            // プレイヤーの移動先から、さらに先へ2(箱)を移動させる。
            // 箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを
            // 呼び、処理が再帰している。移動可不可をboolで記録
            bool success = MoveNumber(2, moveTo, moveTo + offset);
            // もし箱が移動失敗したら、プレイヤーの移動も失敗
            if (!success) { return false; }
        } 
        // 行先に箱がある時
        // プレイヤー・箱関わらずの移動処理（全部動く）
        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }

    void Start()
    {
        map = new int[] { 0, 0, 0, 1, 0, 2, 0, 2, 0 };
        PrintArray();
    }

    void Update()
    {
        bool debugOut = false;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // メソッド化した処理を使用
            int playerIndex = GetPlayerIndex();

            //移動処理を関数化
            MoveNumber(1, playerIndex, playerIndex + 1);
            PrintArray();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // メソッド化した処理を使用
            int playerIndex = GetPlayerIndex();

            //移動処理を関数化
            MoveNumber(1, playerIndex, playerIndex - 1);
            PrintArray();
        }
    }
}