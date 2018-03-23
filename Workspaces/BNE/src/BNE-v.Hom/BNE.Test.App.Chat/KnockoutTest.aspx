<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KnockoutTest.aspx.cs" Inherits="BNE.Test.App.Chat.KnockoutTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.6.4.js"></script>
    <script src="Scripts/jquery-ui-1.9.2.js"></script>
    <script src="Scripts/knockout-2.2.0.debug.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var viewModelTest = function() {

                var self = this;
                this.prop1 = ko.observable("abc");

                alert(prop1.toString());
            };
            ko.applyBindings = new viewModelTest();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
