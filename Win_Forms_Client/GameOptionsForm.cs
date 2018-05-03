using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanki
{
	public partial class GameOptionsForm : Form
	{
		public GameSetings gameSetings;
		public bool ok;
		private string selectedItem;
		private List<IBlock> map;

		public GameOptionsForm()
		{
			InitializeComponent();
			CreateRoom_btn.TabIndex = 0;
		}

		private void GameOptionsForm_Load(object sender, EventArgs e)
		{
			var enumlist = Enum.GetNames(typeof(GameType));
			VictoryCondition_cb.DataSource = enumlist;

			MapList_cb.Items.Add("Auto Generate Map");
			MapList_cb.SelectedItem = MapList_cb.Items[0];

			string[] dirs;
			var path = Directory.GetCurrentDirectory() + @"\Maps";
			if (!Directory.Exists(path)) return;
				dirs = Directory.GetFiles(path);

			foreach (var i in dirs)
			{
				var f_name = i.Split('\\').Last().Replace(".txt","");
				MapList_cb.Items.Add(f_name);
			}
		}

		private void CreateRoom_btn_Click(object sender, EventArgs e)
		{
			var gameSpeed = (int)GameSpeed_nud.Value;
			var object_size = (int)ObjectSize_nud.Value;
			Size mapSize = new Size(
					(int)MapWidth_nud.Value,
					(int)MapHeight_nud.Value);
			var players_count = (int)NamberOfPlayer_nud.Value;
			var game_type = (GameType)Enum.Parse(typeof(GameType), VictoryCondition_cb.SelectedItem.ToString());

			gameSetings = new GameSetings()
			{
				GameSpeed = gameSpeed,
				ObjectsSize = object_size,
				MapSize = mapSize,
				MaxPlayersCount = players_count,
				GameType = game_type,
			};
			if (this.map != null) gameSetings.blocklist = this.map;

			ok = true;
			Close();
		}

		private void Cancel_btn_Click(object sender, EventArgs e)
		{
			ok = false;
			Close();
		}

		private void EditMap_btn_Click(object sender, EventArgs e)
		{
			MapEditor editor;
			if (selectedItem != null)
			editor = new MapEditor(
				new Size((int)MapWidth_nud.Value, (int)MapHeight_nud.Value), 
				(int)ObjectSize_nud.Value, selectedItem);
			else
				editor = new MapEditor(
				new Size((int)MapWidth_nud.Value, (int)MapHeight_nud.Value),
				(int)ObjectSize_nud.Value);
			editor.ShowDialog();

			MapList_cb.Items.Clear();
			GameOptionsForm_Load(this, e);
		}

		private void MapList_cb_SelectedIndexChanged(object sender, EventArgs e)
		{
			var item = MapList_cb.SelectedItem.ToString();
			if (item == "Auto Generate Map") { selectedItem = null; return; }
			else selectedItem = item;

			map = new List<IBlock>();

			var path = Directory.GetCurrentDirectory() + @"\Maps";

			item += ".txt";
			var result = File.ReadAllBytes(path + @"\" + item);

			var ser = new BinSerializator();
			var blocks = ser.MapDeserialize(result) as List<IBlock>;

			foreach (var i in blocks) map.Add(i);
		}
	}
}
