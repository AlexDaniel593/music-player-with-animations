namespace MusicPlayer
{
    partial class FrmIndex
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmIndex));
            this.controlPanel = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.lblTotalTime = new System.Windows.Forms.Label();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.positionSlider = new System.Windows.Forms.TrackBar();
            this.cmbVisualizationType = new System.Windows.Forms.ComboBox();
            this.volumenSlider = new System.Windows.Forms.TrackBar();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.playlistPanel = new System.Windows.Forms.Panel();
            this.playlistBox = new System.Windows.Forms.ListBox();
            this.btnClearPlaylist = new System.Windows.Forms.Button();
            this.btnAddFiles = new System.Windows.Forms.Button();
            this.playlistTitle = new System.Windows.Forms.Label();
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.volumenSlider)).BeginInit();
            this.playlistPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.controlPanel.Controls.Add(this.btnNext);
            this.controlPanel.Controls.Add(this.btnPrevious);
            this.controlPanel.Controls.Add(this.lblTotalTime);
            this.controlPanel.Controls.Add(this.lblCurrentTime);
            this.controlPanel.Controls.Add(this.lblFileName);
            this.controlPanel.Controls.Add(this.positionSlider);
            this.controlPanel.Controls.Add(this.cmbVisualizationType);
            this.controlPanel.Controls.Add(this.volumenSlider);
            this.controlPanel.Controls.Add(this.btnStop);
            this.controlPanel.Controls.Add(this.btnPause);
            this.controlPanel.Controls.Add(this.btnPlay);
            this.controlPanel.Controls.Add(this.btnLoad);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlPanel.ForeColor = System.Drawing.Color.White;
            this.controlPanel.Location = new System.Drawing.Point(0, 574);
            this.controlPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(1283, 80);
            this.controlPanel.TabIndex = 0;
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(280, 10);
            this.btnNext.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(40, 30);
            this.btnNext.TabIndex = 10;
            this.btnNext.Text = "→";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevious.Location = new System.Drawing.Point(96, 10);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(40, 30);
            this.btnPrevious.TabIndex = 9;
            this.btnPrevious.Text = "←";
            this.btnPrevious.UseVisualStyleBackColor = false;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // lblTotalTime
            // 
            this.lblTotalTime.AutoSize = true;
            this.lblTotalTime.Location = new System.Drawing.Point(700, 50);
            this.lblTotalTime.Name = "lblTotalTime";
            this.lblTotalTime.Size = new System.Drawing.Size(38, 16);
            this.lblTotalTime.TabIndex = 8;
            this.lblTotalTime.Text = "00:00";
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Location = new System.Drawing.Point(620, 50);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(38, 16);
            this.lblCurrentTime.TabIndex = 1;
            this.lblCurrentTime.Text = "00:00";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.ForeColor = System.Drawing.Color.LightGray;
            this.lblFileName.Location = new System.Drawing.Point(639, 15);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(150, 16);
            this.lblFileName.TabIndex = 7;
            this.lblFileName.Text = "Ningún archivo cargado";
            // 
            // positionSlider
            // 
            this.positionSlider.Location = new System.Drawing.Point(11, 50);
            this.positionSlider.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.positionSlider.Maximum = 100;
            this.positionSlider.Name = "positionSlider";
            this.positionSlider.Size = new System.Drawing.Size(580, 56);
            this.positionSlider.TabIndex = 6;
            this.positionSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.positionSlider.Value = 50;
            this.positionSlider.Scroll += new System.EventHandler(this.positionSlider_Scroll);
            this.positionSlider.ValueChanged += new System.EventHandler(this.positionSlider_ValueChanged_1);
            // 
            // cmbVisualizationType
            // 
            this.cmbVisualizationType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.cmbVisualizationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVisualizationType.ForeColor = System.Drawing.Color.White;
            this.cmbVisualizationType.FormattingEnabled = true;
            this.cmbVisualizationType.Location = new System.Drawing.Point(489, 15);
            this.cmbVisualizationType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbVisualizationType.Name = "cmbVisualizationType";
            this.cmbVisualizationType.Size = new System.Drawing.Size(121, 24);
            this.cmbVisualizationType.TabIndex = 5;
            this.cmbVisualizationType.SelectedIndexChanged += new System.EventHandler(this.cmbVisualizationType_SelectedIndexChanged_1);
            // 
            // volumenSlider
            // 
            this.volumenSlider.Location = new System.Drawing.Point(319, 10);
            this.volumenSlider.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.volumenSlider.Maximum = 100;
            this.volumenSlider.Name = "volumenSlider";
            this.volumenSlider.Size = new System.Drawing.Size(149, 56);
            this.volumenSlider.TabIndex = 4;
            this.volumenSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.volumenSlider.Value = 50;
            this.volumenSlider.ValueChanged += new System.EventHandler(this.volumenSlider_ValueChanged_1);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(235, 10);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(40, 30);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "⏹";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPause.Location = new System.Drawing.Point(188, 10);
            this.btnPause.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(40, 30);
            this.btnPause.TabIndex = 2;
            this.btnPause.Text = "⏸";
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay.Location = new System.Drawing.Point(141, 10);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(40, 30);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "▶";
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Location = new System.Drawing.Point(11, 10);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 30);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Cargar";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // playlistPanel
            // 
            this.playlistPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.playlistPanel.Controls.Add(this.playlistBox);
            this.playlistPanel.Controls.Add(this.btnClearPlaylist);
            this.playlistPanel.Controls.Add(this.btnAddFiles);
            this.playlistPanel.Controls.Add(this.playlistTitle);
            this.playlistPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.playlistPanel.Location = new System.Drawing.Point(983, 0);
            this.playlistPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.playlistPanel.Name = "playlistPanel";
            this.playlistPanel.Size = new System.Drawing.Size(300, 574);
            this.playlistPanel.TabIndex = 1;
            // 
            // playlistBox
            // 
            this.playlistBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.playlistBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.playlistBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.playlistBox.ForeColor = System.Drawing.Color.White;
            this.playlistBox.FormattingEnabled = true;
            this.playlistBox.Location = new System.Drawing.Point(11, 75);
            this.playlistBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.playlistBox.Name = "playlistBox";
            this.playlistBox.Size = new System.Drawing.Size(279, 470);
            this.playlistBox.TabIndex = 3;
            this.playlistBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.playlistBox_DrawItem);
            this.playlistBox.DoubleClick += new System.EventHandler(this.playlistBox_DoubleClick);
            this.playlistBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.playlistBox_KeyDown);
            // 
            // btnClearPlaylist
            // 
            this.btnClearPlaylist.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnClearPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearPlaylist.Location = new System.Drawing.Point(100, 39);
            this.btnClearPlaylist.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClearPlaylist.Name = "btnClearPlaylist";
            this.btnClearPlaylist.Size = new System.Drawing.Size(75, 30);
            this.btnClearPlaylist.TabIndex = 2;
            this.btnClearPlaylist.Text = "Limpiar";
            this.btnClearPlaylist.UseVisualStyleBackColor = false;
            this.btnClearPlaylist.Click += new System.EventHandler(this.btnClearPlaylist_Click);
            // 
            // btnAddFiles
            // 
            this.btnAddFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnAddFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddFiles.Location = new System.Drawing.Point(11, 39);
            this.btnAddFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(75, 30);
            this.btnAddFiles.TabIndex = 1;
            this.btnAddFiles.Text = "Agregar";
            this.btnAddFiles.UseVisualStyleBackColor = false;
            this.btnAddFiles.Click += new System.EventHandler(this.btnAddFiles_Click);
            // 
            // playlistTitle
            // 
            this.playlistTitle.AutoSize = true;
            this.playlistTitle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playlistTitle.ForeColor = System.Drawing.Color.White;
            this.playlistTitle.Location = new System.Drawing.Point(11, 10);
            this.playlistTitle.Name = "playlistTitle";
            this.playlistTitle.Size = new System.Drawing.Size(187, 23);
            this.playlistTitle.TabIndex = 0;
            this.playlistTitle.Text = "Lista de Reproducción";
            // 
            // FrmIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1283, 654);
            this.Controls.Add(this.playlistPanel);
            this.Controls.Add(this.controlPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmIndex";
            this.Text = "Music Player";
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.volumenSlider)).EndInit();
            this.playlistPanel.ResumeLayout(false);
            this.playlistPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.ComboBox cmbVisualizationType;
        private System.Windows.Forms.TrackBar volumenSlider;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Label lblTotalTime;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TrackBar positionSlider;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Panel playlistPanel;
        private System.Windows.Forms.Button btnClearPlaylist;
        private System.Windows.Forms.Button btnAddFiles;
        private System.Windows.Forms.Label playlistTitle;
        private System.Windows.Forms.ListBox playlistBox;
    }
}