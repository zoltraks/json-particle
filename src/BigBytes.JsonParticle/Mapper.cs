using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace BigBytes.JsonParticle
{
    /// <summary>
    /// Generic class for simple object mapper using serialization.
    /// <br /><br />
    /// Create mapper for destination class you want to map other objects to.
    /// <br /><br />
    /// For proper mapping both classes have to contian Serialize and Deserialize methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Mapper<T>
    {
        private readonly MethodInfo _Deserialize;

        private readonly Dictionary<Type, MethodInfo> _Serialize = new Dictionary<Type, MethodInfo>();

        /// <summary>
        /// Create mapper for destination class you want to map other objects to.
        /// <br /><br />
        /// For proper mapping both classes have to contian Serialize and Deserialize methods.
        /// </summary>
        public Mapper()
        {
            var type = typeof(T);
            MethodInfo method;
            var binding = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            while (true)
            {
                method = type.GetMethod("Deserialize", binding);
                if (null != method)
                {
                    break;
                }
                if (null == type.BaseType)
                {
                    break;
                }
                type = type.BaseType;
            }
            _Deserialize = method;
        }

        /// <summary>
        /// Create new object instance of destination type from source object 
        /// which implements "Serialize" method.
        /// <br /><br />
        /// Null value will be returned if source object could not be serialized 
        /// or destination object can't be deserialized.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public T Map(object o)
        {
            if (null == o)
            {
                return default(T);
            }
            if (null == _Deserialize)
            {
                return default(T);
            }
            var type = o.GetType();

            MethodInfo method;

            if (_Serialize.ContainsKey(type))
            {
                method = _Serialize[type];
            }
            else
            {
                var binding = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
                while (true)
                {
                    method = type.GetMethod("Serialize", binding);
                    if (null != method)
                    {
                        break;
                    }
                    if (null == type.BaseType)
                    {
                        break;
                    }
                    type = type.BaseType;
                }
                _Serialize[type] = method;
            }

            if (null == method)
            {
                return default(T);
            }

            var json = method.Invoke(null, new object[] { o }) as string;

            T r = default(T);

            try
            {
                r = (T)_Deserialize.Invoke(null, new object[] { json });
            }
            catch (InvalidCastException)
            {
                Debug.WriteLine($"{Utility.Now()} Error mapping to {typeof(T).Name} from {o.GetType().Name}");
            }
            return r;
        }
    }
}
