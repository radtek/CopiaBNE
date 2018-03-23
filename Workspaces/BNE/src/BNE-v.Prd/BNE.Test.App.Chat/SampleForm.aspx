<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SampleForm.aspx.cs" Inherits="BNE.Test.App.Chat.SampleForm" %>

<%@ Register TagPrefix="chat" TagName="RenderizarChat" Src="SharedControls/RenderizarChat.ascx" %>
<%@ Register TagPrefix="a1" Namespace="BNE.Test.App.Chat" Assembly="BNE.Test.App.Chat" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chat Test</title>
    <script src="Scripts/jquery-1.6.4.js"></script>
    <script>
        var count = 0;
        function createOnlineContact(e) {
            var vm = scopeViewModel;
            if (vm == null
                || vm == typeof (undefined)) {
                return;
            }
            if (vm.createEmptyContact == null
                || vm.createEmptyContact == typeof (undefined)) {
                return;
            }

            var contactVm = new vm.createEmptyContact();
            if (contactVm == null
                || contactVm == typeof (undefined)) {
                return;
            }

            contactVm.title("Contact Random | " + count.toString());
            contactVm.lastMessage("Hello World");
            contactVm.lastDate(new Date(2014, 3, Math.floor((Math.random() * 30) + 1)));

            count = count + 1;
        }
        function closeAll(e) {
            var vm = scopeViewModel;
            if (vm == null
                || vm == typeof (undefined)) {
                return;
            }

            vm.closeAllChats();
        }
        function createRandomChat(e) {
            var vm = scopeViewModel;
            if (vm == null
                || vm == typeof (undefined)) {
                return;
            }
            if (vm.createEmptyChat == null
                || vm.createEmptyChat == typeof (undefined)) {
                return;
            }

            var chatVm = new vm.createEmptyChat();
            if (chatVm == null
                || chatVm == typeof (undefined)) {
                return;
            }

            chatVm.title("Chat Random | " + count.toString());
            count = count + 1;
        }

        function createReceiveMessageChat(e) {
        
            raiseNewMessage(false);
        }

        function raiseNewMessage(arg1)
        {
            var first = retFirstActiveChat();

            if (isUndefined(first))
                return;

            var message = first.createMessage(arg1);
            message.content("Oaio oxijcxz oi jowqiej oqiwe ad");
        }
        function createSenderMessageChat(e) {
            raiseNewMessage(true);
        }

        function activeBlink() {
            var first = retFirstActiveChat();

            if (isUndefined(first))
                return;

            first.activeBlink(true);
        }
        function removeBlink() {
            var first = retFirstActiveChat();

            if (isUndefined(first))
                return;

            first.activeBlink(false);
        }

        function retFirstActiveChat() {
            var vm = scopeViewModel;
            if (vm == null
                || vm == typeof (undefined)) {
                return null;
            }

            if (vm.createEmptyChat == null
                || vm.createEmptyChat == typeof (undefined)) {
                return null;
            }

            if (vm.activeChats == null
                || vm.activeChats == typeof (undefined)) {
                return null;
            }

            var first = vm.activeChats.validItems()[0];

            if (isUndefined(first))
                return null;

            return first;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scriptmanager1" runat="server"></asp:ScriptManager>

        <div style="width: 710px; margin-left: auto; margin-right: auto; border-top-color: rgb(102, 102, 102); border-top-style: solid; border-top-width: 1px; clear: both; display: block; font-family: Arial, Helvetica, sans; font-size: 11px; margin-bottom: 0px; margin-top: 6px; padding-top: 6px; text-align: center;">
            
            <div style="float: left; margin-left: 15px;">
                <h2>Add Contact Chat</h2>
                <input type="button" value="Do" onclick="createOnlineContact()" />
            </div>

            <div style="float: left; margin-left: 15px;">
                <h2>Add Window Chat</h2>
                <input type="button" value="Do" onclick="createRandomChat()" />
            </div>

            <div style="float: left; margin-left: 15px;">
                <h2>Add Message by Other</h2>
                <input type="button" value="Do" onclick="createReceiveMessageChat()" />
            </div>
            
              <div style="float: left; margin-left: 15px;">
                <h2>Add Message by Owner</h2>
                <input type="button" value="Do" onclick="createSenderMessageChat()" />
            </div>

            <div style="float: left; margin-left: 15px;">
                <h2>Close All</h2>
                <input type="button" value="Do" onclick="closeAll()" />
            </div>
            
            <div style="float: left; margin-left: 15px;">
                <h2>Active Blink</h2>
                <input type="button" value="Do" onclick="activeBlink()" />
            </div>
            
            <div style="float: left; margin-left: 15px;">
                <h2>Remove Blink</h2>
                <input type="button" value="Do" onclick="removeBlink()" />
            </div>

            <a1:SampleContent runat="server" ID="SampleContent" Visible="false" />
        </div>

        <chat:RenderizarChat ID="chat_render" runat="server" />

    </form>
</body>
</html>
