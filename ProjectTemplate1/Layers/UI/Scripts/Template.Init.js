/// <reference path="$customNamespace$.A.Intellisense.js" />
if (!window.console) {
    console = {
        log: function (msg) {

        }
    };
};

String.prototype.toDateFromAspNet = function () {
    var dte = eval("new " + this.replace(/\//g, '') + ";");
    dte.setMinutes(dte.getMinutes() - dte.getTimezoneOffset());
    return dte;
};

String.prototype.toBoolean = function () {
    return (/^true$/i).test(this);
};

function parseBoolean(value) {
    return value.toBoolean();
}

var $customNamespace$ = {};


