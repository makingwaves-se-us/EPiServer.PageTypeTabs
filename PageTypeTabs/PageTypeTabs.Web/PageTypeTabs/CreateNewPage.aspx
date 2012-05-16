<%@ Page Language="C#" AutoEventWireup="true" Inherits="PageTypeTabs.UI.CreateNewPage" %>

<%@ Register TagPrefix="PageTypeTabs" Namespace="PageTypeTabs.Controls" Assembly="PageTypeTabs"  %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainRegion" runat="server">

	<script type='text/javascript'>

		jQuery(function ($) {

			handleTabs(0);

			$('#searchPageTypes').keyup(function (arg) {
				arg = arg || window.event;
				var charCode = arg.which || arg.keyCode;
				var charStr = String.fromCharCode(charCode);
				if (/[a-zA-Z0-9-_() ]/i.test(charStr) || charCode == 8 || charCode == 32) {
					var inputVal = $('.search_page_types').val().toLowerCase();
					$('table.epi-default').show();
					if (inputVal.length > 0) {
						$('table.epi-default tbody tr').each(function () {
							var parentTable = $(this).parents('table');
							$(parentTable).show();
							var tdText = $('td', $(this)).eq(1).text().toLowerCase();
							$(this).show();
							if (tdText.indexOf(inputVal) == -1) {
								$(this).hide();
								if ($(parentTable).find('tbody').find('td:visible').length == 0)
									$(parentTable).hide();
							}
						});
					} else {
						$('table.epi-default tr').removeAttr("style");
					}

				}
			});
		});

		function onNavigate(newPageLink) { return -1; }
		function onCommand(newCommand) { return -1; }

		function getKey(key) {
			var keycode = (key == null) ? event.keyCode : key.keyCode;
			return String.fromCharCode(keycode).toLowerCase();
		}

		function resetRows() {
			$('.epi-default, .epi-default tr').removeAttr("style");
		}

		function handleTabs(index) {
			$('.search_page_types').val('');
			resetRows();
			$("div[id*='ptTab_']").hide();
			$("#ptTab_" + index).show();
			$('.epi-tabView ul li').each(function (i) {
				if (index == i)
					$(this).removeClass('epi-tabView-navigation-item').addClass('epi-tabView-navigation-item-selected');
				else
					$(this).removeClass('epi-tabView-navigation-item-selected').addClass('epi-tabView-navigation-item');
			});
		}
		
	</script>

	<div class="epitoolbuttonrow">
		<EPiServerUI:ToolButton id="Cancel" OnClick="Cancel_Click"  runat="server" Text="<%$ Resources: EPiServer, button.cancel %>" ToolTip="<%$ Resources: EPiServer, button.cancel %>" SkinID="Cancel" />
	</div>

	<PageTypeTabs:PageTypeTabsControl runat="server" ID="PageTypeTabsWebControl" />

    <div id="searchPageTypesCont" style="padding: 6px 3px; margin: 6px 0 6px 0; border: 1px solid #BEBEBE !important ">
		Search: <input type="text" name="searchPageTypes" class="search_page_types" id="searchPageTypes" />
	</div>

    <asp:PlaceHolder runat="server" id="ListHolder" />

</asp:Content>