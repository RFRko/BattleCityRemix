namespace Tanki
{
	partial class MapEditor
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Tree = new System.Windows.Forms.Button();
			this.Brick2 = new System.Windows.Forms.Button();
			this.Brick = new System.Windows.Forms.Button();
			this.Concrete = new System.Windows.Forms.Button();
			this.Mine = new System.Windows.Forms.Button();
			this.Health = new System.Windows.Forms.Button();
			this.Save_btn = new System.Windows.Forms.Button();
			this.Map_pb = new System.Windows.Forms.PictureBox();
			this.Close_btn = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.Map_pb)).BeginInit();
			this.SuspendLayout();
			// 
			// Tree
			// 
			this.Tree.BackColor = System.Drawing.Color.Silver;
			this.Tree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.Tree.Image = global::Tanki.Properties.Resources.tree;
			this.Tree.Location = new System.Drawing.Point(0, 0);
			this.Tree.Name = "Tree";
			this.Tree.Size = new System.Drawing.Size(65, 65);
			this.Tree.TabIndex = 0;
			this.Tree.UseVisualStyleBackColor = false;
			this.Tree.Click += new System.EventHandler(this.Select);
			// 
			// Brick2
			// 
			this.Brick2.BackColor = System.Drawing.Color.Silver;
			this.Brick2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.Brick2.Image = global::Tanki.Properties.Resources.wall1;
			this.Brick2.Location = new System.Drawing.Point(0, 65);
			this.Brick2.Name = "Brick2";
			this.Brick2.Size = new System.Drawing.Size(65, 65);
			this.Brick2.TabIndex = 1;
			this.Brick2.UseVisualStyleBackColor = false;
			this.Brick2.Click += new System.EventHandler(this.Select);
			// 
			// Brick
			// 
			this.Brick.BackColor = System.Drawing.Color.Silver;
			this.Brick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.Brick.Image = global::Tanki.Properties.Resources.wall2;
			this.Brick.Location = new System.Drawing.Point(0, 130);
			this.Brick.Name = "Brick";
			this.Brick.Size = new System.Drawing.Size(65, 65);
			this.Brick.TabIndex = 2;
			this.Brick.UseVisualStyleBackColor = false;
			this.Brick.Click += new System.EventHandler(this.Select);
			// 
			// Concrete
			// 
			this.Concrete.BackColor = System.Drawing.Color.Silver;
			this.Concrete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.Concrete.Image = global::Tanki.Properties.Resources.wall3;
			this.Concrete.Location = new System.Drawing.Point(0, 195);
			this.Concrete.Name = "Concrete";
			this.Concrete.Size = new System.Drawing.Size(65, 65);
			this.Concrete.TabIndex = 3;
			this.Concrete.UseVisualStyleBackColor = false;
			this.Concrete.Click += new System.EventHandler(this.Select);
			// 
			// Mine
			// 
			this.Mine.BackColor = System.Drawing.Color.Silver;
			this.Mine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.Mine.Image = global::Tanki.Properties.Resources.Mine;
			this.Mine.Location = new System.Drawing.Point(0, 260);
			this.Mine.Name = "Mine";
			this.Mine.Size = new System.Drawing.Size(65, 65);
			this.Mine.TabIndex = 4;
			this.Mine.UseVisualStyleBackColor = false;
			this.Mine.Click += new System.EventHandler(this.Select);
			// 
			// Health
			// 
			this.Health.BackColor = System.Drawing.Color.Silver;
			this.Health.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.Health.Image = global::Tanki.Properties.Resources.life2;
			this.Health.Location = new System.Drawing.Point(0, 325);
			this.Health.Name = "Health";
			this.Health.Size = new System.Drawing.Size(65, 65);
			this.Health.TabIndex = 5;
			this.Health.UseVisualStyleBackColor = false;
			this.Health.Click += new System.EventHandler(this.Select);
			// 
			// Save_btn
			// 
			this.Save_btn.Location = new System.Drawing.Point(217, 385);
			this.Save_btn.Name = "Save_btn";
			this.Save_btn.Size = new System.Drawing.Size(65, 23);
			this.Save_btn.TabIndex = 6;
			this.Save_btn.Text = "Save";
			this.Save_btn.UseVisualStyleBackColor = true;
			this.Save_btn.Click += new System.EventHandler(this.Save_btn_Click);
			// 
			// Map_pb
			// 
			this.Map_pb.Location = new System.Drawing.Point(149, 26);
			this.Map_pb.Name = "Map_pb";
			this.Map_pb.Size = new System.Drawing.Size(100, 50);
			this.Map_pb.TabIndex = 7;
			this.Map_pb.TabStop = false;
			this.Map_pb.Paint += new System.Windows.Forms.PaintEventHandler(this.Map_pb_Paint);
			this.Map_pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_pb_MouseDown);
			this.Map_pb.MouseEnter += new System.EventHandler(this.Map_pb_MouseEnter);
			this.Map_pb.MouseLeave += new System.EventHandler(this.Map_pb_MouseLeave);
			this.Map_pb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Map_pb_MouseMove);
			this.Map_pb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Map_pb_MouseUp);
			// 
			// Close_btn
			// 
			this.Close_btn.Location = new System.Drawing.Point(217, 414);
			this.Close_btn.Name = "Close_btn";
			this.Close_btn.Size = new System.Drawing.Size(65, 23);
			this.Close_btn.TabIndex = 9;
			this.Close_btn.Text = "Close";
			this.Close_btn.UseVisualStyleBackColor = true;
			this.Close_btn.Click += new System.EventHandler(this.Close_btn_Click);
			// 
			// MapEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 433);
			this.Controls.Add(this.Close_btn);
			this.Controls.Add(this.Map_pb);
			this.Controls.Add(this.Save_btn);
			this.Controls.Add(this.Health);
			this.Controls.Add(this.Mine);
			this.Controls.Add(this.Concrete);
			this.Controls.Add(this.Brick);
			this.Controls.Add(this.Brick2);
			this.Controls.Add(this.Tree);
			this.Name = "MapEditor";
			this.Text = "MapEditor";
			((System.ComponentModel.ISupportInitialize)(this.Map_pb)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button Tree;
		private System.Windows.Forms.Button Brick2;
		private System.Windows.Forms.Button Brick;
		private System.Windows.Forms.Button Concrete;
		private System.Windows.Forms.Button Mine;
		private System.Windows.Forms.Button Health;
		private System.Windows.Forms.Button Save_btn;
		private System.Windows.Forms.PictureBox Map_pb;
		private System.Windows.Forms.Button Close_btn;
	}
}