using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Tanki
{
    public class ReceiverUdpClientBased : IReciever,IUdpClient
    {
        private ReceiverUdpClientBased() { }
        public ReceiverUdpClientBased(IPEndPoint withLockalEP)
        {
            LockalEndPoint = withLockalEP;
            NetClient = new UdpClient(LockalEndPoint);    //было LockalEndPoint.Port
            _recieving_cancel = _recivingCancelTokenSource.Token;
        }
        public IPEndPoint LockalEndPoint { get; private set; }

        public bool Alive { get; set; }

        public UdpClient NetClient { get; }

        public IRecieverClient Owner { get; private set; }

        public INetCommunicationObj GetNetCommunicationObject()
        {
            return this;
        }

        public void Run()
        {
            RecievingThr = new Thread(RecievingProc);
            RecievingThr.Name = "MAIN_RECIEVING_THREAD";
            RecievingThr.Start();
        }

        public void Stop()
        {
            lock (_stoping_locker)
            {
                _stopping_finished.Reset();

                if (RecievingThr != null &&  
                    (RecievingThr.ThreadState == ThreadState.Running ||
                     RecievingThr.ThreadState == ThreadState.Background ||
                     RecievingThr.ThreadState == ThreadState.WaitSleepJoin)
                    )
                {
                    _recieving_cancel = _recivingCancelTokenSource.Token;
                    _recivingCancelTokenSource.Cancel();
                    RecievingThr.Join(5000);
                }
                _stopping_finished.Set();
            }
        }

        private Thread RecievingThr;
        private Object _stoping_locker = new Object();
        private CancellationTokenSource _recivingCancelTokenSource = new CancellationTokenSource();
        private CancellationToken _recieving_cancel;
        private AutoResetEvent _stopping_finished = new AutoResetEvent(false);


        private void RecievingProc()
        {
            Alive = true;
            //UdpClient Client = new UdpClient(LocalPort);
            try
            {
                while (Alive)
                {
                    if (_recieving_cancel.IsCancellationRequested)
                        _recieving_cancel.ThrowIfCancellationRequested();

                    IPEndPoint remoteIp = null;

                    Console.WriteLine("recieving.. ");

                    byte[] data = NetClient.Receive(ref remoteIp);
                    ISerializator obj = new BinSerializator();
                    IPackage p = obj.Deserialize(data);

                    if (p != null) Console.WriteLine("recieved " + p.ToString());

                    Owner.MessageQueue.Enqueue(p);      //!!!!!!!!!!!!!!!!!!!!!!!
                    //return p;
                }
                //return null;
            }
            catch (Exception ex)
            {
                //допишу позже
                Alive = false;
            }
            finally
            {
                //NetClient.Close();                
            }

        }

        public void OnRegistered_EventHandler(object Sender, RegRecieverData evntData)
        {
            Owner = evntData.owner;
        }

        public void Dispose()
        {
            Stop();
            _stopping_finished.WaitOne();
            _recivingCancelTokenSource.Dispose();
            NetClient.Close();            
        }

        //private int localport;
        //public bool Alive
        //{
        //    get
        //    {
        //        return this.alive;
        //    }

        //    set
        //    {
        //        this.alive = value;
        //    }
        //}

        //public int LocalPort
        //{
        //    get
        //    {
        //        return this.localport;
        //    }

        //    set
        //    {
        //        this.localport = value;
        //    }
        //}

        //public IPackage Run()
        //{
        //    Alive = true;
        //    UdpClient Client = new UdpClient(LocalPort);
        //    IPEndPoint remoteIp = null;
        //    try
        //    {
        //        while (Alive)
        //        {
        //            byte[] data = Client.Receive(ref remoteIp);
        //            ISerializator obj = new BinSerializator();
        //            IPackage p = obj.Deserialize(data);
        //            return p;
        //        }
        //        return null;
        //    }
        //    catch (ObjectDisposedException)
        //    {
        //        if (!Alive)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    finally
        //    {
        //        Client.Close();
        //    }
        //}
    }

    public class RegRecieverData: EventArgs
    {
        public IRecieverClient owner { get; set; }
    }

}
