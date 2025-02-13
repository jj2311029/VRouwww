using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string name; // 대화하는 캐릭터 이름
    public string[] contexts; // 대사 목록
    public Sprite characterSprite; // 대화할 때 표시할 캐릭터 이미지
}
