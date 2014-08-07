
$customNamespace$.Widgets.DialogInline =
{
    MsgTypes: {
        Success: 0
        , Warning: 1
        , Error: 2
        , Confirm: 3
    }
    , Create: function (selector, msgType, onAnswered) {
        jQuery(selector).widgetMsg({
            msgType: msgType
            , answered: function (e, args) {
                onAnswered(e, args);
            }
        });
    }
};