using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Common {

    public abstract class SettingProvider {

        protected string CfgDir {
            get;
            private set;
        }

        /// <summary>
        /// 值如 json
        /// </summary>
        protected abstract string FileType { get; }

        private ConcurrentBag<CacheItem> Cache = new ConcurrentBag<CacheItem>();

        public SettingProvider(string baseDir) {
            this.CfgDir = Path.Combine(baseDir, "configs");

            this.Watch();
        }

        protected abstract object Read(Type type, string fileName = null);

        public T Get<T>(string fileName = null) where T : class {
            var t = typeof(T);

            if (string.IsNullOrWhiteSpace(fileName)) {
                fileName = typeof(T).Name;
            }

            if (!fileName.EndsWith(this.FileType, StringComparison.OrdinalIgnoreCase)) {
                fileName = string.Format("{0}.{1}", fileName, this.FileType);
            }

            var item = this.Cache.FirstOrDefault(c => c.Type.Equals(t) && c.FileName.Equals(fileName));
            if (item != null)
                return (T)item.Value;
            else {
                var o = (T)this.Read(t, fileName);
                this.Cache.Add(new CacheItem() {
                    Type = t,
                    FileName = fileName,
                    Value = o
                });
                return o;
            }
        }


        private void Change(WatcherChangeTypes ct, string file) {
            var item = this.Cache.FirstOrDefault(c => c.FileName.Equals(file, StringComparison.OrdinalIgnoreCase));
            if (item == null)
                return;

            switch (ct) {
                case WatcherChangeTypes.Changed:
                case WatcherChangeTypes.Created:
                case WatcherChangeTypes.Renamed:
                    var o = this.Read(item.Type, file);
                    item.Value = o;
                    break;
                case WatcherChangeTypes.Deleted:
                    item.Value = null;
                    break;
            }
        }

        protected virtual void Watch() {

            if (!Directory.Exists(this.CfgDir))
                Directory.CreateDirectory(this.CfgDir);

            var watcher = new FileSystemWatcher(this.CfgDir, "*." + this.FileType);
            watcher.Changed += Watcher_Changed;
            watcher.Created += Watcher_Created;
            watcher.Deleted += Watcher_Deleted;
            watcher.Renamed += Watcher_Renamed;
            watcher.Error += Watcher_Error;
            watcher.EnableRaisingEvents = true;
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e) {
            this.Change(e.ChangeType, e.Name);
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e) {
            this.Change(e.ChangeType, e.Name);
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e) {
            this.Change(e.ChangeType, e.Name);
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e) {
            this.Change(e.ChangeType, e.Name);
        }

        private void Watcher_Error(object sender, ErrorEventArgs e) {

        }


        private class CacheItem {

            public Type Type { get; set; }

            public string FileName { get; set; }

            public object Value { get; set; }
        }
    }

}
