using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Timers;
using FluorineFx;
using FluorineFx.Messaging.Api.Service;
using FluorineFx.Messaging.Messages;
using FluorineFx.Net;

namespace Set_Retail_5x5.Set10.FlexMod
{
    public class Set10CentrumAuth
    {
        private Timer _sessionActiveTimer;

        private Cookie _jSession;

        private Cookie _setRetailX;

        private NetConnection _amf3Client;

        private string _ip, _port, _msgBrokerPath, _url;


        private bool _isSessionActiveTimerEnabled = false;

        public Set10CentrumAuth(string ip,string port, string msgBrokerPath)
        {
            _ip = ip;
            _port = port;
            _msgBrokerPath = msgBrokerPath;
            _url = string.Format($"http://{ip}:{port}{msgBrokerPath}");
            Amf3ConnectionInitialize();
            SessionActiveTimerInitialize();
        }

        private void Amf3ConnectionInitialize()
        {
            _amf3Client = new NetConnection {ObjectEncoding = ObjectEncoding.AMF3};

            JSession = new Cookie("JSESSION",string.Empty,string.Empty, _ip);
        }

        private void SessionActiveTimerInitialize()
        {
            _sessionActiveTimer = new Timer
            {
                Interval = 5000,
                AutoReset = false
            };

            _sessionActiveTimer.Elapsed += SessionActiveTimerOnElapsed;
        }

        private void SessionActiveTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (!_isSessionActiveTimerEnabled) return; 

            try
            {
                IsActiveConnection();
            }
            catch (Exception ex)
            {
            }
            finally
            {
              _sessionActiveTimer.Start();
            }
        }
        
        private void IsActiveConnection()
        {
            if (!_amf3Client.Connected)
            {
                _amf3Client.Connect(_url);
            }
           
            _amf3Client.CookieContainer.Add(JSession);

            var isActiveConnectionCallBack = new Amf3Callback();

            isActiveConnectionCallBack.Amf3ResponseEvent += IsActiveConnectionCallBack_Amf3ResponseEvent;

            _amf3Client.Call("", isActiveConnectionCallBack, GetIsActiveMessage());
        }

        private void LoginToken()
        {
            if (!_amf3Client.Connected)
            {
                _amf3Client.Connect(_url);
            }

            var loginCallBack = new Amf3Callback();

            loginCallBack.Amf3ResponseEvent += LoginCallBack_Amf3ResponseEvent;

            _amf3Client.Call("", loginCallBack, GetLoginMessage());
        }

        private void LoginCallBack_Amf3ResponseEvent(object sender, Amf3ResponseEventArgs e)
        {
            if (!e.Call.IsSuccess) return;

            var result = e.Call.Result as AcknowledgeMessage;

            if (result == null) return;

            JSession.Value = result.body.ToString();

            SetRetailX = new Cookie("SetRetailX", GetSetretailXCookieString(), string.Empty, _ip);
        }

        private void IsActiveConnectionCallBack_Amf3ResponseEvent(object sender, Amf3ResponseEventArgs e)
        {
            if (!e.Call.IsSuccess) return;

            var result = e.Call.Result as AcknowledgeMessage;

            if (result?.body == null) return;

            if ((bool) result.body == true) return;

            LoginToken();
        }

        private RemotingMessage GetLoginMessage()
        {
            var msg = new RemotingMessage();
            msg.operation = "login";
            msg.source = null;
            msg.timestamp = 0;
            msg.destination = "sessionLocalService";
            msg.messageId = Guid.NewGuid().ToString();
            msg.clientId = null;
            msg.body = new[] {"admin", "dors1000", "MAIN"};
            msg.timeToLive = 0;
            msg.headers = null;
            return msg;
        }

        private RemotingMessage GetIsActiveMessage()
        {
            var msg = new RemotingMessage();
            msg.operation = "isSessionActive";
            msg.source = null;
            msg.timestamp = 0;
            msg.destination = "sessionLocalService";
            msg.messageId = Guid.NewGuid().ToString();
            msg.clientId = null;
            msg.body = new object[0];
            msg.timeToLive = 0;
            msg.headers = GetHeaderDictionary();
            return msg; 
        }

        private Dictionary<string, object> GetHeaderDictionary()
        {
            var header = new Dictionary<string,object>();

            header.Add("DSId",Guid.NewGuid());

            header.Add("DSEndpoint", null);

            return header;
        }

        private string GetSetretailXCookieString()
        {
            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"login\":\"admin\",\"isCentrum\":false,\"version\":\"10.2.29.0\"");
            sb.Append($",\"sessionId\":\"{JSession}\"");
            sb.Append($",\"serverAddress\":\"{_url}\",\"password\":\"dors1000\"");
            sb.Append("}");
            return sb.ToString();
        }

        #region паблики тут

        public Cookie JSession
        {
            get
            {
                return _jSession;
            }

            private set
            {
                _jSession = value;
            }
        }

        public Cookie SetRetailX
        {
            get
            {
                return _setRetailX;
            }

            set
            {
                _setRetailX = value;
            }
        }

        public void Connect()
        {
            _isSessionActiveTimerEnabled = true;

            _sessionActiveTimer.Start();
        }

        public void Disconnect()
        {
            _isSessionActiveTimerEnabled = false;
        }

        #endregion
    }

    public class Amf3Callback : IPendingServiceCallback
    {
        public event EventHandler<Amf3ResponseEventArgs> Amf3ResponseEvent;

        public void ResultReceived(IPendingServiceCall call)
        {
            OnAmf3Response(new Amf3ResponseEventArgs(call));
        }

        protected virtual void OnAmf3Response(Amf3ResponseEventArgs call)
        {
            Amf3ResponseEvent?.Invoke(this, call);
        }
    }
    
    public class Amf3ResponseEventArgs
    {
        public Amf3ResponseEventArgs(IPendingServiceCall call)
        {
            Call = call;
        }

        public IPendingServiceCall Call { get; set; }
    }
}
