namespace ijw.Client.Winform.Controls {
    partial class Unity3DPlayer {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Unity3DPlayer));
            this.axUnityWebPlayer = new AxUnityWebPlayerAXLib.AxUnityWebPlayer();
            this._uplayer = new AxUnityWebPlayerAXLib.AxUnityWebPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.axUnityWebPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._uplayer)).BeginInit();
            this.SuspendLayout();
            // 
            // axUnityWebPlayer
            // 
            this.axUnityWebPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axUnityWebPlayer.Enabled = true;
            this.axUnityWebPlayer.Location = new System.Drawing.Point(0, 0);
            this.axUnityWebPlayer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.axUnityWebPlayer.Name = "axUnityWebPlayer";
            this.axUnityWebPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axUnityWebPlayer.OcxState")));
            this.axUnityWebPlayer.Size = new System.Drawing.Size(596, 432);
            this.axUnityWebPlayer.TabIndex = 0;
            // 
            // _uplayer
            // 
            this._uplayer.Enabled = true;
            this._uplayer.Location = new System.Drawing.Point(485, 332);
            this._uplayer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._uplayer.Name = "_uplayer";
            this._uplayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("_uplayer.OcxState")));
            this._uplayer.Size = new System.Drawing.Size(88, 96);
            this._uplayer.TabIndex = 1;
            this._uplayer.Visible = false;
            // 
            // Unity3DPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._uplayer);
            this.Controls.Add(this.axUnityWebPlayer);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Unity3DPlayer";
            this.Size = new System.Drawing.Size(596, 432);
            ((System.ComponentModel.ISupportInitialize)(this.axUnityWebPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._uplayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxUnityWebPlayerAXLib.AxUnityWebPlayer axUnityWebPlayer;
        private AxUnityWebPlayerAXLib.AxUnityWebPlayer _uplayer;
    }
}
