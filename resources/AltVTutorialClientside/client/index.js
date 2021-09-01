/// <reference types ="@altv/types-client" />
/// <reference types ="@altv/types-natives" />


import * as alt from "alt-client"
import * as native from "natives"

//Variables
let loginHud;
let guiHud;

//Commands
alt.onServer('freezePlayer', (freeze) => {
    const lPlayer = alt.Player.local.scriptID;
    native.freezeEntityPosition(lPlayer, freeze);
})

alt.onServer('CloseLoginHud', () => {
    alt.showCursor(false);
    alt.toggleGameControls(true);
    alt.toggleVoiceControls(true);

    if(loginHud)
    {
        loginHud.destroy();
    }
})

alt.onServer('SendErrorMessage', (text) => {
    loginHud.emit('ErrorMessage', text);
})

alt.on('connectionComplete', () => {
    guiHud = new alt.WebView("http://resource/gui/gui.html");

    loginHud = new alt.WebView("http://resource/login/login.html");
    loginHud.focus();

    alt.showCursor(true)
    alt.toggleGameControls(false)
    alt.toggleVoiceControls(false)

    loginHud.on('Auth.Login', (name, password) => {
        alt.emitServer('Event.Login', name, password);
    })

    loginHud.on('Auth.Register', (name, password) => {
        alt.emitServer('Event.Register', name, password);
    })
})

alt.onServer('sendNotification', (status, text) => {
    guiHud.emit('sendNotification', status, text);
})