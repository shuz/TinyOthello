namespace TinyOthello.GraphicUI {
    partial class MainWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.boardPanel = new System.Windows.Forms.Panel();
            this.btnPass = new System.Windows.Forms.Button();
            this.btnNewGame = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.blackIsAI = new System.Windows.Forms.CheckBox();
            this.whiteIsAI = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblWhite = new System.Windows.Forms.Label();
            this.lblBlack = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.picCurrent = new System.Windows.Forms.PictureBox();
            this.btnRedo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picCurrent)).BeginInit();
            this.SuspendLayout();
            // 
            // boardPanel
            // 
            this.boardPanel.AutoSize = true;
            this.boardPanel.Location = new System.Drawing.Point(12, 12);
            this.boardPanel.Name = "boardPanel";
            this.boardPanel.Size = new System.Drawing.Size(345, 337);
            this.boardPanel.TabIndex = 2;
            // 
            // btnPass
            // 
            this.btnPass.Enabled = false;
            this.btnPass.Location = new System.Drawing.Point(399, 12);
            this.btnPass.Name = "btnPass";
            this.btnPass.Size = new System.Drawing.Size(82, 30);
            this.btnPass.TabIndex = 3;
            this.btnPass.Text = "Pass";
            this.btnPass.UseVisualStyleBackColor = true;
            this.btnPass.Click += new System.EventHandler(this.btnPass_Click);
            // 
            // btnNewGame
            // 
            this.btnNewGame.Location = new System.Drawing.Point(399, 48);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(82, 30);
            this.btnNewGame.TabIndex = 4;
            this.btnNewGame.Text = "New Game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(398, 130);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(82, 32);
            this.btnUndo.TabIndex = 5;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // blackIsAI
            // 
            this.blackIsAI.AutoSize = true;
            this.blackIsAI.Location = new System.Drawing.Point(399, 84);
            this.blackIsAI.Name = "blackIsAI";
            this.blackIsAI.Size = new System.Drawing.Size(82, 17);
            this.blackIsAI.TabIndex = 7;
            this.blackIsAI.Text = "Black is AI?";
            this.blackIsAI.UseVisualStyleBackColor = true;
            this.blackIsAI.CheckedChanged += new System.EventHandler(this.blackIsAI_CheckedChanged);
            // 
            // whiteIsAI
            // 
            this.whiteIsAI.AutoSize = true;
            this.whiteIsAI.Checked = true;
            this.whiteIsAI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.whiteIsAI.Location = new System.Drawing.Point(399, 107);
            this.whiteIsAI.Name = "whiteIsAI";
            this.whiteIsAI.Size = new System.Drawing.Size(83, 17);
            this.whiteIsAI.TabIndex = 8;
            this.whiteIsAI.Text = "White is AI?";
            this.whiteIsAI.UseVisualStyleBackColor = true;
            this.whiteIsAI.CheckedChanged += new System.EventHandler(this.whiteIsAI_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(395, 330);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "White Score";
            // 
            // lblWhite
            // 
            this.lblWhite.AutoSize = true;
            this.lblWhite.Location = new System.Drawing.Point(396, 353);
            this.lblWhite.Name = "lblWhite";
            this.lblWhite.Size = new System.Drawing.Size(0, 13);
            this.lblWhite.TabIndex = 10;
            // 
            // lblBlack
            // 
            this.lblBlack.AutoSize = true;
            this.lblBlack.Location = new System.Drawing.Point(396, 308);
            this.lblBlack.Name = "lblBlack";
            this.lblBlack.Size = new System.Drawing.Size(0, 13);
            this.lblBlack.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(396, 282);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Black Score";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(396, 214);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Current Player";
            // 
            // picCurrent
            // 
            this.picCurrent.Location = new System.Drawing.Point(399, 230);
            this.picCurrent.Name = "picCurrent";
            this.picCurrent.Size = new System.Drawing.Size(40, 40);
            this.picCurrent.TabIndex = 14;
            this.picCurrent.TabStop = false;
            // 
            // btnRedo
            // 
            this.btnRedo.Location = new System.Drawing.Point(398, 168);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(82, 32);
            this.btnRedo.TabIndex = 15;
            this.btnRedo.Text = "Redo";
            this.btnRedo.UseVisualStyleBackColor = true;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 379);
            this.Controls.Add(this.btnRedo);
            this.Controls.Add(this.picCurrent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblBlack);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblWhite);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.whiteIsAI);
            this.Controls.Add(this.blackIsAI);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.btnNewGame);
            this.Controls.Add(this.btnPass);
            this.Controls.Add(this.boardPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "TinyOthello";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picCurrent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel boardPanel;
        private System.Windows.Forms.Button btnPass;
        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.CheckBox blackIsAI;
        private System.Windows.Forms.CheckBox whiteIsAI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblWhite;
        private System.Windows.Forms.Label lblBlack;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picCurrent;
        private System.Windows.Forms.Button btnRedo;

    }
}