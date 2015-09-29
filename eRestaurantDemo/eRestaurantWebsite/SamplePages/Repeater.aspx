<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Repeater.aspx.cs" Inherits="SamplePages_Repeater" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1> Repeater Display</h1>  <asp:ObjectDataSource ID="ODSReservations" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetReservationsByDate" TypeName="eRestaurantSystem.BLL.AdminController">
    <SelectParameters>
        <asp:ControlParameter ControlID="DateArg" Name="reservationdate" PropertyName="Text" Type="String" />
    </SelectParameters>
    </asp:ObjectDataSource>
        <table align="center" style="width: 70%">
            <tr>
                <td>Enter a date (mm/dd/yyyy):</td>
                <td>
                    <asp:TextBox ID="DateArg" runat="server">01/01/1990</asp:TextBox>
                    <asp:LinkButton ID="FetchReservations" runat="server">Fetch Reservations</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    
                </td>
               
            </tr>
            <tr>
                <td colspan="2">
                    <div class="row col-md-12">
                    <asp:Repeater ID="EventReservationList" runat="server" DataSourceID="ODSReservations">
                    <ItemTemplate>
                        <h3><%# Eval("Description") %></h3>
                        <asp:Repeater ID="ReservationList" runat="server"
                                DataSource='<%# Eval("Reservations") %>'>
                            <ItemTemplate>
                                <h4>
                                <%# Eval("CustomerName") %>
                                <%# Eval("ContactPhone") %></h4>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                    </asp:Repeater>
</div>
                </td>
                
            </tr>
        </table>
     
    

</asp:Content>

