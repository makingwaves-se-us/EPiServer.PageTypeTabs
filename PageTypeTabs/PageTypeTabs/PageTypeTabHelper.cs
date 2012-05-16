using System;

namespace PageTypeTabs
{
	public class PageTypeTabHelper
	{
		public static Attribute GetAttribute(Type type, Type attributeType)
		{
			Attribute attribute = null;
			object[] customAttributes = type.GetCustomAttributes(typeof (PageTypeTabAttribute), true);
			
			foreach (object customAttribute in customAttributes)
			{
				if (attributeType.IsInstanceOfType(customAttribute))
				{
					attribute = (Attribute) customAttribute;
				}
			}

			return attribute;
		}
	}
}
