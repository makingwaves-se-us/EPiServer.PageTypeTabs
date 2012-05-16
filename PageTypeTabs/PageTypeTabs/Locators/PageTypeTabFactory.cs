using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PageTypeBuilder.Reflection;

namespace PageTypeTabs.Locators
{
	public class PageTypeTabFactory
	{
		private static PageTypeTabFactory instance = new PageTypeTabFactory();

		public static PageTypeTabFactory Instance
		{
			get
			{
				return instance;
			}
		}


		public virtual List<PageTypeTab> GetDefinedTabs()
		{
			List<PageTypeTab> tabs = new List<PageTypeTab>();
			List<Type> pagetypeTabTypesInApplicationDomain = this.GetPagetypeTabTypesInApplicationDomain();

			foreach (Type type in pagetypeTabTypesInApplicationDomain)
			{
				tabs.Add((PageTypeTab)Activator.CreateInstance(type));
			}

			return tabs;
		}

		public virtual List<PageTypeTabDefinition> GetPageTypeTabDefinitions()
		{
			List<Type> types = (from p in this.GetTypesWithAttributes()
							   where !p.IsAbstract
							   select p).ToList<Type>();

			List<PageTypeTabDefinition> definitions = new List<PageTypeTabDefinition>();

			foreach (Type type in types)
			{
				PageTypeTabAttribute attribute = (PageTypeTabAttribute)PageTypeTabHelper.GetAttribute(type, typeof(PageTypeTabAttribute));

				PageTypeTabDefinition item = new PageTypeTabDefinition
				{
					Type = type,
					Attribute = attribute
				};

				definitions.Add(item);
			}

			return definitions;
		}

		public List<Type> GetPagetypeTabTypesInApplicationDomain()
		{
			Func<AssemblyName, bool> predicate = null;
			string tabTypeAssemblyName = typeof(PageTypeTab).Assembly.GetName().Name;
			List<Type> list = new List<Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (Assembly assembly in assemblies)
			{
				if (predicate == null)
				{
					predicate = a => a.Name == tabTypeAssemblyName;
				}

				if (assembly.GetReferencedAssemblies().Count<AssemblyName>(predicate) != 0)
				{
					IEnumerable<Type> collection = assembly.GetTypes().AssignableTo(typeof(PageTypeTab)).Concrete();
					list.AddRange(collection);
				}
			}

			return list;
		}

		public List<Type> GetTypesWithAttributes()
		{
			return (from a in AppDomain.CurrentDomain.GetAssemblies()
					from t in a.GetTypes()
					let attributes = t.GetCustomAttributes(typeof(PageTypeTabAttribute), true)
					where (attributes != null) && (attributes.Length > 0)
					select t).ToList<Type>();
		}

	}
}
