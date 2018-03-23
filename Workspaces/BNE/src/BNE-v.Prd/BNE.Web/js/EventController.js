// <![CDATA[

// Event Controller
var eventcontrol = {
    VERSION: function() {
        return 0.1;
    },
    VERSION_DATE: function() {
        return new Date("23-10-2009");
    },
    AUTHOR: function() {
        return "TechResult";
    }
};

eventcontrol.util = {

    findControl: function(control) {
        return $('[id$=' + control + ']');
    },

    findClientID: function(control) {
        return eventcontrol.util.findControl(control).attr("id");
    },

    addEvent: function(control, event, callback) {
        eventcontrol.util.findControl(control).bind(event, callback);
    },

    addEventParms: function(control, event, callback, parms) {
        eventcontrol.util.findControl(control).bind(event, parms, callback);
    },

    remEvent: function(control, event) {
        eventcontrol.util.findControl(control).unbind(event);
    },

    hasEvent: function(control, event) {
        for (var controlEvent in eventcontrol.util.findControl(control).data("events"))
            if (controlEvent == event)
            return true;

        return false;
    }
}

// ]]>