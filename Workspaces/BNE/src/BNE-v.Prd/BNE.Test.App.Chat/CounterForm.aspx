<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CounterForm.aspx.cs" Inherits="BNE.Test.App.Chat.CounterForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.6.4.min.js"></script>
    <script src="Scripts/jquery.signalR-1.2.1.js"></script>
    <script src='/signalr/hubs'></script>
    <script type="text/javascript">
        $(function () {

            $.connection.hub.url = "signalr/hubs";
            // Declare a proxy to reference the hub.
            var counter = $.connection.hitCounterServer;

            // register online user at the very begining of the page
            $.connection.hub.start().done(function () {
                // Call the Send method on the hub.
                counter.server.getTotalOnline().done(function(arg) {
                    $('#paragraph_counter').text(arg);
                }).fail(function(exp) {
                    if (true) { // todo
                        
                    }
                });
            });;

            // Create a function that the hub can call to recalculate online users.
            counter.client.recalculateOnlineUsers = function (message) {

                // Add the message to the page.
                $('#paragraph_counter').text(message);
            };

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Counter
        <p id="paragraph_counter">
        </p>
        </div>
    </form>
</body>
</html>
