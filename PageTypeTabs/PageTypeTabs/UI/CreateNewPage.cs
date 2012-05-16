using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using EPiServer.DataAbstraction;
using EPiServer.UI;
using EPiServer.UI.WebControls;
using EPiServer.Web.PageExtensions;
using PageTypeBuilder.Discovery;
using PageTypeTabs.Controls;
using PageTypeTabs.Locators;

namespace PageTypeTabs.UI
{
	public class CreateNewPage : SystemPageBase
	{
		private string _mode;
		private List<PageType> _availablePageTypes;
		protected ToolButton Cancel;
		protected PlaceHolder ListHolder;
		protected PageTypeTabsControl PageTypeTabsWebControl;
		private List<PageTypeDefinition> ptDefinitions;

		public CreateNewPage() : base(0, SiteRedirect.OptionFlag)
		{
			_availablePageTypes = new List<PageType>();
			ptDefinitions = new PageTypeDefinitionLocator().GetPageTypeDefinitions();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (Request.Params["mode"] != null)
			{
				_mode = Request.Params["mode"].ToLower();
			}

			if (IsPostBack) return;

			PageType type = PageType.Load(CurrentPage.PageTypeID);
			IEnumerable<PageType> allPageTypes = PageType.List();
			_availablePageTypes = type.FilterAllowedChildTypes(allPageTypes, User).ToList();

			if (_availablePageTypes.Count == 1)
			{
				HandleRedirect(string.Format("EditPanel.aspx?parent=" + CurrentPageLink + "&type={0}&mode=" + Request.QueryString["mode"], _availablePageTypes[0].ID));
			}

			List<PageTypeTab> definedTabs = new PageTypeTabFactory().GetDefinedTabs();
			PageTypeTabsWebControl.Tabs = definedTabs;

			int num = 1;
			foreach (PageTypeTab tab in definedTabs)
			{
				AddPageTypeList(num.ToString(CultureInfo.InvariantCulture), FindPageTypesByTab(tab));
				num++;
			}

			var leftovers = new Dictionary<string, List<PageType>>();
			leftovers.Add("__NULL__", _availablePageTypes);
			AddPageTypeList("0", leftovers);
		}

		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);

			MasterPageFile = ResolveUrlFromUI("MasterPages/EPiServerUI.Master");
			SystemMessageContainer.Heading = "Create New Page";
		}

		private void AddPageTypeList(string containerId, Dictionary<string, List<PageType>> sections)
		{
			PageTypeListControl child = new PageTypeListControl
			{
				ContainerID = containerId,
				ParentID = CurrentPage.PageLink.ID.ToString(CultureInfo.InvariantCulture),
				PageTypeSections = sections
			};

			ListHolder.Controls.Add(child);
		}

		protected void Cancel_Click(object sender, EventArgs e)
		{
			if ((this._mode == "simpleeditmode") || (this._mode == "createpage"))
			{
				base.RedirectToViewMode(this.CurrentPage.LinkURL);
			}
			else
			{
				base.Response.Redirect("EditPanel.aspx?id=" + this.CurrentPage.PageLink.ToString());
			}
		}

		private Dictionary<string, List<PageType>> FindPageTypesByTab(PageTypeTab tab)
		{
			var sections = new Dictionary<string, List<PageType>>();

			IEnumerable<PageTypeTabDefinition> definitions = from p in PageTypeTabFactory.Instance.GetPageTypeTabDefinitions()
															 where p.Attribute.Tab == tab.GetType()
															 select p;

			foreach (PageTypeTabDefinition tabDefinition in definitions)
			{
				foreach (PageTypeDefinition definition in ptDefinitions)
				{
					if (tabDefinition.Type == definition.Type)
					{
						PageType item = PageType.Load(definition.GetPageTypeName());

						string section = tabDefinition.Attribute.Section;
						if (string.IsNullOrEmpty(section))
							section = "__NULL__";

						if (sections.ContainsKey(section))
						{
							var sectionList = sections[section];
							sectionList.Add(item);
							sections[section] = sectionList;
						}
						else
						{
							sections.Add(section, new List<PageType> { item });
						}
						
						_availablePageTypes.Remove(item);
					}
				}
			}

			return sections;
		}

		protected virtual void HandleRedirect(string url)
		{
			base.Response.Redirect(url);
		}
	}
}
