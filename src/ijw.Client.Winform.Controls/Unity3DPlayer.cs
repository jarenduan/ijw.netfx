using ijw.Client.Win32;
using ijw.Contract;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ijw.Client.Winform.Controls {
    public partial class Unity3DPlayer : UserControl {

        public Unity3DPlayer() {
            InitializeComponent();
            this.axUnityWebPlayer.Size = this._ControlSize;
        }

        /// <summary>
        /// 是否隐藏右键菜单，默认显示
        /// </summary>
        public bool DisableContextMenu { get; set; }

        public AxUnityWebPlayerAXLib.AxUnityWebPlayer AxUnityWebPlayer
        {
            get
            {
                return this.axUnityWebPlayer;
            }
        }
        /// <su
        /// mmary>
        /// 装载指定.unity3d场景包
        /// </summary>
        /// <param name="path">.unity3d文件路径，必须是绝对路径</param>
        public void LoadUnity3DPackage(string path) {
            /*
           * 给unity设置src属性时，会自动生成字符串资源，并把它赋值给属性OcxState。
           * 由于没办法手动生成这个字符串，因而需要通过代码，即先赋值给OcxState，再取出来的方式得到需要的字符串资源。
           * 然后再将值赋给重新创建的控件。
           */
            try {
                path.ShouldSatisfy(p => p.Length > 3 && p[1] == ':' && p[2] == '\\');
                path.ShouldExistSuchFile();
                this._ControlSize = this.axUnityWebPlayer.Size;

                //path 必须是绝对路径，否则两次运行之后，当前工作目录会变化，导致错误
                this._uplayer.src = path;
                ((System.ComponentModel.ISupportInitialize)(this.axUnityWebPlayer)).BeginInit();
                this.Controls.Remove(axUnityWebPlayer);
                ((System.ComponentModel.ISupportInitialize)(this.axUnityWebPlayer)).EndInit();
#if DEBUG
                this.axUnityWebPlayer.Dispose();
#endif
                this.axUnityWebPlayer = new AxUnityWebPlayerAXLib.AxUnityWebPlayer();
                ((System.ComponentModel.ISupportInitialize)(this.axUnityWebPlayer)).BeginInit();
                this.SuspendLayout();
                this.axUnityWebPlayer.Dock = DockStyle.Fill;
                this.axUnityWebPlayer.Enabled = true;
                this.axUnityWebPlayer.Location = new Point(0, 0);
                this.axUnityWebPlayer.Name = "axUnityWebPlayer";
                this.axUnityWebPlayer.OcxState = _uplayer.OcxState;
                this.axUnityWebPlayer.Size = this._ControlSize;
                this.axUnityWebPlayer.TabIndex = 0;
                this.Controls.Add(this.axUnityWebPlayer);
                ((System.ComponentModel.ISupportInitialize)(this.axUnityWebPlayer)).EndInit();
                this.ResumeLayout(false);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            switch (m.Msg) {
                case (int)Win32Message.WM_PARENTNOTIFY:
                    if (DisableContextMenu) {
                        if (!_isContextMenuHided && m.ToString().Contains("WM_RBUTTONDOWN")) {
                            HideContextMenu(m);
                        }
                    }
                    else if (_isContextMenuHided)
                        RestoreContextMenu(m);
                    break;
            }
        }

        private async void RestoreContextMenu(Message m) {
            IntPtr handle = await GetUnityContextMenuHandle();
            //存储当前鼠标的位置
            Rect size;
            Win32Window.GetWindowRect(handle, out size);
            //还原窗口大小
            Win32Window.MoveWindow(handle, size.Left, size.Top, this._contextMenuOriginalSize.Right - this._contextMenuOriginalSize.Left, this._contextMenuOriginalSize.Bottom - this._contextMenuOriginalSize.Top, true);
            //更新状态
            _isContextMenuHided = false;
        }

        private async void HideContextMenu(Message m) {
            IntPtr handle = await GetUnityContextMenuHandle();
            //存储右键菜单大小
            Win32Window.GetWindowRect(handle, out this._contextMenuOriginalSize);
            //把右键菜单移到左上角并设置其大小为0
            Win32Window.MoveWindow(handle, 0, 0, 0, 0, true);
            _isContextMenuHided = true;
        }

        /// <summary>
        /// 异步获取UnityWebPlayer的ContextMenu窗口的句柄
        /// </summary>
        /// <returns>UnityWebPlayer的ContextMenu窗口的句柄</returns>
        private async Task<IntPtr> GetUnityContextMenuHandle() {
            var handle = await Task.Run(() => {
                IntPtr contextMenuHandle = FindUnityContextMenuHandle();
                while (contextMenuHandle == IntPtr.Zero) {
                    contextMenuHandle = FindUnityContextMenuHandle();
                }
                return contextMenuHandle;
            });
            return handle;
        }

        private IntPtr FindUnityContextMenuHandle() {
            IntPtr contextMenuHandle = Win32Window.FindWindow(UNITY_CONTEXT_MENU_TITLE, null);
            return contextMenuHandle;
        }

        //Unity.ContextSubmenu 为右键快键菜单的窗口id
        private const string UNITY_CONTEXT_MENU_TITLE = "Unity.ContextSubmenu";
        private Rect _contextMenuOriginalSize;
        private bool _isContextMenuHided;
        private Size _ControlSize = new Size(401, 294);

    }
}
