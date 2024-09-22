using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Utils;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class UserService : Singleton<UserService>
    {
        ValidStr validStr;
        public void Init()
        {
            validStr = new ValidStr();
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(OnUserRegisterRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(OnUserLoginRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(OnUserCreateCharacterRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(OnUserGameEnterRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(OnUserGameLeaveRequest);
        }

        private void OnUserRegisterRequest(NetConnection<NetSession> sender, UserRegisterRequest message)
        {
            Log.InfoFormat("UserRegisterRequest[UserName:{0},Password:{1}]", message.User, message.Passward);

            NetMessage msg = new NetMessage()
            {
                Response = new NetMessageResponse()
                {
                    userRegister = new UserRegisterResponse()
                }
            };

            if (!(validStr.Valid(message.User) && validStr.Valid(message.Passward)))
            {
                msg.Response.userRegister.Result = Result.Failed;
                msg.Response.userRegister.Errormsg = "账号或密码含有非法字符";
            }
            else if (DBService.Instance.Entities.Users.Where(u => u.Username == message.User).FirstOrDefault() != null)
            {
                msg.Response.userRegister.Result = Result.Failed;
                msg.Response.userRegister.Errormsg = "账号已存在";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = message.User, Password = message.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();

                msg.Response.userRegister.Result = Result.Success;
                msg.Response.userRegister.Errormsg = "None";
            }

            byte[] data = PackageHandler.PackMessage(msg);
            sender.SendData(data, 0, data.Length);
        }

        private void OnUserLoginRequest(NetConnection<NetSession> sender, UserLoginRequest message)
        {
            Log.InfoFormat("UserLoginRequest[UserName:{0},Password:{1}]", message.User, message.Passward);

            NetMessage msg = new NetMessage()
            {
                Response = new NetMessageResponse()
                {
                    userLogin = new UserLoginResponse()
                }
            };

            if (!(validStr.Valid(message.User) && validStr.Valid(message.Passward)))
            {
                msg.Response.userLogin.Result = Result.Failed;
                msg.Response.userLogin.Errormsg = "账号或密码含有非法字符";
            }
            else
            {
                TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == message.User).FirstOrDefault();
                if (user == null)
                {
                    msg.Response.userLogin.Result = Result.Failed;
                    msg.Response.userLogin.Errormsg = "账号不存在";
                }
                else if (user.Password != message.Passward)
                {
                    msg.Response.userLogin.Result = Result.Failed;
                    msg.Response.userLogin.Errormsg = "账号或密码错误";
                }
                else
                {
                    msg.Response.userLogin.Result = Result.Success;
                    msg.Response.userLogin.Errormsg = "None";

                    sender.Session.User = user;
                    msg.Response.userLogin.Userinfo = new NUserInfo()
                    {
                        Id = 1,
                        Player = new NPlayerInfo()
                        {
                            Id = user.Player.ID,
                        }
                    };
                    foreach (var character in user.Player.Characters)
                    {
                        NCharacterInfo info = new NCharacterInfo()
                        {
                            Id = character.ID,
                            Name = character.Name,
                            Class = (CharacterClass)character.Class,
                            Tid = character.TID,
                            mapId = character.MapID,
                            Type = CharacterType.Player
                        };
                        msg.Response.userLogin.Userinfo.Player.Characters.Add(info);
                    }
                }
            };

            byte[] data = PackageHandler.PackMessage(msg);
            sender.SendData(data, 0, data.Length);
        }

        private void OnUserCreateCharacterRequest(NetConnection<NetSession> sender, UserCreateCharacterRequest message)
        {
            Log.InfoFormat("UserLoginRequest[Name:{0},Class:{1}]", message.Name, message.Class);

            NetMessage msg = new NetMessage()
            {
                Response = new NetMessageResponse()
                {
                    createChar = new UserCreateCharacterResponse()
                }
            };

            if (!validStr.Valid(message.Name))
            {
                msg.Response.createChar.Result = Result.Failed;
                msg.Response.createChar.Errormsg = "昵称含有非法字符";
            }
            else
            {
                TCharacter tch = DBService.Instance.Entities.Characters.Where(c => c.Name == message.Name).FirstOrDefault();
                if (tch != null)
                {
                    msg.Response.createChar.Result = Result.Failed;
                    msg.Response.createChar.Errormsg = "昵称已存在";
                }
                else
                {
                    TCharacter character = new TCharacter()
                    {
                        Name = message.Name,
                        Class = (int)message.Class,
                        TID = (int)message.Class,
                        MapID = 1,
                        MapPosX = 5000,
                        MapPosY = 4000,
                        MapPosZ = 802
                    };
                    DBService.Instance.Entities.Characters.Add(character);
                    sender.Session.User.Player.Characters.Add(character);
                    DBService.Instance.Entities.SaveChanges();

                    msg.Response.createChar.Result = Result.Success;
                    msg.Response.createChar.Errormsg = "None";
                    NCharacterInfo info = new NCharacterInfo()
                    {
                        Id = 1,
                        Name = message.Name,
                        Class = message.Class,
                        Tid = (int)message.Class,
                        Level = 1,
                        mapId = 1,
                        Type = CharacterType.Player,
                    };
                    msg.Response.createChar.Characters.Add(info);
                }
            };

            byte[] data = PackageHandler.PackMessage(msg);
            sender.SendData(data, 0, data.Length);
        }

        private void OnUserGameEnterRequest(NetConnection<NetSession> sender, UserGameEnterRequest message)
        {
            Log.InfoFormat("UserGameEnterRequest[characterIdx:{0}]", message.characterIdx);
            TCharacter dbCharacter = sender.Session.User.Player.Characters.ElementAt(message.characterIdx);
            Character character = CharacterManager.Instance.AddCharacter(dbCharacter);

            sender.Session.Character = character;

            NetMessage msg = new NetMessage()
            {
                Response = new NetMessageResponse()
                {
                    gameEnter = new UserGameEnterResponse()
                    {
                        Result = Result.Success,
                        Errormsg = "None",
                        userCharacter = character.Info
                    }
                }
            };

            //道具测试
            ItemTest(character);

            byte[] data = PackageHandler.PackMessage(msg);
            sender.SendData(data, 0, data.Length);
            Log.InfoFormat("Assigned Id:{0}", character.Info.Entity.Id);
            MapManager.Instance[dbCharacter.MapID].CharacterEnter(sender, character);
        }

        private void OnUserGameLeaveRequest(NetConnection<NetSession> sender, UserGameLeaveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("UserGameEnterRequest[entityId:{0}]", character.entityId);

            NetMessage msg = new NetMessage()
            {
                Response = new NetMessageResponse()
                {
                    gameLeave = new UserGameLeaveResponse()
                    {
                        Result = Result.Success,
                        Errormsg = "None"
                    }
                }
            };
            RemoveCharacter(character);
            sender.Session.Character = null;

            byte[] data = PackageHandler.PackMessage(msg);
            sender.SendData(data, 0, data.Length);
        }

        public void RemoveCharacter(Character character)
        {
            CharacterManager.Instance.RemoveCharacter(character.entityId);
            MapManager.Instance[character.Info.mapId].CharacterLeave(character);
        }

        private static void ItemTest(Character character)
        {
            int ItemID = 1;
            bool hasItem = character.ItemManager.HasItem(ItemID);
            Log.InfoFormat("Item Test: Character {0} HasItem {1}: {2}", character.entityId, ItemID, hasItem);
            if (hasItem)
            {
                character.ItemManager.RemoveItem(ItemID, 1);
            }
            else
            {
                character.ItemManager.AddItem(ItemID, 2);
            }
            Models.Item item = character.ItemManager.GetItem(ItemID);
            Log.InfoFormat("Item Test: Character {0} ItemInfo: [ID:{1},Count:{2}]", character.entityId, ItemID, item.Count);
        }
    }
}
