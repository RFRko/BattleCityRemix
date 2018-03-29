﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanki
{
    public abstract class ServerEngineAbs : IServerEngine
    {
        public ServerEngineAbs(IRoom inRoom) { Room = inRoom; }
        public abstract ProcessMessageHandler ProcessMessage { get; protected set; }
        public abstract ProcessMessagesHandler ProcessMessages { get; protected set; }

        public IRoom Room { get; protected set; }
    }

    public class ServerEngineFabric : IServerEngineFabric
    {
        public IServerEngine CreateEngine(SrvEngineType engineType, IRoom inRoom)
        {
            IServerEngine res = null;

            switch (engineType)
            {
                case SrvEngineType.srvGameEngine:
                    res = new ServerGameEngine(inRoom);
                    break;
            }
            return res;
        }
    }



}
