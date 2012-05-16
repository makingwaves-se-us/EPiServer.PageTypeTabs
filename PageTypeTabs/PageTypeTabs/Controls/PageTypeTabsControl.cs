using System.Collections.Generic;
using System.Web.UI;

namespace PageTypeTabs.Controls
{
	public class PageTypeTabsControl : Control
	{
		public List<PageTypeTab> Tabs { get; set; }

		public override void RenderControl(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "epi-tabView");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "epi-tabView-navigation");
			writer.RenderBeginTag(HtmlTextWriterTag.Ul);
			
			WriteTab("Default", writer, 0);

			for (int i = 1; i <= this.Tabs.Count; i++)
			{
				WriteTab(Tabs[i - 1].Name, writer, i);
			}

			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		private void WriteTab(string TabText, HtmlTextWriter writer, int index)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "epi-tabView-navigation-item");
			writer.RenderBeginTag(HtmlTextWriterTag.Li);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "epi-tabView-tab");
			writer.AddAttribute(HtmlTextWriterAttribute.Target, "_self");
			writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("handleTabs({0});return false;", index));
			writer.AddAttribute(HtmlTextWriterAttribute.Style, "cursor: pointer");
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write(TabText);
			writer.RenderEndTag();
			writer.RenderEndTag();
		}
	}
}
