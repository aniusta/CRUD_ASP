<%@ Page Title="Profile Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Form.aspx.cs" Inherits="CRUDWeb.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <link rel="stylesheet" href="styles.css" />
        <h2 id="title"><%: Title %></h2>
        <fieldset>
            <label for="fullname" class="label-control">Name:
                <asp:TextBox ID="fullname" 
                    runat="server" 
                    class="text-control"
                    placeholder="Enter Name">
                </asp:TextBox>
            </label>
            <label for="address" class="label-control">Address:
                <asp:TextBox ID="address"
                    runat="server"
                    class="text-control"
                    placeholder="Enter Address"></asp:TextBox>
            </label>
            <label for="calendarDOB" class="label-control">Date of Birth:
                <asp:Calendar ID="calendarDOB" 
                    class="calendar-control"
                    runat="server">
                    <DayStyle CssClass="myCalendarDay" ForeColor="#2d3338" />
                </asp:Calendar>
            </label>
            <label for="genderRadioList" class="label-control">Gender:
                <asp:RadioButtonList ID="genderRadioList" CssClass="custom-radio" runat="server">
                    <asp:ListItem Text="Male" Value="Male" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                </asp:RadioButtonList>
            </label>
            <label for="civilStatusDropdown" class="label-control">Civil Status:
                <asp:DropDownList ID="civilStatusDropdown" runat="server">
                    <asp:ListItem Text="Single" Value="Single"></asp:ListItem>
                    <asp:ListItem Text="Married" Value="Married"></asp:ListItem>
                    <asp:ListItem Text="Divorced" Value="Divorced"></asp:ListItem>
                    <asp:ListItem Text="Widowed" Value="Widowed"></asp:ListItem>
                </asp:DropDownList>
            </label>
            <label for="photoUpload" class="label-control">Upload Photo:
                <asp:FileUpload ID="photoUpload" runat="server"/>
                <asp:Label ID="errorLabel" runat="server" Text="" Visible="false"></asp:Label>
            </label>
            <asp:Button ID="submit" runat="server" Text="Submit" OnClick="submit_Click"/>
        </fieldset>

    </main>
</asp:Content>
