using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LeonDrace.Utility.Helpers
{
	public static class AssemblyHelper
	{
		/// <summary>
		/// Check if a namespace exists.
		/// </summary>
		/// <param name="namespace"></param>
		/// <returns></returns>
		public static bool NamespaceExists(string @namespace)
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (type.Namespace == @namespace)
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Get all derived types.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static List<Type> GetTypes<T>()
		{
			return GetTypes(typeof(T));
		}

		/// <summary>
		/// Get all derived type in the given assembly.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="assemblyName"></param>
		/// <returns></returns>
		public static List<Type> GetTypes<T>(string assemblyName)
		{
			return GetTypesInAssembly(assemblyName, typeof(T));
		}

		/// <summary>
		/// Get all derived types.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static List<Type> GetTypes(Type searchedType)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			List<Type> filteredTypes = new List<Type>();

			for (int i = 0; i < assemblies.Length; i++)
			{
				AddTypesFromAssembly(assemblies[i].GetTypes(), searchedType, filteredTypes);
			}

			return filteredTypes;
		}

		/// <summary>
		/// Get all derived type in the given assembly.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static List<Type> GetTypesInAssembly(string assemblyName, Type searchedType)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			List<Type> filteredTypes = new List<Type>();

			for (int i = 0; i < assemblies.Length; i++)
			{
				if (assemblies[i].GetName().Name == assemblyName)
				{
					AddTypesFromAssembly(assemblies[i].GetTypes(), searchedType, filteredTypes);
					break;
				}
			}

			return filteredTypes;
		}

		/// <summary>
		/// Get types with the option to exclude compiler generated ones.
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <param name="excludeCompilerGenerated"></param>
		/// <returns></returns>
		public static Type[] GetTypesInAssembly(string assemblyName, bool excludeCompilerGenerated = true)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			for (int i = 0; i < assemblies.Length; i++)
			{
				if (assemblies[i].GetName().Name == assemblyName)
				{
					System.Type[] types = assemblies[i].GetTypes();
					if (excludeCompilerGenerated)
					{
						return types.Where(x => x.GetCustomAttribute<CompilerGeneratedAttribute>() == null).ToArray();
					}

					return types;
				}
			}

#if UNITY_EDITOR
			UnityEngine.Debug.Log($"Assembly {assemblyName} No Types Found");
#endif

			return new Type[0];
		}


		private static void AddTypesFromAssembly(Type[] assemblyTypes, Type searchedType, ICollection<Type> results)
		{
			if (assemblyTypes == null) return;

			for (int i = 0; i < assemblyTypes.Length; i++)
			{
				Type type = assemblyTypes[i];
				if (type != searchedType && searchedType.IsAssignableFrom(type))
				{
					results.Add(type);
				}
			}
		}
	}
}
