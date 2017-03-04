<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentModerator.aspx.cs" Inherits="Sitecore.ContentModerator.Admin.ContentModerator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Content Moderator</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.10.4/themes/smoothness/jquery-ui.css" />
    <style>
        * {
            font-family: Arial;
            line-height: 20px;
            resize: none;
            outline: none;
        }

        h1 {
            font-size: 30px;
            color: #fff;
            text-transform: uppercase;
            font-weight: 300;
            text-align: center;
            margin-bottom: 15px;
        }

            h1 span {
                font-size: 12px;
                text-transform: none;
                font-weight: bold;
            }

        a:link, a:visited, a:hover, a:active {
            color: #BB1000;
            font-size: 13px;
            text-decoration: none;
        }

        .moderator-highlight {
            background-color: #FF8;
            padding: 3px 5px;
            font-weight: bold;
            color: #bb1000;
            -webkit-box-shadow: 1px 1px 3px 0px rgba(0,0,0,0.75);
            -moz-box-shadow: 1px 1px 3px 0px rgba(0,0,0,0.75);
            box-shadow: 1px 1px 3px 0px rgba(0,0,0,0.75);
            cursor: default;
        }

        #moderator-button {
            border: 1px solid #d22542;
            text-transform: uppercase;
            color: #d22542;
            background: #FFF;
            padding: 5px 10px;
            margin-top: -14px;
            margin-left: 10px;
            font-size: 12px;
            cursor: hand;
        }

        .tbl-item {
            text-align: center;
            margin: 24px 10px 10px;
        }

            .tbl-item a {
                color: #FFF;
            }

        table {
            width: 100%;
            table-layout: fixed;
        }

        .tbl-header {
            background-color: rgba(255,255,255,0.3);
        }

        .tbl-content {
            margin-top: 0;
            border: 1px solid rgba(255,255,255,0.3);
        }

        th {
            padding: 20px 15px;
            text-align: left;
            font-weight: bold;
            font-size: 14px;
            color: #fff;
            text-transform: uppercase;
        }

        td {
            padding: 15px;
            text-align: left;
            vertical-align: middle;
            font-weight: 300;
            font-size: 14px;
            color: #fff;
            border-bottom: solid 1px rgba(255,255,255,0.1);
            line-height: 22px;
            vertical-align: top;
        }

        body {
            background: -webkit-linear-gradient(left,#d22542,#fab398);
            background: linear-gradient(to right,#d22542,#fab398);
            font-family: sans-serif;
        }

        section {
            margin: 50px;
        }

        ::-webkit-scrollbar {
            width: 10px;
        }

        ::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 10px rgba(0,0,0,0.3);
        }

        ::-webkit-scrollbar-thumb {
            -webkit-box-shadow: inset 0 0 10px rgba(0,0,0,0.3);
        }

        @media print {
            #moderator-button {
                display: none;
            }
        }
        .hide{display:none;}
        .td3 img {
            max-width:100%;
        }
    </style>    
</head>
<body>
    <form id="form1" runat="server">
        <h1>Content Moderation Report <span>-</span>
            <asp:Label ID="itemNamelbl" runat="server" /></h1>

        <div class="tbl-item">
            <button id="gotoitem" onclick="javascript:window.print()">Goto Sitecore Item</button>
            <button id="startModeration">Moderate</button>
            <button id="print" onclick="javascript:window.print()">Print</button></div>

        <div class="tbl-header" style="padding-right: 6px;">
            <table cellpadding="0" cellspacing="0" border="0">
                <thead>
                    <tr>
                        <th width="15%">Field Name</th>
                        <th width="15%">Field Type</th>
                        <th>Field Value</th>
                    </tr>
                </thead>
            </table>
        </div>

        <asp:Repeater ID="fieldRepeater" runat="server">
            <HeaderTemplate>
                <div class="tbl-content">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="itemRows">
                    <td width="15%">
                        <span><%#Eval("DisplayName") %></span>
                        <span class="itemId hide"><%#Eval("ItemId") %></span>
                        <span class="fieldName hide"><%#Eval("FieldName") %></span>
                        <span class="isImg hide"><%#Eval("IsImage") %></span>
                    </td>
                    <td width="15%"><span class="dispName"><%#Eval("Type") %></span></td>
                    <td class="td3">
                        <span class="fieldVal"><%#Eval("Value") %></span>                        
                        <span class="result"></span>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                        </table>
                    </div>
            </FooterTemplate>
        </asp:Repeater>
    </form>
    <script type="text/javascript">
        var $j = jQuery.noConflict();
        $j(document).ready(function () {
            $j('#startModeration').click(function () {
                $j('.itemRows').each(function (x, y) {
                    var postData = { "ItemId": $j(this).find('.itemId').text(), "FieldName": $j(this).find('.fieldName').text(), "IsImage": $j(this).find('.isImg').text() };
                    $j.ajax({
                        type: "POST",
                        url: "/sitecore/api/moderate/moderation/start",
                        data: JSON.stringify(postData),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data, status) {
                            //append status in the page
                        },
                        error: function (data, status) {
                            alert(data);
                        }
                    });
                })
                return false;
            });
        });
    </script>
</body>
</html>
