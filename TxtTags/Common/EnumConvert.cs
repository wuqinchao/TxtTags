using System;
using System.ComponentModel;
using System.Linq;

namespace TxtTags.Common
{
    public class EnumConvert<T>
    {
        /// <summary>
        /// 根据字符串返回指定枚举类型相匹配的枚举值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static T GetValue(string text)
        {
            return (T)Enum.Parse(typeof(T), text);
        }
        /// <summary>
        /// 根据枚举值返回枚举值的注释
        /// </summary>
        /// <param name="v">枚举值</param>
        /// <returns>枚举值的注释</returns>
        public static string GetDescription(T v)
        {
            var type = typeof(T).GetField(v.ToString());
            if(type == null)
            {
                return v.ToString();
            }
            var descriptionAttribute = type
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return descriptionAttribute != null ? descriptionAttribute.Description : v.ToString();
        }
    }
}
