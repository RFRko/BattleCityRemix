﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tanki
{
    /// <summary>
    /// Добавлен для включения интерфейсов, которые не являются общими (характерны для конкретных библиотек),
    /// Но не могут быть в них объявлены из-за возникновения циклических ссылок при dependecy injection
    /// например ServerEngine должен иметь dependency Room, но Room в библиотеке Server, которой нужна dependency ServerEngine
    /// </summary>
    /// 

    public interface IGamer
    {
        String id { get; set; }
        Socket Socket { get; set; }
    }


    /// <summary>
    /// Нужна для:
    /// -IServer (библиотека GameServer)
    /// -ServerGameEngine (библиотека ServerEngine)
    /// </summary>
    public interface IRoom
    {
        String RoomId { get; set; }
        Socket RoomListner { get; }
        IEnumerable<IGamer> Gamers { get; }
        void AddGamer(IGamer newGamer);

        IMessageQueue MessageQueue { get; }
        ISender Sender { get; }
        void RUN();

    }

}
