using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Common {
    public class JsonSettingProvider : SettingProvider {


        protected override string FileType {
            get {
                return "json";
            }
        }

        public JsonSettingProvider(string baseDir = null)
            : base(baseDir) {
        }

        protected override object Read(Type type, string fileName) {
            var path = Path.Combine(this.CfgDir, fileName);
            if (File.Exists(path)) {
                var ctx = File.ReadAllText(path);
                return JsonConvert.DeserializeObject(ctx, type);
            } else {
                return null;
            }
        }
    }
}
