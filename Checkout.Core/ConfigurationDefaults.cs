using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Core
{
    public static class ConfigurationDefaults
    {
        public const string SQLSERVER_CONNECTIONSTRING = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Checkout;Integrated Security=False;Persist Security Info=False;User ID=sa;Password=1qaz2wsx";

        public const string RABBITMQ_HOST = "localhost";
        public const string RABBITMQ_EXCHANGENAME = "navigation";

        public const string COUCHBASE_URI = "couchbase://localhost";
        public const string COUCHBASE_USERNAME = "Administrator";
        public const string COUCHBASE_PASSWORD = "123456";
        public const string COUCHBASE_COLLECTIONNAME = "navigation";

        public const string NAVIGATIONAPI_HOST = "http://localhost:50225/";

    }
}
