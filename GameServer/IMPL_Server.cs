﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tanki
{
    class GameServer: ListeningClientAbs,  IServer
    {
        private GameServer() { }

        private GameServer(IListener listener)
        {
            ServerListner = listener;
            RegisterListener(ServerListner);
        }

        private List<IRoom> _rooms = new List<IRoom>();

        public IListener ServerListner { get; private set; }
        public IEnumerable<IRoom> Rooms { get { return _rooms; } }

        public override void OnNewConnectionHandler(object Sender, NewConnectionData evntData)
        {
            var remoteEP = (IPEndPoint)evntData.RemoteClientSocket.RemoteEndPoint;
            Gamer newGamer = new Gamer(remoteEP);
            _rooms[0].AddGamer(newGamer);
        }

        public void RUN()
        {
            //

            IPAddress roomAddr = ((IPEndPoint)ServerListner.ipv6_listener.LocalEndPoint).Address;
            Int32 roomPort = 10001;
            IPEndPoint roomEP = new IPEndPoint(roomAddr, roomPort);

            IRoom managerRoom = (new RoomFabric()).CreateRoom("", roomEP, RoomType.rtMngRoom);
            _rooms.Add(managerRoom);
            managerRoom.RUN();

            ServerListner.RUN();
        }
    }
}
