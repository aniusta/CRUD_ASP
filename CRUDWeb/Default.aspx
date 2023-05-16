<%@ Page Title="Profiles" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CRUDWeb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <link rel="stylesheet" href="styles.css" />
        <h2 id="title"><%: Title %></h2>
        <asp:Table ID="myTable" class="table-control" runat="server"></asp:Table>
        <asp:Panel ID="dynamicPopupContainer" runat="server"></asp:Panel>
        <script src="script.js" type="text/javascript"></script>
        <asp:HiddenField ID="hfName" runat="server" /> 
            <link rel="stylesheet" href="styles.css" />
        <div class="modalBackground" id="mymodal" role="dialog" runat="server" style="display:none;">
            <div class="modalPopup">
                <div class="modal-content">
                    <div class="modalHeader">
                        <h2 class="modal-title">Edit record</h2>
                        <asp:Button ID="closeButton" runat="server" Text="X" OnClick="closePopup_Click"/>
                    </div>
                    <div class="modalContent">
                        <fieldset>
                            <div class="labelpopup-control">
                                <label for="fullname">Name:</label>
                                <asp:TextBox ID="fullname" CssClass="text-control" placeholder="Enter Name" runat="server"></asp:TextBox>
                            </div>
                            <div class="labelpopup-control">
                                <label for="address">Address:</label>
                                <asp:TextBox ID="address" CssClass="text-control" placeholder="Enter Address" runat="server"></asp:TextBox>
                            </div>
                            <div class="labelpopup-control">
                                <label for="calendarDOB">Date of Birth:</label>
                                <asp:TextBox ID="calendarDOB" CssClass="calendar-control" TextMode="Date" runat="server"></asp:TextBox>
                            </div>
                            <div class="labelpopup-control">
                                <label for="genderRadioList">Gender:</label>
                                <asp:RadioButton ID="male" Text="Male" GroupName="gender" Value="male" runat="server" />
                                <asp:RadioButton ID="female" Text="Female" GroupName="gender" Value="female" runat="server" />
                            </div>
                            <div class="labelpopup-control">
                                <label for="civilStatusDropdown">Civil Status:</label>
                                <asp:DropDownList ID="civilStatusDropdown" runat="server">
                                    <asp:ListItem Text="Single" Value="single"></asp:ListItem>
                                    <asp:ListItem Text="Married" Value="married"></asp:ListItem>
                                    <asp:ListItem Text="Divorced" Value="divorced"></asp:ListItem>
                                    <asp:ListItem Text="Widowed" Value="widowed"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="labelpopup-control">
                                <label for="photoUpload">Upload Photo:</label>
                                <asp:FileUpload ID="photoUpload" runat="server" />
                                <span id="errorLabel" style="display:none;"></span>
                            </div>
                            <asp:Button ID="Save" runat="server" OnClick="updateData" Text="Save"/>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </main>
</asp:Content>
