using HarmonyLib;
using System;
using System.Reflection;

namespace MouseSupport.Helpers
{
    public class ReflectionHelper
    {
        public static T GetMemberValue<T>(object o, string memberName)
        {
            return Traverse.Create(o).Field(memberName).GetValue<T>();
        }

        public static void SetMemberValue(object o, string memberName, object value)
        {
            Traverse.Create(o).Field(memberName).SetValue(value);
        }

        public static bool CompareMemberValue(object o, string memberName, object value)
        {
            return value.Equals(Traverse.Create(o).Field(memberName).GetValue());
        }

        public static void InvokeMethod(object o, string methodName, params object[] args)
        {
            var method = o.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(o, args);
        }

        public static T GetMethodValue<T>(object o, string methodName, params object[] args)
        {
            var method = o.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)method.Invoke(o, args);
        }
    }
}
