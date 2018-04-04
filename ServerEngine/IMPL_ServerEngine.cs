﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Tanki
{
	/// <summary>
	/// Реализация игрового движка
	/// </summary>
    public class ServerGameEngine : EngineAbs
    {
		/// <summary>
		/// Делегат принимающий сообщение от MessageQueue
		/// </summary>
        public override ProcessMessageHandler ProcessMessage { get; protected set; }
		/// <summary>
		/// Делегат принимающий сообщения от MessageQueue
		/// </summary>
		public override ProcessMessagesHandler ProcessMessages { get; protected set; }
		/// <summary>
		/// Конструктор игрового движка
		/// </summary>
        public ServerGameEngine() : base() { }
		/// <summary>
		/// Конструктор игрового движка
		/// </summary>
		/// <param name="room">Значение Owner базового абстрактного класса</param>
		public ServerGameEngine(IRoom room):base(room) 
		{
			this.ProcessMessages += MessagesHandler;
            this.ProcessMessage = null;
		}
		/// <summary>
		/// Ширина игрового поля
		/// </summary>
		public int Width { get { return this.width;	} set {	this.width = value;	}}
		/// <summary>
		/// Высота игрового поля
		/// </summary>
		public int Height { get { return this.height; } set { this.height = value; } }
		/// <summary>
		/// Список всех сущностей на игровом поле
		/// </summary>
        private List<IEntity> objects;
        private int width;
        private int height;
		/// <summary>
		/// Список всех танков на игровом поле
		/// </summary>
        private List<ITank> tanks = new List<ITank>();
		/// <summary>
		/// Список всех блоков на игровом поле
		/// </summary>
        private List<IBlock> blocks = new List<IBlock>();
		/// <summary>
		/// Список всех пуль на игровом поле
		/// </summary>
        private List<IBullet> bullets = new List<IBullet>();

		/// <summary>
		/// Метод реализирующий проверку, выполнено ли условие победы в игре
		/// </summary>
		/// <returns>Возвращает закончена ли игра</returns>
        private bool CheckWin()
		{
			return true;
		}
		/// <summary>
		/// Метод реализирующий проверку списка сущностей на наличие убитых
		/// </summary>
		/// <param name="package"> Список сущностей, который подлежит проверке на "мертвых"</param>
		private void CheckAlive(IEnumerable<IPackage> package)
		{
			Parallel.ForEach(package, item => {
				var entity = item.Data as IEntity;
				if (!entity.Is_Alive)
				{
					if (entity is ITank)
					{
						var tank = entity as ITank;
						if (tank.Lives > 0)
						{
							this.Reload(entity);
						}
						else
						{
							tanks.Remove(entity as ITank);
							objects.Remove(entity);
						}
					}
					if (entity is IBlock)
					{
						blocks.Remove(entity as IBlock);
						objects.Remove(entity);
					}
					if (entity is IBullet)
					{
						bullets.Remove(entity as IBullet);
						objects.Remove(entity);
					}
				}
			});


			//foreach (var item in package)   РЕАЛИЗАЦИЯ НЕ ПАРАЛЕЛЬНО, НА ВСЯКИЙ СЛУЧАЙ
			//{
			//	var entity = item.Data as IEntity;
			//	if (!entity.Is_Alive)
			//	{
			//		if (entity is ITank)
			//		{
			//			var tank = entity as ITank;
			//			if (tank.Lives > 0)
			//			{
			//				this.Reload(entity);
			//			}
			//			else
			//			{
			//				tanks.Remove(entity as ITank);
			//				objects.Remove(entity);
			//			}
			//		}
			//		if (entity is IBlock)
			//		{
			//			blocks.Remove(entity as IBlock);
			//			objects.Remove(entity);
			//		}
			//		if (entity is IBullet)
			//		{
			//			bullets.Remove(entity as IBullet);
			//			objects.Remove(entity);
			//		}
			//	}
			//}
		}
		/// <summary>
		/// Реализация делегата ProcessMessagesHandler
		/// </summary>
		/// <param name="list">Список пакетов переданый движку на обработку</param>
		private void MessagesHandler(IEnumerable<IPackage> list)
		{
			this.CheckAlive(list);
			foreach(var x in bullets) // могу и Эту чепуху сделать паралельной, она на работу не повлияет
			{
				this.Move(x);
			}
			foreach(var t in list)
			{
                var tmp = t.Data as IEntity;
                if (tmp.Command == EntityAction.Move)
                {
                    this.Move(tmp);
                }
                if (tmp.Command == EntityAction.Fire)
				{ 
                    this.Fire(tmp);
                }
            }
        }
		/// <summary>
		/// Метод реализирующий обработку "убитой" сущности
		/// </summary>
		/// <param name="entity">"Убитая" сущность</param>
		private void Death(IEntity entity) 
        {
			var tmp = objects.FirstOrDefault(t => t==entity);
			if (tmp is ITank)
			{
				var tank = tmp as ITank;
				if (tank.Lives > 0)
				{
					tank.Lives--;
				}
			}
			else if(tmp is IBullet)
			{
				var bullet = tmp as IBullet;
				tanks.FirstOrDefault(t => t.Tank_ID == bullet.Parent_Id).Can_Shoot = true;
			}
			tmp.Is_Alive = false;
        }
		/// <summary>
		/// Метод реализирующий выстрел
		/// </summary>
		/// <param name="entity">Сущность осуществившая выстрел</param>
		private void Fire(IEntity entity)
		{
			var tmp = objects.FirstOrDefault(t => t == entity) as ITank;
			if (tmp.Can_Shoot)
			{
				var bullet = new object() as IBullet;
				bullet.Direction = tmp.Direction;				
				bullet.Parent_Id = tmp.Tank_ID;
				tanks.FirstOrDefault(t=>t==tmp).Can_Shoot = false;
				bullet.Is_Alive = true;
				bullet.Can_Be_Destroyed = false;
				bullet.Can_Shoot = false;
				bullet.Position = tmp.Position;
				bullet.Command = EntityAction.Move;
				bullets.Add(bullet);
			}
			entity.Command = EntityAction.None;
		}
		/// <summary>
		/// Генерация сущностей на игровом поле
		/// </summary>
        private void GenerateMap()
		{
            var room = Owner as IRoom;
			int tankCount = room.Gamers.Count();
			int objectCount = (this.height * this.width) / (this.height + this.width);
            foreach (var t in room.Gamers)
            {
				var obj = new object() as ITank;
				obj.Tank_ID = t.Passport;
				obj.Lives = 5;
				obj.Is_Alive = true;
				obj.Can_Shoot = true;
				obj.Direction = Direction.Up;
				this.Reload(obj);
				tanks.Add(obj);
				objects.Add(obj);
			}
			while(objectCount>0)
			{
				var obj = new object() as IBlock;
				this.Reload(obj);
				obj.Can_Be_Destroyed = true;
				obj.Can_Shoot = false;
				obj.Is_Alive = true;
				blocks.Add(obj);
				objects.Add(obj);
				objectCount--;
			}
		}
		/// <summary>
		/// Метод реализирующий движение сущности
		/// </summary>
		/// <param name="entity">Сущность осуществляющая движение</param>
		private void Move(IEntity entity)
		{
			var tmp = objects.FirstOrDefault(t => t == entity);
			if (tmp.Is_Alive)
			{
				if (tmp is ITank)
				{
					var tank = tmp as ITank;
					switch (tank.Direction)
					{
						case Direction.Left:
							if (tank.Position.X > 0)
							{
								var pos = new Point(tank.Position.X - 1, tank.Position.Y);
								tank.Position = new Rectangle(pos, new Size(tank.Size, tank.Size));
							}
							break;

						case Direction.Right:
							if (tank.Position.X < width)
							{
								var pos = new Point(tank.Position.X + 1, tank.Position.Y);
								tank.Position = new Rectangle(pos, new Size(tank.Size, tank.Size));
							}
							break;

						case Direction.Up:
							if (tank.Position.Y > 0)
							{
								var pos = new Point(tank.Position.X, tank.Position.Y - 1);
								tank.Position = new Rectangle(pos, new Size(tank.Size, tank.Size));
							}
							break;

						case Direction.Down:

							if (tank.Position.Y < height)
							{
								var pos = new Point(tank.Position.X, tank.Position.Y + 1);
								tank.Position = new Rectangle(pos, new Size(tank.Size, tank.Size));
							}
							break;
					}
					tank.Command = EntityAction.None;
				}
				else if (tmp is IBullet)
				{
					var bullet = tmp as IBullet;
					switch (bullet.Direction)
					{
						case Direction.Left:
							if (bullet.Position.X > 0)
							{
								var pos = new Point(bullet.Position.X - 1, bullet.Position.Y);
								bullet.Position = new Rectangle(pos, new Size(bullet.Size, bullet.Size));
							}
							break;

						case Direction.Right:
							if (bullet.Position.X < width)
							{
								var pos = new Point(bullet.Position.X + 1, bullet.Position.Y);
								bullet.Position = new Rectangle(pos, new Size(bullet.Size, bullet.Size));
							}
							break;

						case Direction.Up:
							if (bullet.Position.Y > 0)
							{
								var pos = new Point(bullet.Position.X, bullet.Position.Y - 1);
								bullet.Position = new Rectangle(pos, new Size(bullet.Size, bullet.Size));
							}
							break;

						case Direction.Down:

							if (bullet.Position.Y < height)
							{
								var pos = new Point(bullet.Position.X, bullet.Position.Y + 1);
								bullet.Position = new Rectangle(pos, new Size(bullet.Size, bullet.Size));
							}
							break;
					}
					this.HitTarget(bullet);
				}
			}
        }
		/// <summary>
		/// Реализация попадания пули в другую сущность
		/// </summary>
		/// <param name="bullet">Пуля попавшая в "цель"</param>
		private void HitTarget(IBullet bullet)
		{
			var tmp = objects.FirstOrDefault(tank => tank.Position.IntersectsWith(bullet.Position));
			if(tmp!=null)
			{
				this.Death(tmp);
				this.Death(bullet);
			}
		}
		/// <summary>
		/// Гененация расположения для сущности
		/// </summary>
		/// <param name="entity">Сущность требующая разположения на игровом поле</param>
		private void Reload(IEntity entity)
        {
			entity.Position = Rectangle.Empty;
			while (entity.Position != Rectangle.Empty)
			{
				Random colInd = new Random(DateTime.Now.Millisecond - 15);
				Random rowInd = new Random(DateTime.Now.Millisecond + 20);
				int columnIndex = colInd.Next(0, width);
				int rowIndex = rowInd.Next(0, height);
				Point p = new Point(rowIndex, columnIndex);
				if (objects.FirstOrDefault(tank => tank.Position.IntersectsWith(new Rectangle(p, new Size(entity.Size, entity.Size))) == true) == null)
				{
					entity.Position = new Rectangle(p,new Size(entity.Size,entity.Size));
				}
			}
		}
		/// <summary>
		/// Метод реализирующий передачу данных на сендер
		/// </summary>
		public void Send()
		{       
            var t = new object() as IMap;
            t.Blocks = blocks;
            t.Bullets = bullets;
            t.Tanks = tanks;
			var pack = new object() as IPackage;
			pack.Data = t;
			var adress = Owner as IRoom;
			Owner.Sender.SendMessage(pack, adress.Gamers);
        }



		// Нужно вызывать эту чепуху при новом игроке в комнате, метод ниже мне не подходит, по причине - мне не нужен ендпоинт, мне нужен гуид
		public void NewGamer(IGamer gamer)
		{
			var obj = new object() as ITank;
			obj.Tank_ID = gamer.Passport;  
			obj.Lives = 5;
			obj.Is_Alive = true;
			obj.Can_Shoot = true;
			obj.Direction = Direction.Up;
			this.Reload(obj);
			tanks.Add(obj);
			objects.Add(obj);
		}

		//или сендер здесь и есть IGamer?
        public override void OnNewAddresssee_Handler(object Sender, NewAddressseeData evntData)
        {
            throw new NotImplementedException();
        }
    }
}
