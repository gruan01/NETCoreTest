namespace Common {

    public static class Settings {

        private static string ContentRootPath;

        private static SettingProvider _provider = null;

        public static SettingProvider Provider {
            get {
                if (_provider == null)
                    _provider = new JsonSettingProvider(ContentRootPath);
                return _provider;
            }
            set {
                _provider = value;
            }
        }

        internal static void Init(string contentRootPath) {
            ContentRootPath = contentRootPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T Get<T>(string fileName = null) where T : class {
            return Provider.Get<T>(fileName);
        }
    }
}
