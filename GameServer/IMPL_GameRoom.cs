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
        public RoomAbs(String id, IPEndPoint localEP)
        {
            RoomId = id;
            Reciever = new ReceiverUdpClientBased(localEP);
            Sender = new SenderUdpClientBased(Reciever);
        }

        private List<IGamer> _gamers = new List<IGamer>();

        public string RoomId { get; set; }        

        public IEnumerable<IGamer> Gamers { get { return _gamers; } }

        public virtual void AddGamer(IGamer newGamer)
        {
            _gamers.Add(newGamer);
        }

        public virtual new void RUN()
        {
            base.RUN();
            //if (MessageQueue == null) throw new Exception("MessageQueue object not valid");
            //if (Engine == null) throw new Exception("Engine object not valid");

            //MessageQueue.RUN();
            //Reciever.Run();
        }
    }



    public class ManagingRoom : RoomAbs
    {
        public ManagingRoom(string id, IPEndPoint localEP) : base(id, localEP)
        {
            Reciever = new ReceiverUdpClientBased(localEP);
            base.RegisterDependcy(Reciever);

            Engine = (new ServerEngineFabric()).CreateEngine(SrvEngineType.srvManageEngine);
            base.RegisterDependcy(Engine);

            MessageQueue = (new MessageQueueFabric()).CreateMessageQueue(MsgQueueType.mqOneByOneProcc);
            base.RegisterDependcy(MessageQueue);

        }
    }

    public class GameRoom : RoomAbs
    {
        public GameRoom(string id, IPEndPoint localEP) : base(id, localEP)
        {
            Reciever = new ReceiverUdpClientBased(localEP);
            base.RegisterDependcy(Reciever);

            Engine = (new ServerEngineFabric()).CreateEngine(SrvEngineType.srvGameEngine);
            base.RegisterDependcy(Engine);

            MessageQueue = (new MessageQueueFabric()).CreateMessageQueue(MsgQueueType.mqByTimerProcc);
            base.RegisterDependcy(MessageQueue);

        }
    }


    public class RoomFabric : IRoomFabric
    {
        public IRoom CreateRoom(String roomId, IPEndPoint localEP, RoomType roomType)
        {
            IRoom res = null;

            switch (roomType)
            {
                case RoomType.rtMngRoom:
                    res = new ManagingRoom(roomId, localEP);
                    break;
                case RoomType.rtGameRoom:
                    res = new GameRoom(roomId, localEP);
                    break;
            }
            return res;
        }
    }



}
