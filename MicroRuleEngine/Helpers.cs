using Dynamitey;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace MicroRuleEngine
{
    public class Helpers
    {
        public static T ChangePropertyValue<T>(object data, Rule r)
        {
            PropertyInfo propertyInfo = data.GetType().GetProperty(r.MemberName);

            if (typeof(T) == typeof(XmlDocument))
            {
                Helpers.SetNodeValue(data as XmlDocument, r.MemberName, r.TargetValue.ToString());
                return (T)data;
            }
            if (propertyInfo != null)
            {
                if (propertyInfo.PropertyType == typeof(Enum) || propertyInfo.PropertyType.BaseType == typeof(Enum))
                {

                    propertyInfo.SetValue(data, Enum.Parse(propertyInfo.PropertyType, r.TargetValue.ToString()));

                }
                else
                    propertyInfo.SetValue(data, Convert.ChangeType(r.TargetValue, propertyInfo.PropertyType), null);

            }
            else
            {
                Helpers.SetJSONPropertyValue(data, r.MemberName, r.TargetValue.ToString());
            }

            return (T)data;
        }

        public string GetNodeValue(XmlDocument doc, string XPath)
        {
            if (string.IsNullOrWhiteSpace(XPath))
                return string.Empty;
            if (doc == null)
                return string.Empty;
            XPath = XPath.Replace(".", "/");
            XmlNode node = doc.SelectSingleNode("//" + XPath);
            if (node != null)
            {
                return node.InnerText;
            }
            return string.Empty;
        }

        public int GetNodeValueInt(XmlDocument doc, string XPath)
        {
            if (string.IsNullOrWhiteSpace(XPath))
                return -1;
            if (doc == null)
                return -1;
            XPath = XPath.Replace(".", "/");
            XmlNode node = doc.SelectSingleNode("//" + XPath);
            if (node != null)
            {
                int returnvalue;
                int.TryParse(node.InnerText, out returnvalue);
                return returnvalue;
            }
            return -1;
        }

        public decimal GetNodeValueDecimal(XmlDocument doc, string XPath)
        {
            if (string.IsNullOrWhiteSpace(XPath))
                return -1;
            if (doc == null)
                return -1;
            XPath = XPath.Replace(".", "/");
            XmlNode node = doc.SelectSingleNode("//" + XPath);
            if (node != null)
            {
                decimal returnvalue;
                decimal.TryParse(node.InnerText, out returnvalue);
                return returnvalue;
            }
            return -1;
        }

        public string GetJSONPropertyValue(object data, string PropertyValue)
        {
            if (string.IsNullOrWhiteSpace(PropertyValue))
                return string.Empty;
            if (data == null)
                return string.Empty;

            return Dynamic.InvokeGetChain(data, PropertyValue);

        }

        public int GetJSONPropertyInt(object data, string PropertyValue)
        {
            if (string.IsNullOrWhiteSpace(PropertyValue))
                return -1;
            if (data == null)
                return -1;
            object result = Dynamic.InvokeGetChain(data, PropertyValue);
            int returnvalue;
            int.TryParse(result.ToString(), out returnvalue);

            return returnvalue;

        }


        public static void SetJSONPropertyValue(object data, string PropertyName,string PropertyValue)
        {
             Dynamic.InvokeSetChain(data, PropertyName, PropertyValue);

        }


        public static void SetNodeValue(XmlDocument doc, string XPath,string NodeValue)
        {
            XPath = XPath.Replace(".", "/");
            XmlNode node = doc.SelectSingleNode("//" + XPath);
            if (node != null)
            {
                node.InnerXml= NodeValue;
            }
           // return "";
        }
    }
}
