using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ijw.IO.FileSystem.Watcher {
    public class SingleFileChangeMonitor {
        private FileSystemWatcher _watcher = new FileSystemWatcher();
        public event Action Changed;
        public FileMonitorMultipleInvokingOption MultipleInvokingOption { get; set; }

        public void StartMonitoring(string fullFileName) {
            FileInfo fi = null;
            try {
                fi = new FileInfo(fullFileName);
            }
            catch {
                throw new Exception("Invaild file path or name. Or cannot access.");
            }
            StartMonitoring(fi);
        }

        public void StartMonitoring(FileInfo fileInfo) {
            this._watcher = new FileSystemWatcher();
            this._watcher.EnableRaisingEvents = false;
            this._watcher.Filter = fileInfo.Name;
            this._watcher.Path = fileInfo.DirectoryName;
            this._watcher.NotifyFilter = NotifyFilters.LastWrite;
            this._watcher.Changed += _watcher_Changed;
            this._watcher.EnableRaisingEvents = true;
        }

        public void StopMonitoring() {
            this._watcher.EnableRaisingEvents = false;
        }

        void _watcher_Changed(object sender, FileSystemEventArgs e) {
            if (this.Changed == null) return;
            switch (this.MultipleInvokingOption) {
                case FileMonitorMultipleInvokingOption.DoNothing:
                    this.Changed();
                    break;
                case FileMonitorMultipleInvokingOption.MergeDoubleEvent:
                    doubleEvent();
                    break;
                case FileMonitorMultipleInvokingOption.OffAndOn:
                    offAndOn(e);
                    break;
                default:
                    break;
            }
        }

        private int times = 0;
        private void doubleEvent() {
            times++;
            if (times == 2) {
                this.times = 0;
                this.Changed();
            }
        }

        private void offAndOn(FileSystemEventArgs e) {
            try {
                _watcher.EnableRaisingEvents = false;
                FileInfo objFileInfo = new FileInfo(e.FullPath);
                if (!objFileInfo.Exists) return;
                this.Changed();
            }
            catch{
                throw;
            }
            finally {
                _watcher.EnableRaisingEvents = true;
            }
        }
    }
}
