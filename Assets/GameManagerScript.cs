using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int[] map;

    // ���\�b�h��
void PrintArray()
    {
        string debugText = "";
        for(int i = 0; i< map.Length; i++) // 1�����ǂ̃C���f�b�N�X�Ȃ̂��𒲂ׂ�
        {
            debugText += map[i].ToString() + ","; // �z��̓��e���o�͂��鏈��
        }
        Debug.Log(debugText); // �v�f��������o��
    }

    // 1�̒l���i�[����Ă���C���f�b�N�X���擾���鏈���̃��\�b�h��
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

    // �ړ��̉s�𔻒f���Ĉړ������鏈���̃��\�b�h��
    bool MoveNumber(int number, int moveFrom, int moveTo)
    {
        // �ړ���ɂ���Ĕ��f�i�v���C���[�̈ʒu�ɂ���Ĕ��f����Ɨʂ���ςȂ��ƂɂȂ�j
        // �ړ��悪�͈͊O�Ȃ�ړ��s��
        if (moveTo < 0 || moveTo >= map.Length) { return false; }
        // �ړ����2(��)��������
        if (map[moveTo] == 2)
        {
            // �ǂ̕����ֈړ����邩���Z�o
            int offset = moveTo - moveFrom; // ���̍s������߂邽�߂̍���
            // �v���C���[�̈ړ��悩��A����ɐ��2(��)���ړ�������B
            // ���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h��
            // �ĂсA�������ċA���Ă���B�ړ��s��bool�ŋL�^
            bool success = MoveNumber(2, moveTo, moveTo + offset);
            // ���������ړ����s������A�v���C���[�̈ړ������s
            if (!success) { return false; }
        } 
        // �s��ɔ������鎞
        // �v���C���[�E���ւ�炸�̈ړ������i�S�������j
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
            // ���\�b�h�������������g�p
            int playerIndex = GetPlayerIndex();

            //�ړ��������֐���
            MoveNumber(1, playerIndex, playerIndex + 1);
            PrintArray();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // ���\�b�h�������������g�p
            int playerIndex = GetPlayerIndex();

            //�ړ��������֐���
            MoveNumber(1, playerIndex, playerIndex - 1);
            PrintArray();
        }
    }
}