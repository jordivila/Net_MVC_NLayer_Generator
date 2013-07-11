/// <reference path="$customNamespace$.A.Intellisense.js" />
$customNamespace$.Widgets.DialogInline =
{
    MsgTypes: {
        Success: 0
        , Warning: 1
        , Error: 2
        , Confirm: 3
    }
    , Create: function (selector, msgType, onAnswered) {
        /// <summary>
        ///     Display the Inline messages
        ///     &#10;1 - Create(selector) 
        ///     &#10;2 - Create(selector, msgType) 
        ///     &#10;3 - Create(selector, msgType, callback)
        /// </summary>
        /// <param name="selector" type="String">
        ///     jQuery selector
        /// </param>
        /// <param name="msgType" type="$customNamespace$.Widget.InlineMsg.MsgTypes">
        ///     A string indicating which message type must be used
        /// </param>
        /// <param name="callback" type="Function">
        ///     A function to call once the message has been closed or answered
        /// </param>
        /// <returns type="jQuery" />
        jQuery(selector).widgetMsg({
            msgType: msgType
            , answered: function (e, args) {
                onAnswered(e, args);
            }
        });
    }
};