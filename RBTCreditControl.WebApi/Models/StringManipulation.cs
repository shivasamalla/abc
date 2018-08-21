using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RBTCreditControl.WebApp.Models
{
    public static class StringManipulation<T>
    {
        public static void Trim(T obj)
        {
            var stringProperties = obj.GetType().GetProperties()
                          .Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(obj, null);
                if (currentValue!=null)
                  stringProperty.SetValue(obj, currentValue.Trim(), null);
            }
        }
    }
}
