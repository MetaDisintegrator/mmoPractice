using System;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;
using Models;
using Managers;

namespace Services
{
    class UserService : Singleton<UserService>, IDisposable
    {
        public UnityEngine.Events.UnityAction<Result, string> OnRegister;
        public UnityEngine.Events.UnityAction<Result,string> OnLogin;
        public UnityEngine.Events.UnityAction<Result, string> OnCreate;
        NetMessage pendingMessage = null;
        bool connected = false;

        public UserService()
        {
            NetClient.Instance.OnConnect += OnGameServerConnect;
            NetClient.Instance.OnDisconnect += OnGameServerDisconnect;
            MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Subscribe<UserLoginResponse>(this.OnUserLogin);
            MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(this.OnUserGameEnter);
            MessageDistributer.Instance.Subscribe<UserGameLeaveResponse>(this.OnUserGameLeave);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(this.OnUserLogin);
            MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            MessageDistributer.Instance.Unsubscribe<UserGameEnterResponse>(this.OnUserGameEnter);
            MessageDistributer.Instance.Unsubscribe<UserGameLeaveResponse>(this.OnUserGameLeave);
            NetClient.Instance.OnConnect -= OnGameServerConnect;
            NetClient.Instance.OnDisconnect -= OnGameServerDisconnect;
        }

        public void Init()
        {

        }

        public void ConnectToServer()
        {
            Debug.Log("ConnectToServer() Start ");
            //NetClient.Instance.CryptKey = this.SessionId;
            NetClient.Instance.Init("127.0.0.1", 8888);
            NetClient.Instance.Connect();
        }


        void OnGameServerConnect(int result, string reason)
        {
            Log.InfoFormat("LoadingMesager::OnGameServerConnect :{0} reason:{1}", result, reason);
            if (NetClient.Instance.Connected)
            {
                this.connected = true;
                if(this.pendingMessage!=null)
                {
                    NetClient.Instance.SendMessage(this.pendingMessage);
                    this.pendingMessage = null;
                }
            }
            else
            {
                if (!this.DisconnectNotify(result, reason))
                {
                    MessageBox.Show(string.Format("网络错误，无法连接到服务器！\n RESULT:{0} ERROR:{1}", result, reason), "错误", MessageBoxType.Error);
                }
            }
        }

        public void OnGameServerDisconnect(int result, string reason)
        {
            this.DisconnectNotify(result, reason);
            return;
        }

        bool DisconnectNotify(int result,string reason)
        {
            if (this.pendingMessage != null)
            {
                if (this.pendingMessage.Request.userRegister!=null)
                {
                    if (this.OnRegister != null || this.OnLogin!=null)
                    {
                        this.OnRegister(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                return true;
            }
            return false;
        }

        public void SendRegister(string user, string psw)
        {
            Debug.LogFormat("UserRegisterRequest[user :{0} psw:{1}]", user, psw);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userRegister = new UserRegisterRequest();
            message.Request.userRegister.User = user;
            message.Request.userRegister.Passward = psw;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }

        void OnUserRegister(object sender, UserRegisterResponse response)
        {
            Debug.LogFormat("OnUserRegister:{0} [{1}]", response.Result, response.Errormsg);

            if (this.OnRegister != null)
            {
                this.OnRegister(response.Result, response.Errormsg);
            }
        }

        public void SendLogin(string user, string psw)
        {
            Debug.LogFormat("UserLoginRequest[user:{0} psw:{1}]",user,psw);

            NetMessage msg = new NetMessage()
            {
                Request = new NetMessageRequest()
                {
                    userLogin = new UserLoginRequest()
                    { 
                        User = user,
                        Passward = psw
                    }
                }
            };

            if (this.connected && NetClient.Instance.Connected)
            { 
                this.pendingMessage = null ;
                NetClient.Instance.SendMessage(msg);
            }
            else
            {
                this.pendingMessage = msg;
                this.ConnectToServer();
            }
        }

        private void OnUserLogin(object sender, UserLoginResponse message)
        {
            Debug.LogFormat("OnUserLogin:{0} [{1}]",message.Result,message.Errormsg);

            User.Instance.SetupUserInfo(message.Userinfo);
            if (this.OnLogin != null)
            { 
                this.OnLogin(message.Result, message.Errormsg);
            }
        }

        public void SendCreateCharacter(string name, CharacterClass characterClass)
        {
            Debug.LogFormat("UserCreateCharacterRequest[name:{0} characterClass:{1}]", name, characterClass);

            NetMessage msg = new NetMessage()
            {
                Request = new NetMessageRequest()
                {
                    createChar = new UserCreateCharacterRequest()
                    {
                        Name = name,
                        Class = characterClass
                    }
                }
            };

            NetClient.Instance.SendMessage(msg);
        }

        private void OnUserCreateCharacter(object sender, UserCreateCharacterResponse message)
        {
            Debug.LogFormat("OnUserCreateCharacter:{0} [{1}]", message.Result, message.Errormsg);

            User.Instance.Info.Player.Characters.AddRange(message.Characters);
            if (this.OnCreate != null)
            {
                this.OnCreate(message.Result, message.Errormsg);
            }
        }

        public void SendGameEnter(int characterIdx)
        {
            Debug.LogFormat("UserGameEnterRequest[characterIdx:{0}]", characterIdx);

            NetMessage msg = new NetMessage()
            {
                Request = new NetMessageRequest()
                {
                    gameEnter = new UserGameEnterRequest()
                    {
                        characterIdx = characterIdx
                    }
                }
            };

            NetClient.Instance.SendMessage(msg);
        }

        private void OnUserGameEnter(object sender, UserGameEnterResponse message)
        {
            Debug.LogFormat("OnUserGameEnter:{0} [{1}]", message.Result, message.Errormsg);
            if (message.Result == Result.Success)
            {
                User.Instance.CurrentCharacter = message.userCharacter;
                ItemManager.Instance.Init(message.userCharacter.Items);
                BagManager.Instance.Init(message.userCharacter.Bag);
            }
        }

        public void SendGameLeave()
        {
            NetMessage msg = new NetMessage()
            {
                Request = new NetMessageRequest
                {
                    gameLeave = new UserGameLeaveRequest()
                }
            };

            byte[] data = PackageHandler.PackMessage(msg);
            NetClient.Instance.SendMessage(msg);
        }

        private void OnUserGameLeave(object sender, UserGameLeaveResponse message)
        {
            Debug.LogFormat("OnUserGameLeave:{0} [{1}]", message.Result, message.Errormsg);
        }
    }
}
