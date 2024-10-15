using LeonDrace.Utility.Helpers;
using NUnit.Framework;
using System.Collections.Generic;

namespace LeonDrace.Utility.Tests
{
	public class RuntimeAssemblyTests
	{
		[TestCase("LeonDrace.Utility.Helpers")]
		[TestCase("LeonDrace.Utility.Extensions")]
		[TestCase("LeonDrace.Utility.Singletons")]
		public void Search_Valid_Namespace(string @namespace)
		{
			bool found = AssemblyHelper.NamespaceExists(@namespace);
			Assert.IsTrue(found);
		}

		[TestCase("Assembly-CSharp1")]
		public void Search_Invalid_Namespace(string @namespace)
		{
			bool found = AssemblyHelper.NamespaceExists(@namespace);
			Assert.IsFalse(found);
		}

		[TestCase("LeonDrace.Utility.Tests.Runtime")]
		public void Get_Derived_Classes_In_Domain(string assembly)
		{
			List<System.Type> foundTypes = AssemblyHelper.GetTypes<BaseClassTest>(assembly);

			Assert.That(foundTypes.Count, Is.EqualTo(2));
		}

		[Test]
		public void Get_Derived_Classes_In_All_Domains()
		{
			List<System.Type> foundTypes = AssemblyHelper.GetTypes<BaseClassTest>();

			Assert.That(foundTypes.Count, Is.EqualTo(2));
		}

		[TestCase("LeonDrace.Utility.Tests.Runtime")]
		public void Get_Interface_In_Domain(string assembly)
		{
			List<System.Type> foundTypes = AssemblyHelper.GetTypes<ITest>(assembly);

			Assert.That(foundTypes.Count, Is.EqualTo(1));
		}

		[Test]
		public void Get_Interface_In_All_Domains()
		{
			List<System.Type> foundTypes = AssemblyHelper.GetTypes<ITest>();

			Assert.That(foundTypes.Count, Is.EqualTo(1));
		}

		public abstract class BaseClassTest
		{
		}

		public class BaseClass1 : BaseClassTest, ITest
		{
		}

		public class BaseClass2 : BaseClassTest
		{
		}

		public interface ITest
		{

		}
	}
}
