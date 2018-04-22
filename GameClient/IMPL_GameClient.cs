using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tanki
{
    public class GameClient:NetProcessorAbs, IClient, IGameClient, IEngineClient
    {
        private Dictionary<string, IAddresssee> adresee_list;       // приватный Dictionary<String, IAddresssee>  для хранения перечня адрессатов
        private TcpClient tcp;                                      // должен быть приватный TCPClient  для коннекта к хосту
        private IEntity clientGameState;
        private int miliseconds;
        public Guid Passport { get; set; }
        private TimerCallback tm;                                   //должен быть приватный Timer - на callBack которого будет вызываться метод переодической отправки клинтского состояния игры на сервер.
        private IPackage package;
        private IPEndPoint endpoint;
        

        public event EventHandler<EnforceDrawingData> EnforceDrawing;

        //взять этот за основу НУЖЕН НОВЫЙ КОНСТРУКТОР!!!!
        public GameClient(IPEndPoint localEP, IRoomOwner owner = null) 
        {
			this.miliseconds = 500;
            this.adresee_list = new Dictionary<string, IAddresssee>();
            this.tcp = new TcpClient(localEP);
            this.package = new Package();

            IReciever _Reciever = new ReceiverUdpClientBased(localEP);
            base.RegisterDependcy(_Reciever);
            base.Sender = new SenderUdpClientBased(Reciever);

			IEngine _Engine = new ClientEngine();
            // Нужно будет прописать создание клиентского Engine
            //IEngine _Engine =  (new ServerEngineFabric()).CreateEngine(SrvEngineType.srvManageEngine);
            base.RegisterDependcy(_Engine);

            //entity = _engine.Entity;

            IMessageQueue _MessageQueue = (new MessageQueueFabric()).CreateMessageQueue(MsgQueueType.mqOneByOneProcc);
            base.RegisterDependcy(_MessageQueue);
		}

        public void AddAddressee(string Id, IAddresssee addresssee)
        {
            this.adresee_list.Add(Id, addresssee);
        }

        public IAddresssee this[string id]
        {
            get
            {
                if(this.adresee_list[id] != null)
                {
                    return this.adresee_list[id];
                }
                else
                {
                    return null;
                }
            }
        }


        public IEntity ClientGameState
        {
            get
            {
                return this.clientGameState;
            }

            set
            {
                this.clientGameState = value;
            }
        }


        public int MiliSeconds
        {
            get
            {
                return this.miliseconds;
            }
            set
            {
                this.miliseconds = value;
            }
        }

        private Timer _timer;
        private CancellationTokenSource _sendingTimerCancelTokenSource = new CancellationTokenSource();
        private CancellationToken _sendingTimerCancelToken;
        private AutoResetEvent _timer_dispose = new AutoResetEvent(false);

        // Guid IGameClient.Passport { get; set ; }

        public void RUN()   // запускает базовый NetProcessorAbs.RUN (очередь\reciver)
        {
            base.RUN();
        }
        public void STOP()  // останавливает базовый NetProcessorAbs.STOP (очередь\reciver)
        {
            base.STOP();
        }


        #region moved_to_client_engine
        //public void RUN_GAME()    // запускает таймер переодической отправки клиентского состоянения игры на сервер
        //{
        //    _sendingTimerCancelToken = _sendingTimerCancelTokenSource.Token;
        //    this.tm = new TimerCallback(ProceedQueue);
        //    _timer = new Timer(tm, (Engine as IClientEngine).Entity, 0, this.MiliSeconds);

        //}

        //public void STOP_GAME()   // останавливает таймер переодической отправки клиентского состоянения игры на сервер
        //{
        //    //Reciever.Alive = false;
        //    _sendingTimerCancelTokenSource.Cancel();
        //    _timer.Dispose(_timer_dispose);
        //    _timer_dispose.WaitOne();
        //}
        #endregion moved_to_client_engine

        private void ProceedQueue(object state)          //должен будет быть приватный метод  'void ProceedQueue(Object state)' который будет передаваться time-ру как callback 
        {                                                           // этот метод должен с периодиностью таймера отправлять клиентское состояние игры на сервер    

            if (_sendingTimerCancelToken.IsCancellationRequested)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                return;
            }

            var tosend = state as IEntity;
            var packagee = new Package()
            {
                Sender_Passport = this.Passport,
                Data = tosend,
                MesseggeType = MesseggeType.Entity
            };

            //this.clientGameState = (IEntity)state;
            // отправка данных
            //this.package.Data = clientGameState;
            Sender.SendMessage(packagee, adresee_list["Room"]);
            
        }

        public bool Connect(IPEndPoint ServerEndPoint)
        {
			try
			{
				tcp.Connect(ServerEndPoint.Address, ServerEndPoint.Port);
				return true;
			}
			catch { return false; };
        }

        public void OnClientGameStateChangedHandler(object Sender, GameStateChangeData evntData)
        {
            throw new NotImplementedException();
        }
    }
}
