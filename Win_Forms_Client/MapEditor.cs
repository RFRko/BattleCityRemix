using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Tanki
{
	public partial class MapEditor : Form
	{
		string path;
		Size objectsSize;
		Rectangle rect;
		Image selected_image;
		BlockType selected_blockType;
		Dictionary<IBlock, Image> blockList;
		bool mouseOnMap;
		public List<IBlock> map;
		public string name;
		bool up = false;

		public MapEditor(Size mapSize, int objectsSize, string filename = null)
		{
			InitializeComponent();

			mouseOnMap = false;
			selected_image = new Bitmap(Tree.Image, new Size(objectsSize, objectsSize));
			selected_blockType = (BlockType)Enum.Parse(typeof(BlockType), Tree.Name);

			this.objectsSize = new Size(objectsSize, objectsSize);
			
			Map_pb.Location = new Point(0, 0);
			Map_pb.Size = mapSize;
			Map_pb.BackColor = Color.Black;

			this.ClientSize = new Size(mapSize.Width + Tree.Size.Width, mapSize.Height);

			Tree.Location = new Point(ClientSize.Width - Tree.Size.Width, Tree.Location.Y);
			Brick2.Location = new Point(ClientSize.Width - Brick2.Size.Width, Brick2.Location.Y);
			Brick.Location = new Point(ClientSize.Width - Brick.Size.Width, Brick.Location.Y);
			Concrete.Location = new Point(ClientSize.Width - Concrete.Size.Width, Concrete.Location.Y);
			Mine.Location = new Point(ClientSize.Width - Mine.Size.Width, Mine.Location.Y);
			Health.Location = new Point(ClientSize.Width - Health.Size.Width, Health.Location.Y);

			Close_btn.Location = new Point(ClientSize.Width - Save_btn.Size.Width, ClientSize.Height - Save_btn.Size.Height);
			Save_btn.Location = new Point(ClientSize.Width - Save_btn.Size.Width, ClientSize.Height - Save_btn.Size.Height - Close_btn.Height);

			blockList = new Dictionary<IBlock, Image>();

			path = Directory.GetCurrentDirectory() + @"\Maps";
			if (filename == null) return;

			filename += ".txt";
			var result = File.ReadAllBytes(path + @"\" + filename);

			var ser = new BinSerializator();
			var blocks = ser.MapDeserialize(result) as List<IBlock>;

			foreach(var i in blocks)
			{
				switch (i.blockType)
				{
					case BlockType.Tree:
						blockList.Add(i, Tree.Image);
						break;
					case BlockType.Brick:
						blockList.Add(i, Brick.Image);
						break;
					case BlockType.Brick2:
						blockList.Add(i, Brick2.Image);
						break;
					case BlockType.Concrete:
						blockList.Add(i, Concrete.Image);
						break;
					case BlockType.Health:
						blockList.Add(i, Health.Image);
						break;
					case BlockType.Mine:
						blockList.Add(i, Mine.Image);
						break;
				}
			} 

		}

		private void Save_btn_Click(object sender, EventArgs e)
		{
			List<IBlock> newMap = new List<IBlock>();

			foreach (var i in blockList) newMap.Add(i.Key);

			var ser = new BinSerializator();
			var result = ser.Serialize(newMap);

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			var saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.InitialDirectory = path;
			saveFileDialog1.DefaultExt = "txt";
			saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
			saveFileDialog1.Title = "Save Map";
			saveFileDialog1.OverwritePrompt = true;

			if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;

			map = newMap;
			File.WriteAllBytes(saveFileDialog1.FileName, result);
			name = saveFileDialog1.FileName;

			Close();
		}

		private void Select(object sender, EventArgs e)
		{
			selected_image = new Bitmap((sender as Button).Image, objectsSize);
			selected_blockType = (BlockType)Enum.Parse(typeof(BlockType), (sender as Button).Name);
		}

		private void Map_pb_Paint(object sender, PaintEventArgs e)
		{
			foreach (var i in blockList)
				e.Graphics.DrawImage(i.Value, i.Key.Position);

			if (mouseOnMap)
				e.Graphics.DrawImage(selected_image, rect);
		}

		private void Map_pb_MouseMove(object sender, MouseEventArgs e)
		{
			mouseOnMap = true;
			rect = new Rectangle(new Point(
				e.X - objectsSize.Height / 2, 
				e.Y - objectsSize.Width / 2), 
				objectsSize);

			if (!up) Map_pb_MouseDown(sender, e);

			Map_pb.Invalidate();
		}
		private void Map_pb_MouseDown(object sender, MouseEventArgs e)
		{
			up = false;
			if (e.Button == MouseButtons.Left)
			{
				var newBlock = new Block();
				newBlock.Position = new Rectangle(new Point(
					e.X - objectsSize.Height / 2,
					e.Y - objectsSize.Width / 2),
					objectsSize);

				if (blockList.FirstOrDefault(
						i => i.Key.Position.IntersectsWith(newBlock.Position)).Key != null
						) return;

				if (!new Rectangle(Map_pb.Location, Map_pb.Size).Contains(newBlock.Position)) return;

				newBlock.Size = objectsSize.Height;
				newBlock.Can_Be_Destroyed = true;
				newBlock.blockType = selected_blockType;
				switch (newBlock.blockType)
				{
					case BlockType.Brick:
						newBlock.HelthPoints = 2;
						break;
					case BlockType.Brick2:
						newBlock.HelthPoints = 3;
						break;
					case BlockType.Concrete:
						newBlock.Can_Be_Destroyed = false;
						break;
					case BlockType.Tree:
						newBlock.HelthPoints = 1;
						break;
					case BlockType.Health:
						newBlock.HelthPoints = 1;
						break;
					case BlockType.Mine:
						newBlock.HelthPoints = 1;
						break;
				}

				blockList.Add(newBlock, selected_image);
			}
			else if (blockList.Count != 0)
			{
				var x = blockList.FirstOrDefault(i => i.Key.Position.Contains(e.X, e.Y)).Key;
				if(x!= null) blockList.Remove(x);
			}
			Map_pb.Invalidate();
		}
		private void Map_pb_MouseLeave(object sender, EventArgs e)
		{
			mouseOnMap = false;
			Cursor.Show();
			Map_pb.Invalidate();
		}
		private void Map_pb_MouseEnter(object sender, EventArgs e)
		{
			mouseOnMap = true;
			Cursor.Hide();
		}
		private void Close_btn_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void MapEditor_MouseUp(object sender, MouseEventArgs e)
		{
			this.up = true;
		}

		private void Map_pb_MouseUp(object sender, MouseEventArgs e)
		{
			up = true;
		}
	}
}
