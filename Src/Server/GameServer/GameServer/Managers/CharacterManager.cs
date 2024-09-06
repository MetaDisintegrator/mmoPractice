using Common;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class CharacterManager : Singleton<CharacterManager>
    {
        private Dictionary<int, Character> characters = new Dictionary<int, Character>();

        public void Clear()
        {
            characters.Clear();
        }

        public Character AddCharacter(TCharacter dbCharacter)
        {
            Character character = new Character(SkillBridge.Message.CharacterType.Player, dbCharacter);
            EntityManager.Instance.AddEntity(character.Info.mapId, character);
            characters[character.entityId] = character;
            return character;
        }

        public void RemoveCharacter(int id) 
        {
            EntityManager.Instance.RemoveEntity(characters[id].Info.mapId, characters[id]);
            characters.Remove(id);
        }
    }
}
