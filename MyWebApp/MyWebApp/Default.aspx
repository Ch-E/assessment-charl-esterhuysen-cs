<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyWebApp._Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <img src="Images/VW.png" alt="VW" width="55" height="55" style="float:left;position:relative;top:10px;"><h2 class="header">&nbsp;Volkswagen Stock - Roodepoort</h2>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <ajaxToolkit:Accordion ID="Accordion1" runat="server">
        </ajaxToolkit:Accordion>
        <asp:Panel ID="MyContent" runat="server">
        </asp:Panel>

</asp:Content>
