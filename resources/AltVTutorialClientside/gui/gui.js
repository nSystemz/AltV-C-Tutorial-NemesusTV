alt.on('sendNotification', (status,text) => notify(status, text));
function notify(status, text)
{
    if(text.length > 0)
    {
        toastr.options.timeOut = 5000;
        toastr.options.positionClass = 'toast-top-right';
        toastr[status](text);
    }
}