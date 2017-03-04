<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentModerator.aspx.cs" Inherits="Sitecore.ContentModerator.Admin.ContentModerator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Content Moderator</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.10.4/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .fldtbl {
            width: 100%;
            display: table;
            border-collapse: collapse;
        }

            .fldtbl th, .fldtbl td {
                border: 1px solid #DDD;
                text-align: left;
                padding: 10px;
            }

        .td1 {
            width: 10%;
        }

        .td2 {
            width: 5%;
        }

        .td3 {
            width: 35%;
        }

        .td4 {
            width: 5%;
        }

        .td5 {
            width: 35%;
        }

        .fldtbl td img {
            max-width: 100%;
        }
        .parent{float:left;position:relative;}
        .left{width:50%;float:left;clear:left;}
        .right {width:50%;float:left;clear:right;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="parent">
            <div class="left">
                <asp:Label ID="itemIdlbl" runat="server"></asp:Label><br />
                <asp:Label ID="itemNamelbl" runat="server"></asp:Label><br />
                <asp:Label ID="itemPathlbl" runat="server"></asp:Label>
            </div>
            <div class="right">
                <button id="startModeration" title="Start Moderation" value="Moderate"></button>
            </div>
            <asp:Repeater ID="fieldRepeater" runat="server">
                <HeaderTemplate>
                    <table class="fldtbl">
                        <tr>
                            <th>Field</th>
                            <th>FieldType</th>
                            <th>FieldValue</th>
                            <th>Review</th>
                            <th>Result</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="td1"><%#Eval("Name") %>-<%#Eval("DisplayName") %></td>
                        <td class="td2"><%#Eval("Type") %></td>
                        <td class="td3"><%#Eval("Value") %></td>
                        <td class="td4">Loading...</td>
                        <td class="td5">Loading...</td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </form>
    <script type="text/javascript">
        var $j = jQuery.noConflict();
        $j(document).ready(function () {
            $j('#startModeration').click(function () {
                alert("Starting moderation!!");
            });
        });
    </script>
</body>
</html>