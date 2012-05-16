EPiServer.PageTypeTabs
==

EPiServer.PageTypeTabs is a slightly modified version of [Joshua Folkerts' PageTypeTabs](http://www.joshuafolkerts.com/blog/2011/08/page-type-tabs/) for EPiServer. This version allows you to separate the page types within each tab into separate sections. This was developed and tested with EPiServer CMS 6 R2 and PageTypeBuilder 1.3.1.

Setup &amp; Usage
--

1) Update the Virtual Path Mapping in episerver.config to point to the new CreateNewPage.aspx file.
	
		<virtualPath customFileSummary="~/FileSummary.config">
			<providers>
				 <add virtualName="CreateNewPage" 
				 	showInFileManager="false" bypassAccessCheck="false"
				 	virtualPath="~/PageTypeTabs/CreateNewPage.aspx"  
				 	physicalPath="" name="CreateNewPageMapping"
				 	type="EPiServer.Web.Hosting.VirtualPathMappedProvider,EPiServer" />
			</providers>
		</virtualPath>
		
		<virtualPathMappings>
			<add url="~/EPiServer/CMS/Edit/NewPage.aspx" 
				mappedUrl="~/PageTypeTabs/CreateNewPage.aspx" />
		</virtualPathMappings>


2) Create your tabs similar to how you would create a PageTypeBuilder tab

		public class MyPageTypeTab : PageTypeTab
		{
			public override string Name
			{
				get { return "My Tab"; }
			}
	
			public override int SortIndex
			{
				get { return 100; }
			}
		}

3) Add the PageTypeTab attribute to your PageType classes

		[PageTypeTab( Tab=typeof(MyPageTypeTab), Section ="General Pages")]
		[PageType(Filename = "/MyPage.aspx", Name = "My Page")]
		public class MyPage : TypedPageData
		{
		}