using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ServerInfo.Tests
{
    public static class DynamicProperty
    {
        public static void ChangeProperty(this object o, string propertyName, object newValue)
        {
            PropertyInfo pi;
                
            try 
            { 
                pi = o.GetType().GetProperty(propertyName); 
            }
            catch (Exception ex)
            { 
                throw new Exception("No Property " + propertyName + " in " + o.GetType().ToString(), ex); 
            }
            
            pi.SetValue(o, newValue, null);
        }
    }
}
