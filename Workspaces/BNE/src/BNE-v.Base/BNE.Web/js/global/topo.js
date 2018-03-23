var page;
function pageLoad() {
    page = Sys.WebForms.PageRequestManager.getInstance();
    page.add_initializeRequest(OnInitializeRequest);
}
function OnInitializeRequest(sender, args) {
    //var postBackElement = args.get_postBackElement();
    if (page.get_isInAsyncPostBack()) {
        //alert('One request is already in progress.');
        args.set_cancel(true);
    }
}    