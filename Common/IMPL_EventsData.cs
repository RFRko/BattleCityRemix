using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tanki
{

    public class RegMsgQueueData : EventArgs
    {
        public IMessageQueueClient MsgQueueOwner { get; set; }
    }

    public class RegEngineData : EventArgs
    {
        public IEngineClient EngineOwner { get; set; }
    }

    public class NewAddressseeData : EventArgs
    {
        public IAddresssee newAddresssee { get; set; }
    }

    public class RemoveAddressseeData : EventArgs
    {
        public IAddresssee removeddresssee { get; set; }
    }



    public class RoomConnect : EventArgs
	{
		public Size MapSize { get; set; }
	}
	public class DestroyableTank : EventArgs
	{
		public ITank tankToDestroy { get; set; }
	}

	public class ErrorData : EventArgs
	{
		public string errorText { get; set; }
	}

	public class GameStateChangeData : EventArgs
    {
        public IMap newMap { get; set; }
    }

    public class EnforceDrawingData : EventArgs
    {
        public IEntity mustDrawTheEntity { get; set; }
    }

    public class RoomStatChangeData : EventArgs
    {
        public IEnumerable<IRoomStat> newRoomsStat { get; set; }
    }

    public class GameStatusChangedData : EventArgs
	{
		  public GameStatus newStatus { get; set; }
	}

    public class NetProcStartedEvntData : EventArgs
    {
        public Boolean Started { get; set; }
    }

    public class NetProcBeforStartedEvntData : EventArgs
    {
        public Boolean BeforStart { get; set; }
    }


    public class AddressseeHolderFullData : EventArgs
    {
        public Boolean isFull { get; set; }
        public IAddresssee lastAddedAdresssee { get; set; }
    }

    
    public class NotifyJoinedPlayerData : EventArgs
    {
        public IAddresssee JoinedAddresssee { get; set; }
    }

    public class NotifyStartGameData : EventArgs
    {
        public Boolean EnforceStartGame { get; set; }
    }

    
    public class NotifyRemovePlayerData : EventArgs
    {
        public Guid RemovedPlayerPassport { get; set; }
    }

    public class NotifyMustRemoveRoom : EventArgs
    {
        public IRoom Room2remove { get; set; }
    }
    

}
