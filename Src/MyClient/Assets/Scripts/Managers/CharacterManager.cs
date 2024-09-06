using Common.Data;
using Entities;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class CharacterManager : Singleton<CharacterManager>, IDisposable
    {
        public Dictionary<int, Character> characters = new Dictionary<int, Character>();

        public event UnityAction<Character> OnCharacterEnter;
        public event UnityAction<Character> OnCharacterLeave;

        public void Init()
        { 
        }

        public void Dispose()
        {
        }

        public Character AddCharacter(NCharacterInfo info)
        {
            Debug.LogFormat("AddCharacter:{0} {1}, Map:{2}, Entity: {3}", info.Id, info.Name, info.mapId, info.Entity.ToString());
            Character character = new Character(info);
            characters[info.Id] = character;
            EntityManager.Instance.AddEntity(character);

            OnCharacterEnter?.Invoke(character);
            return character;
        }

        public void RemoveCharacter(int characterId)
        {
            Debug.LogFormat("RemoveCharacter:{0}", characterId);
            if (!characters.ContainsKey(characterId))
                return;
            OnCharacterLeave?.Invoke(characters[characterId]);
            characters.Remove(characterId);
            EntityManager.Instance.RemoveEntity(characterId);
        }

        public void Clear()
        {
            int[] ids = characters.Keys.ToArray();
            foreach (int id in ids)
            {
                RemoveCharacter(id);
            }
        }
    }
}

