﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tanki
{
    public abstract class RoomAbs : NetProcessorAbs, IRoom
    {
        private RoomAbs() { }
        public RoomAbs(String id, IPEndPoint localEP, IRoomOwner owner) :base()
        {
            RoomId = id;
            Owner = owner;
            Passport = Guid.NewGuid();
            //Reciever = new ReceiverUdpClientBased(localEP);
            //Sender = new SenderUdpClientBased(Reciever);
        }

        private List<IGamer> _gamers = new List<IGamer>();

        public IRoomOwner Owner { get; protected set; }
        public string RoomId { get; set; }        
        public IEnumerable<IGamer> Gamers { get { return _gamers; } }

		public IGameSetings GameSetings { get; set; }
		public GameStatus Status { get; set; }

        public Guid Passport { get; protected set; }
        public Guid CreatorPassport { get; set ; }

        public virtual void AddGamer(IGamer newGamer)
        {
            _gamers.Add(newGamer);
            OnNewAddresssee?.Invoke(this, new NewAddressseeData() { newAddresssee = newGamer });
        }

        public virtual new void RUN()
        {
            base.RUN();
            //if (MessageQueue == null) throw new Exception("MessageQueue object not valid");
            //if (Engine == null) throw new Exception("Engine object not valid");

            //MessageQueue.RUN();
            //Reciever.Run();
        }

        public IEnumerable<IGamer> GetAddresssees() {return _gamers;}

        public IRoomStat getRoomStat()
        {
            return new RoomStat() { Pasport = this.Passport, Players_count = Gamers.Count(), Creator_Pasport = this.CreatorPassport };
        }

        public IAddresssee this[string id] {
            get
            {
                var v = from g in _gamers where g.Name == id select g;
                if (v.Count() > 1) throw new Exception(" addresssee not unique") ;
                return v.First();
            } }
        public event EventHandler<NewAddressseeData> OnNewAddresssee;
	}



    public class ManagingRoom : RoomAbs, IManagerRoom
    {
        public ManagingRoom(string id, IPEndPoint localEP, IRoomOwner owner, IEngine engine = null) : base(id, localEP, owner)
        {
            IReciever _Reciever = new ReceiverUdpClientBased(localEP);
            base.RegisterDependcy(_Reciever);

            Sender = new SenderUdpClientBased(Reciever);

            IEngine _Engine;
            if (engine != null)
                _Engine = engine;
            else
                _Engine = (new ServerEngineFabric()).CreateEngine(SrvEngineType.srvManageEngine);
            base.RegisterDependcy(_Engine);

            IMessageQueue _MessageQueue = (new MessageQueueFabric()).CreateMessageQueue(MsgQueueType.mqOneByOneProcc);
            base.RegisterDependcy(_MessageQueue);

        }

        public IGamer GetGamerByGuid(Guid gamerGuid)
        {
            IGamer foundGamer = null;

            var g = (from G in Gamers where G.Passport == gamerGuid select G);
            if (g.Count() > 1) throw new Exception("Rooms ID not unique");

            foundGamer = g.FirstOrDefault();

            return foundGamer;
        }

        public IEnumerable<IRoomStat> getRoomsStat()
        {
            IManagerRoomOwner mO = Owner as IManagerRoomOwner;
            return mO.getRoomsStat();
        }

        public IRoomStat getRoomStat(String forRoomID)
        {
            IManagerRoomOwner mO = Owner as IManagerRoomOwner;
            return mO.getRoomStat(forRoomID);
        }

        public IPEndPoint MooveGamerToRoom(IGamer gamer, Guid TargetRoomId)
        {
            IManagerRoomOwner mO = Owner as IManagerRoomOwner;
            return mO.MooveGamerToRoom(gamer,TargetRoomId);
        }

        public IRoom AddRoom(IGameSetings gameSettings, Guid Creator_Passport)
        {
            IManagerRoomOwner mO = Owner as IManagerRoomOwner;
            return mO.AddRoom(gameSettings, Creator_Passport);
        }

    }

    public class GameRoom : RoomAbs, IGameRoom
    {
        public GameRoom(string id, IPEndPoint localEP, IRoomOwner owner, IEngine engine = null) : base(id, localEP, owner)
        {
            Reciever = new ReceiverUdpClientBased(localEP);
            base.RegisterDependcy(Reciever);

            Sender = new SenderUdpClientBased(Reciever);

            IEngine _Engine;
            if (engine != null)
                _Engine = engine;
            else
                _Engine = (new ServerEngineFabric()).CreateEngine(SrvEngineType.srvGameEngine);
            base.RegisterDependcy(_Engine);

            MessageQueue = (new MessageQueueFabric()).CreateMessageQueue(MsgQueueType.mqByTimerProcc);
            base.RegisterDependcy(MessageQueue);

        }

        public event EventHandler<GameStatusChangedData> OnNewGameStatus;
    }


    public class RoomFabric : IRoomFabric
    {
        public IRoom CreateRoom(String roomId, IPEndPoint localEP, RoomType roomType, IRoomOwner owner, IEngine engine = null)
        {
            IRoom res = null;

            switch (roomType)
            {
                case RoomType.rtMngRoom:
                    res = new ManagingRoom(roomId, localEP, owner, engine);
                    break;
                case RoomType.rtGameRoom:
                    res = new GameRoom(roomId, localEP, owner, engine);
                    break;
            }
            return res;
        }
    }



}
