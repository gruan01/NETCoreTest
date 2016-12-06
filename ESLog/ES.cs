using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ESLog {
    public class ES {

        private static readonly ElasticClient Client;

        static ES() {
            var node = new Uri("http://183.61.21.211:9200");
            var settings = new ConnectionSettings(node);
            Client = new ElasticClient(settings);
        }

        public static async Task Write(object o) {
            if (o == null)
                return;

            var i = o.GetType().GetTypeInfo().Assembly.GetName().Name.Replace("_", ".").ToLower();
            var t = o.GetType().FullName.Replace(".", "_").ToLower();

            var rst = await Client.IndexAsync(o, idx =>
                                                    idx.Index(i)
                                                    .Type(t)
                                                    .Id(Guid.NewGuid())
                                                    );
        }

    }
}
