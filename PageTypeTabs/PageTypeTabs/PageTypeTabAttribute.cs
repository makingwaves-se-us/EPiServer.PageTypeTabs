using System;

namespace PageTypeTabs
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class PageTypeTabAttribute : Attribute
	{
		public virtual Type Tab { get; set; }
		public virtual string Section { get; set; }
	}
}
