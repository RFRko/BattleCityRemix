﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    #region GamerInterfaces
    public interface IGamer: IAddresssee
    {
        String Name { get;}
        Guid Passport { get; }
        IPEndPoint RemoteEndPoint { get; }
		//Socket Socket { get; set; }
		void SetId(String Name, Guid confirmpassport);
    }


    public interface IAddressseeHolderBase
    {
        event EventHandler<NewAddressseeData> OnNewAddresssee;
        event EventHandler<RemoveAddressseeData> OnRemoveAddresssee;
        event EventHandler<AddressseeHolderFullData> OnAddressseeHolderFull;
    }

    public interface IAddressseeHolder<T, Tid>: IAddressseeHolderBase where T:IAddresssee
    {
        IEnumerable<T> GetAddresssees();
        IAddresssee this[Tid id] { get; }
    }


    public interface IAddressseeHolderClient
    {

        void OnNewAddresssee_Handler(Object Sender, NewAddressseeData evntData);
        void OnRemoveAddresssee_Handler(Object Sender, RemoveAddressseeData evntData);
        void OnAddressseeHolderFull_Handler(Object Sender, AddressseeHolderFullData evntData);
    }

    #endregion GamerInterfaces

    #region RoomInterfaces

    public enum RoomType
    {
        rtMngRoom,
        rtGameRoom,
        rtAbstract
    }

    /// <summary>
    /// Нужна для:
    /// -IServer (библиотека GameServer)
    /// -ServerGameEngine (библиотека ServerEngine)
    /// </summary>
    public interface IRoom: INetProcessor, IAddressseeHolder<IGamer,String>
    {
        IRoomOwner Owner { get; }
        String RoomId { get; set; }
        Guid Passport { get; }
        Guid CreatorPassport { get; set; }
        IGameSetings GameSetings { get; set; }
        IEnumerable<IGamer> Gamers { get; }
        void AddGamer(IGamer newGamer);
        void RemoveGamer(IGamer delGamer);
        void RemoveGamer(Guid delGamerId);
        IRoomStat getRoomStat();
		GameStatus Status { get; set; }
        RoomType Room_Type { get; }

        void RUN();
    }


    //все что нужно для Server Manage Engine 
    public interface IManagerRoom
    {
        IRoomStat getRoomStat(String forRoomID);
        IEnumerable<IRoomStat> getRoomsStat();
        IPEndPoint MooveGamerToRoom(IGamer gamer, Guid TargetRoomId);
        void RemoveGamerFromRoom(IGamer gamer, Guid TargetRoomId);
        IRoom AddRoom(IGameSetings gameSettings, Guid Creator_Passport);
        IRoom GetRoomByGuid(Guid roomGuid);
        IGamer GetGamerByGuid(Guid gamerGuid);
    }

    public interface IRoomOwner
    {
        IRoomStat getRoomStat(String RoomID);
    }

    public interface IManagerRoomOwner: IRoomOwner
    {
        IEnumerable<IRoomStat> getRoomsStat();
        IPEndPoint MooveGamerToRoom(IGamer gamer, Guid TargetRoomId);
        void RemoveGamerFromRoom(IGamer gamer, Guid TargetRoomId);
        IRoom AddRoom(IGameSetings gameSettings, Guid Creator_Passport);
        IRoom GetRoomByGuid(Guid roomGuid);
    }


    public interface IGameRoom
    {
        Int32 MaxPlayerCount { get; }
        IGamer Creator { get; }
        void NotifyGameRoomForEvent(EventArgs evntData);
        event EventHandler<GameStatusChangedData> OnNewGameStatus;
        event EventHandler<NotifyJoinedPlayerData> OnNotifyJoinedPlayer;
        event EventHandler<NotifyStartGameData> OnNotifyStartGame;
        event EventHandler<NotifyRemovePlayerData> OnNotifyRemovePlayer;

    }


    public interface IRoomFabric
    {
        IRoom CreateRoom(String roomId, IPEndPoint localEP, RoomType roomType, IRoomOwner owner, IEngine engine = null);
    }





    #endregion RoomInterfaces

    #region GameClientInterfaces


    public interface IGameClient
    {
        IEntity ClientGameState { get; set; }
        void RUN();  //запускает базовый NetProcessorAbs.RUN (очередь\reciver)
        void STOP(); // останавленивает базовый NetProcessorAbs.STOP (очередь\reciver)
        #region moved_to_client_engine
        //void RUN_GAME(); // запускает таймер переодической отправки клиентского состоянения игры на сервер
        //void STOP_GAME(); // останавливает таймер переодической отправки клиентского состоянения игры на сервер
        #endregion moved_to_client_engine
        Guid Passport { get; set; }
        bool Connect(IPEndPoint ServerEndPoint);
		void AddAddressee(String Id, IAddresssee addresssee);
		IAddresssee this[String id] { get; }
	}

    public interface IClient
    {
        //void AddAddressee(String Id, IAddresssee addresssee);   // добавляем нового адресата 

        //IAddresssee this[String id] { get; } //свойство идексатор для возврата Адресата по текстовому имени\ид.  
                                             //Адресат это объект с IPEndPoint комнаты (может быть как минимум два аддерсата - управляющая комната, текущая игровая комната


        void RUN(); // создаем тспклиент с serverendpoint, через него запускает базовый NetProcessorAbs.RUN (очередь\reciver), коннектится к cерверу
        void STOP();
        IEntity ClientGameState { get; set; }   // польностью вернуть объект
        void OnClientGameStateChangedHandler(Object Sender, GameStateChangeData evntData); // просто реализовать метод на котрый что-то подпишеи
        event EventHandler<EnforceDrawingData> EnforceDrawing;  // дернет движок, просто делегат

    }

    


    public interface IGameEngineOwner: INetProcessor, IGameClient
    {
    }


    #endregion GameClientInterfaces

}
