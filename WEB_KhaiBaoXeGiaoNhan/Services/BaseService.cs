using log4net;
using System.Reflection;
using System.Text;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class BaseService<T> where T : new()
    {
        /// <summary>
        /// 	Logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static T _instance = default(T);

        public static T GetInstance()
        {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }

        public void DumpObject(object obj)
        {
            StringBuilder builder = new StringBuilder("");
            var dumbObj = ObjectDumper.Dump(obj, DumpStyle.CSharp);
            var type = obj.GetType().Name;
            builder.Append(string.Format("Preparing dumb {0}: ", type));
            builder.Append(dumbObj);
            _logger.Debug(builder.ToString());
        }
    }
}