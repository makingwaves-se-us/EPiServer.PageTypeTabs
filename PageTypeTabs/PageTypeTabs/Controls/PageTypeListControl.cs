using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using EPiServer.DataAbstraction;

namespace PageTypeTabs.Controls
{
	public class PageTypeListControl : Control
	{
		public Dictionary<string, List<PageType>> PageTypeSections { get; set; }
		public string ContainerID { get; set; }
		public string ParentID { get; set; }

		public override void RenderControl(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(ContainerID))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("ptTab_{0}", this.ContainerID));
			}

			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			var sections = PageTypeSections.OrderBy(s => s.Key);
			foreach (var section in sections)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Div);

				writer.AddAttribute(HtmlTextWriterAttribute.Class, "epi-default");
				writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.RenderBeginTag(HtmlTextWriterTag.Table);
				writer.RenderBeginTag(HtmlTextWriterTag.Thead);

				if (!string.Equals(section.Key, "__NULL__"))
				{
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);
					writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "3");
					writer.RenderBeginTag(HtmlTextWriterTag.Th);
					writer.RenderBeginTag(HtmlTextWriterTag.H4);
					writer.Write(section.Key);
					writer.RenderEndTag();
					writer.RenderEndTag();
					writer.RenderEndTag();
				}

				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.AddAttribute(HtmlTextWriterAttribute.Scope, "col");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("&nbsp;");
				writer.RenderEndTag();
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Name");
				writer.RenderEndTag();
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Description");
				writer.RenderEndTag();
				writer.RenderEndTag();
				writer.RenderEndTag();
				writer.RenderBeginTag(HtmlTextWriterTag.Tbody);

				var types = section.Value.OrderBy(t => t.Name);
				foreach (PageType type in types)
				{
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					string str = string.Format("EditPanel.aspx?parent={0}&type={1}&mode=", this.ParentID, type.ID);
					writer.AddAttribute(HtmlTextWriterAttribute.Href, str);
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write("Create");
					writer.RenderEndTag();
					writer.RenderEndTag();
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write(type.Name);
					writer.RenderEndTag();
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write(type.Description);
					writer.RenderEndTag();
					writer.RenderEndTag();
				}

				writer.RenderEndTag();
				writer.RenderEndTag();

				writer.RenderEndTag();
			}

			writer.RenderEndTag();

			base.RenderControl(writer);
		}
	}
}
