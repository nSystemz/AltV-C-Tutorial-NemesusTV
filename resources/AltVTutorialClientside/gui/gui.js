//Notifications
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

//HUD Elemente

var healthbar = new ProgressBar.Circle(container, {
    strokeWidth: 6,
    easing: 'easeInOut',
    duration: 1400,
    color: '#fc2003',
    trailColor: '#eee',
    trailWidth: 1,
    text: {
        value: '&#10084;',
        alignToBottom: false
    },
    svgStyle: null
});

var hungerbar = new ProgressBar.Circle(container2, {
    strokeWidth: 6,
    easing: 'easeInOut',
    duration: 1400,
    color: '#f5b042',
    trailColor: '#eee',
    trailWidth: 1,
    text: {
        value: '&#127828;',
        alignToBottom: false
    },
    svgStyle: null
});

var thirstbar = new ProgressBar.Circle(container3, {
    strokeWidth: 6,
    easing: 'easeInOut',
    duration: 1400,
    color: '#42adf5',
    trailColor: '#eee',
    trailWidth: 1,
    text: {
        value: '&#129380;',
        alignToBottom: false
    },
    svgStyle: null
});

alt.on('updatePB', (bar,wert) => updateProgressbar(bar, wert));
function updateProgressbar(bar, wert)
{
    if(bar == 1)
    {
        healthbar.animate(wert);
    }
    if(bar == 2)
    {
        hungerbar.animate(wert);
    }
    if(bar == 3)
    {
        thirstbar.animate(wert);
    }
}