/// <reference types ="@altv/types-client" />
/// <reference types ="@altv/types-natives" />


import * as alt from "alt-client"
import * as native from "natives"

alt.onServer('freezePlayer', (freeze) => {
    const lPlayer = alt.Player.local.scriptID;
    native.freezeEntityPosition(lPlayer, freeze);
})