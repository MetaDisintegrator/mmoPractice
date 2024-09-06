using Entities;
using Managers;
using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoSingleton<GameObjectManager>
{
    private Dictionary<int, GameObject> characterObjs = new Dictionary<int, GameObject>();

    private bool inited = false;

    protected override void OnStart()
    {
        StartCoroutine(InitGameObjects());
        if (!inited)
        {
            CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
            CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
            inited = true;
        }
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
    }

    void OnCharacterEnter(Character character) => CreateCharacterObject(character);

    private void OnCharacterLeave(Character character)
    {
        if (characterObjs.TryGetValue(character.entityId, out GameObject go))
        {
            if (go != null)
            {
                Destroy(go);
            }
        }
        characterObjs[character.entityId] = null;
    }

    IEnumerator InitGameObjects()
    {
        foreach (var character in CharacterManager.Instance.characters.Values)
        {
            CreateCharacterObject(character);
            yield return null;
        }
    }

    private void CreateCharacterObject(Character character)
    {
        if (!characterObjs.ContainsKey(character.entityId) || characterObjs[character.entityId] == null)
        {
            Object obj = Resloader.Load<Object>(character.define.Resource);
            if (obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.", character.define.TID, character.define.Resource);
                return;
            }
            GameObject go = Instantiate(obj, this.transform) as GameObject;
            go.name = "Character_" + character.info.Id + "_" + character.info.Name;
            go.SetActive(true);
            characterObjs[character.entityId] = go;
            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
        }
        InitGameObject(characterObjs[character.entityId], character);
    }

    private void InitGameObject(GameObject go, Character character)
    {
        go.transform.position = GameObjectTool.LogicToWorld(character.position);
        go.transform.forward = GameObjectTool.LogicToWorld(character.direction);

        EntityController ec = go.GetComponent<EntityController>();
        if (ec != null)
        {
            ec.entity = character;
            ec.isPlayer = character.IsPlayer;
            EntityManager.Instance.RegisterEntityNotifier(character.entityId, ec);
        }

        PlayerInputController pc = go.GetComponent<PlayerInputController>();
        if (pc != null)
        {
            if (character.entityId == User.Instance.CurrentCharacter.Entity.Id)
            {
                User.Instance.CurrentAvatarObj = go;
                MainPlayerCamera.Instance.character = go;
                Debug.Log("Set Camera");
                pc.enabled = true;
                pc.character = character;
                pc.entityController = ec;
            }
            else
                pc.enabled = false;
        }
    }
}
