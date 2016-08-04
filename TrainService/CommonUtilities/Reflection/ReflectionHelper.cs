using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommonUtilities
{
    public static class ReflectionHelper
    {
        public static Assembly LoadAssembly(string path)
        {
            return Assembly.LoadFrom(path);
        }

        #region For Type
        public static List<Type> GetTypes(this IEnumerable<Assembly> assemblies)
        {
            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    types.AddRange(assembly.GetTypes());
                }
                catch (Exception)
                {
                }
            }

            return types;
        }

        public static List<MethodInfo> GetAllMethods(
       this IEnumerable<Type> types)
        {
            var methodInfos = new List<MethodInfo>();
            foreach (var type in types)
            {
                try
                {
                    methodInfos.AddRange(type.GetMethods());
                }
                catch (Exception)
                {
                }
            }

            return methodInfos;
        }

        public static IEnumerable<Type> WhichHasAttr(
            this IEnumerable<Type> types, string attrFullName)
        {
            return types.Where(
                             t =>
                             (Attribute.GetCustomAttributes(t)
                                       .Where(a => (a.TypeId.ToString() == attrFullName))
                                       .ToArray()
                                       .Length > 0));


        }

        public static IEnumerable<Type> WhichNotHasAttr(
            this IEnumerable<Type> types, string attrFullName)
        {
            return types.Where(
                             t =>
                             (Attribute.GetCustomAttributes(t)
                                       .Where(a => (a.TypeId.ToString() == attrFullName))
                                       .ToArray()
                                       .Length == 0));
        }

        public static IEnumerable<Type> WhichNamespaceStartsWith(
           this IEnumerable<Type> types, string nameSpace)
        {
            return types.Where(t => t.Namespace.StartsWith(nameSpace));
        }

        public static IEnumerable<Type> WhichNamespaceStartsWithOneOf(
           this IEnumerable<Type> types, IEnumerable<string> nameSpaces)
        {
            return types.Where(t => t.Namespace.StartsWithOneOf(nameSpaces));
        }

        public static IEnumerable<Type> WhichNamespaceNotStartsWith(
           this IEnumerable<Type> types, string nameSpace)
        {
            return types.Where(t => !t.Namespace.StartsWith(nameSpace));
        }

        public static IEnumerable<Type> WhichNamespaceNotStartsWithOneOf(
           this IEnumerable<Type> types, IEnumerable<string> nameSpaces)
        {
            return types.Where(t => !t.Namespace.StartsWithOneOf(nameSpaces));
        }

        public static IEnumerable<Type> WhichFullNameStarstWith(
           this IEnumerable<Type> types, string name)
        {
            return types.Where(t => t.FullName.StartsWith(name));
        }

        public static IEnumerable<Type> WhichFullNameStarstWithOneOf(
           this IEnumerable<Type> types, IEnumerable<string> names)
        {
            return types.Where(t => t.FullName.StartsWithOneOf(names));
        }

        public static IEnumerable<Type> WhichHasAttr(
            this IEnumerable<Type> types, string attrFullName, object value)
        {
            return types.Where(type =>
                             CustomAttributeData.GetCustomAttributes(type)
                             .Any(attr => attr.Constructor.DeclaringType != null &&
                                          attrFullName.Equals(attr.Constructor.DeclaringType.FullName) &&
                                          attr.ConstructorArguments.Count > 0 &&
                                          attr.ConstructorArguments[0].Value.Equals(value)));
        }

        public static IEnumerable<Type> WhichHasAttr(
            this IEnumerable<Type> types, string attrFullName, object value1, object value2)
        {
            return types.Where(type =>
                             CustomAttributeData.GetCustomAttributes(type)
                             .Any(attr => attr.Constructor.DeclaringType != null &&
                                          attrFullName.Equals(attr.Constructor.DeclaringType.FullName) &&
                                          attr.ConstructorArguments.Count > 1 &&
                                          attr.ConstructorArguments[0].Value.Equals(value1) &&
                                          attr.ConstructorArguments[1].Value.Equals(value2)));
        }

        public static T CreateInstance<T>(this Type type, params object[] args) where T : class
        {
            return Activator.CreateInstance(type, args) as T;
        }

        #endregion


        #region For Method

        public static string GetFullName(this MethodInfo method)
        {
            var className = method.DeclaringType.FullName;

            return "{0}.{1}".FormatedWith(className, method.Name);
        }

        public static IEnumerable<MethodInfo> GetAllTestMethods(this Assembly assembly)
        {
            return assembly.GetTypes().WhichHasAttr(AttributeFullNames.TestClass)
                  .GetAllMethods().WhichHasAttr(AttributeFullNames.TestMethod).SelectUnstatic();
        }

        public static IEnumerable<MethodInfo> GetAllTestMethods(string testDllPath)
        {
            return LoadAssembly(testDllPath).GetAllTestMethods();
        }

        public static IEnumerable<MethodInfo> WhichHasAttr(
            this IEnumerable<MethodInfo> methodInfos, string attrFullName)
        {
            return methodInfos.Where(
                             m =>
                             (Attribute.GetCustomAttributes(m)
                                       .Where(a => (a.TypeId.ToString() == attrFullName))
                                       .ToArray()
                                       .Length > 0));
        }

        public static IEnumerable<MethodInfo> WhichNotHasAttr(
            this IEnumerable<MethodInfo> methodInfos, string attrFullName)
        {
            return methodInfos.Where(
                              m =>
                              (Attribute.GetCustomAttributes(m)
                                        .Where(a => (a.TypeId.ToString() == attrFullName))
                                        .ToArray()
                                        .Length == 0));
        }

        public static IEnumerable<MethodInfo> WhichHasAttr(
            this IEnumerable<MethodInfo> methodInfos, string attrFullName, object value)
        {
            return methodInfos.Where(m =>
                             CustomAttributeData.GetCustomAttributes(m)
                             .Any(attr => attr.Constructor.DeclaringType != null &&
                                          attrFullName.Equals(attr.Constructor.DeclaringType.FullName) &&
                                          attr.ConstructorArguments.Count == 1 &&
                                          attr.ConstructorArguments[0].Value.Equals(value)));
        }

        public static IEnumerable<MethodInfo> WhichHasAttr(
           this IEnumerable<MethodInfo> methodInfos, string attrFullName, string value, Func<object, string> toStringFunc, StrCompareRule compareRule = StrCompareRule.Equals)
        {
            return methodInfos.Where(m =>
                             CustomAttributeData.GetCustomAttributes(m)
                             .Any(attr => attr.Constructor.DeclaringType != null &&
                                          attrFullName.Equals(attr.Constructor.DeclaringType.FullName) &&
                                          attr.ConstructorArguments.Count == 1 &&
                                          toStringFunc(attr.ConstructorArguments[0].Value).IsMatch(value, compareRule)));
        }

        public static IEnumerable<MethodInfo> WhichHasAttr(
            this IEnumerable<MethodInfo> methodInfos, string attrFullName, object value1, object value2)
        {
            return methodInfos.Where(m =>
                             CustomAttributeData.GetCustomAttributes(m)
                             .Any(attr => attr.Constructor.DeclaringType != null &&
                                          attrFullName.Equals(attr.Constructor.DeclaringType.FullName) &&
                                          attr.ConstructorArguments.Count > 1 &&
                                          attr.ConstructorArguments[0].Value.Equals(value1) &&
                                          attr.ConstructorArguments[1].Value.Equals(value2)));
        }

        public static IEnumerable<MethodInfo> WhichHasAttr(
            this IEnumerable<MethodInfo> methodInfos, string attrFullName,
            string value1, Func<object, string> toStringFunc1, StrCompareRule compareRule1,
            string value2, Func<object, string> toStringFunc2, StrCompareRule compareRule2)
        {
            return methodInfos.Where(m =>
                             CustomAttributeData.GetCustomAttributes(m)
                             .Any(attr => attr.Constructor.DeclaringType != null &&
                                          attrFullName.Equals(attr.Constructor.DeclaringType.FullName) &&
                                          attr.ConstructorArguments.Count > 1 &&
                                          toStringFunc1(attr.ConstructorArguments[0].Value).IsMatch(value1, compareRule1) &&
                                          toStringFunc2(attr.ConstructorArguments[1].Value).IsMatch(value2, compareRule2)));
        }

        public static IEnumerable<MethodInfo> WhichFullNameStartsWith(
            this IEnumerable<MethodInfo> methodInfos, string name)
        {
            return methodInfos.Where(m => m.GetFullName().StartsWith(name));
        }

        public static IEnumerable<MethodInfo> WhichFullNameStartsWithOneOf(
            this IEnumerable<MethodInfo> methodInfos, IEnumerable<string> names)
        {
            return methodInfos.Where(m => m.GetFullName().StartsWithOneOf(names));
        }

        public static IEnumerable<MethodInfo> WhichFullNameNotStartsWith(
            this IEnumerable<MethodInfo> methodInfos, string name)
        {
            return methodInfos.Where(m => !m.GetFullName().StartsWith(name));
        }

        public static IEnumerable<MethodInfo> WhichFullNameNotStartsWithOneOf(
            this IEnumerable<MethodInfo> methodInfos, IEnumerable<string> names)
        {
            return methodInfos.Where(m => !m.GetFullName().StartsWithOneOf(names));
        }

        public static IEnumerable<MethodInfo> SelectStatic(
            this IEnumerable<MethodInfo> methodInfos)
        {
            return methodInfos.Where(m => m.IsStatic);
        }

        public static IEnumerable<MethodInfo> SelectUnstatic(
            this IEnumerable<MethodInfo> methodInfos)
        {
            return methodInfos.Where(m => !m.IsStatic);
        }

        public static object RunStaticMethod(Type t, string strMethod, object[] aobjParams)
        {
            var eFlags =
             BindingFlags.Static | BindingFlags.Public |
             BindingFlags.NonPublic;
            return RunMethod(t, strMethod,
             null, aobjParams, eFlags);
        }

        public static object RunInstanceMethod(Type t, string strMethod,
         object objInstance, object[] aobjParams)
        {
            var eFlags = BindingFlags.Instance | BindingFlags.Public |
             BindingFlags.NonPublic;
            return RunMethod(t, strMethod,
             objInstance, aobjParams, eFlags);
        }


        private static object RunMethod(Type t, string
         strMethod, object objInstance, object[] aobjParams, BindingFlags eFlags)
        {
            MethodInfo m;
            try
            {
                m = t.GetMethod(strMethod, eFlags);
                if (m == null)
                {
                    throw new ArgumentException("There is no method '" +
                     strMethod + "' for type '" + t + "'.");
                }

                object objRet = m.Invoke(objInstance, aobjParams);
                return objRet;
            }
            catch
            {
                throw;
            }
        }



        #endregion

        public static ListType AsList(this object obj)
        {
            var interfaces = obj.GetType().GetInterfaces();
            if (interfaces != null && interfaces.Length > 0)
            {
                foreach (var i in interfaces)
                {
                    if (i.Name.Equals("IList`1"))
                    {
                        return new ListType(obj);
                    }
                }
            }

            return null;
        }

        public static object GetFieldValue(this object obj, string fieldName, object defaultValue = null)
        {
            try
            {
                return obj.GetType().GetField(fieldName).GetValue(obj);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static object GetPropertyValue(this object obj, string fieldName, object defaultValue = null)
        {
            try
            {
                return obj.GetType().GetProperty(fieldName).GetValue(obj);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }


    public static class AttributeFullNames
    {
        public const string TestClass = "Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute";
        public const string TestMethod = "Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute";
        public const string Owner = "Microsoft.VisualStudio.TestTools.UnitTesting.OwnerAttribute";
        public const string Priority = "Microsoft.VisualStudio.TestTools.UnitTesting.PriorityAttribute";
        public const string TestProperty = "Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute";
        public const string AssemblyInitialize = "Microsoft.VisualStudio.TestTools.UnitTesting.AssemblyInitializeAttribute";
        public const string Timeout = "Microsoft.VisualStudio.TestTools.UnitTesting.TimeoutAttribute";
        public const string ClassInitialize = "Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute";
        public const string TestInitialize = "=Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute";
        public const string TestCleanup = "=Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute";
        public const string ClassCleanup = "Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute";
        public const string AssemblyCleanup = "Microsoft.VisualStudio.TestTools.UnitTesting.AssemblyCleanupAttribute";
        public const string Activity = "CommonUtilities.ActivityAttribute";
    }
}
