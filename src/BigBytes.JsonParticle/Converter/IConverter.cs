using System;
using System.Collections.Generic;
using System.Text;

namespace BigBytes.JsonParticle.Converter
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConverter<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        T Convert(T input);
    }
}
