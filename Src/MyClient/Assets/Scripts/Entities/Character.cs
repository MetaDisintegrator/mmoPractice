using Common.Data;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Character : Entity
    {
        public NCharacterInfo info;
        public CharacterDefine define;

        public Character(NCharacterInfo info) : base(info.Entity)
        {
            this.info = info;
            this.define = DataManager.Instance.Characters[info.Tid];
        }

        public string Name => info.Type == CharacterType.Player ? info.Name : define.Name;

        public bool IsPlayer => info.Id == User.Instance.CurrentCharacter.Id;

        public void MoveForward()
        {
            Debug.LogFormat("{0} move forward", Name);
            this.speed = this.define.Speed;
        }

        public void MoveBack()
        {
            Debug.LogFormat("{0} move back", Name);
            this.speed = -this.define.Speed;
        }

        public void Stop()
        {
            Debug.LogFormat("{0} stop", Name);
            this.speed = 0;
            this.EntityNetData.Speed = 0;
        }

        public void SetDirection(Vector3Int dir) => this.direction = dir;

        public void SetPosition(Vector3Int pos) => this.position = pos;
    }
}

