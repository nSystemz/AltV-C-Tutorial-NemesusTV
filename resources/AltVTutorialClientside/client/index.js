/// <reference types ="@altv/types-client" />
/// <reference types ="@altv/types-natives" />
import * as alt from "alt-client"
import * as native from "natives"
import * as NativeUI from 'includes/NativeUIMenu/NativeUI.mjs';

//Variables
let loginHud;
let guiHud;
let lockHud;
let invHud;
let charHud;
let mdcHud;
let bodyCam;
let bodyCamStart;
let bodyCamSet = -1;

let showInv = false;
let mdcShow = false;

let funmodus = false;
let fpsBoost = false;

let cardealer = [];

const DISCORD_APP_ID = '';

const nametags = new Map();

//Nametags
alt.onServer('addToNametag', (playerId, playerName) => {
    nametags.set(playerId, playerName);
});

alt.onServer('removeFromNametag', (playerId) => {
    nametags.remove(playerId);
});

//Cardealer
alt.onServer('createCardealerText', (modelname, price, posx, posy, posz) => {
    let text = `Das Fahrzeug ${modelname} steht für ${price}$ zum Verkauf!`;
    let cardealerObj = {
        text: text,
        price: price,
        posx: posx,
        posy: posy,
        posz: posz
    }
    cardealer.push(cardealerObj);
});

//Commands
alt.onServer('freezePlayer', (freeze) => {
    const lPlayer = alt.Player.local.scriptID;
    native.freezeEntityPosition(lPlayer, freeze);
    alt.showCursor(false);
})

//Login/Register
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
    loadBlips();
    loadPeds();

    alt.toggleGameControls(false)
    alt.toggleVoiceControls(false)

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

    //getOAuth2Token();
})

async function getOAuth2Token()
{
    try {
        const token = await alt.Discord.requestOAuth2Token(DISCORD_APP_ID);
        alt.emitServer('Event.OAuth2Request', token);
    }
    catch (e)
    {
        alt.logError(e);
    }
}

//UpdateMoneyHud
alt.onServer('updateMoneyHud', (money) => {
    guiHud.emit('updateMoneyHud', money);
})

//Notifications
alt.onServer('sendNotification', (status, text) => {
    guiHud.emit('sendNotification', status, text);
})

//Player Huds
alt.onServer('updatePB', (bar, wert) => {
    guiHud.emit('updatePB', bar, wert);
})

//Garagensystem
alt.onServer('createGaragen', (json) => {
    let obj = JSON.parse(json);
    obj.forEach(function (garage) {
        if(!garage.created)
        {
            garage.created = true;
            createPed(garage.name, 4, parseFloat(garage.posx), parseFloat(garage.posy), parseFloat(garage.posz), parseFloat(garage.posa));
        }
    })
})

alt.onServer('setFunmodus', () => {
    funmodus = !funmodus;
})

//FPS Booster
alt.onServer('fpsBoost', () => {
    if(fpsBoost == false)
    {
        fpsBoost = true;
        native.ropeDrawShadowEnabled(false)
        native.cascadeShadowsClearShadowSampleType()
        native.cascadeShadowsSetAircraftMode(false)
        native.cascadeShadowsEnableEntityTracker(true)
        native.cascadeShadowsSetDynamicDepthMode(false)
        native.cascadeShadowsSetEntityTrackerScale(0.0)
        native.cascadeShadowsSetDynamicDepthValue(0.0)
        native.cascadeShadowsSetCascadeBoundsScale(0.0)
        native.setFlashLightFadeDistance(0.0)
        native.setLightsCutoffDistanceTweak(0.0)
    }
    else
    {
        fpsBoost = false;
        native.ropeDrawShadowEnabled(true)
        native.cascadeShadowsSetAircraftMode(true)
        native.cascadeShadowsEnableEntityTracker(false)
        native.cascadeShadowsSetDynamicDepthMode(true)
        native.cascadeShadowsSetEntityTrackerScale(5.0)
        native.cascadeShadowsSetDynamicDepthValue(5.0)
        native.cascadeShadowsSetCascadeBoundsScale(5.0)
        native.setFlashLightFadeDistance(10.0)
        native.setLightsCutoffDistanceTweak(10.0)
        native.setArtificialLightsState(false)
    }
})

//Blips
function loadBlips()
{
    createBlip(-427.85934, 1115.0637, 326.76343,8,29,1.0,false,"Zivispawn");
}

function createBlip(x,y,z,sprite,color,scale=1.0,shortRange=false,name="")
{
    const tempBlip = new alt.PointBlip(x,y,z);

    tempBlip.sprite = sprite;
    tempBlip.color = color;
    tempBlip.scale = scale;
    tempBlip.shortRange = shortRange;
    if(name.length > 0)
    tempBlip.name = name;
}

//Peds
function loadPeds()
{
    createPed('u_m_m_aldinapoli', 4, -427.85934, 1115.0637, 326.76343, 0.0);
}

async function createPed(hash,pedtype,x,y,z,a)
{
    const modelHash = alt.hash(hash);
    alt.loadModel(modelHash);
    await alt.Utils.wait(100);
    return native.createPed(pedtype, modelHash, x, y, z, a, false, false);
}

//DrawText3d
export function drawText3d(
    msg,
    x,
    y,
    z,
    scale,
    fontType,
    r,
    g,
    b,
    a,
    useOutline = true,
    useDropShadow = true,
    layer = 0
) {
    let hex = msg.match('{.*}');
    if (hex) {
        const rgb = hexToRgb(hex[0].replace('{', '').replace('}', ''));
        r = rgb[0];
        g = rgb[1];
        b = rgb[2];
        msg = msg.replace(hex[0], '');
    }

    native.setDrawOrigin(x, y, z, 0);
    native.beginTextCommandDisplayText('STRING');
    native.addTextComponentSubstringPlayerName(msg);
    native.setTextFont(fontType);
    native.setTextScale(1, scale);
    native.setTextWrap(0.0, 1.0);
    native.setTextCentre(true);
    native.setTextColour(r, g, b, a);

    if (useOutline) {
        native.setTextOutline();
    }

    if (useDropShadow) {
        native.setTextDropShadow();
    }

    native.endTextCommandDisplayText(0, 0, 0);
    native.clearDrawOrigin();
}

//DrawText2D
function drawText2d( 
    msg,
    x,
    y,
    scale,
    fontType,
    r,
    g,
    b,
    a,
    useOutline = true,
    useDropShadow = true,
    layer = 0,
    align = 0
 ) {
    let hex = msg.match('{.*}');
    if (hex) {
        const rgb = hexToRgb(hex[0].replace('{', '').replace('}', ''));
        r = rgb[0];
        g = rgb[1];
        b = rgb[2];
        msg = msg.replace(hex[0], '');
    }
 
    native.beginTextCommandDisplayText('STRING');
    native.addTextComponentSubstringPlayerName(msg);
    native.setTextFont(fontType);
    native.setTextScale(1, scale);
    native.setTextWrap(0.0, 1.0);
    native.setTextCentre(true);
    native.setTextColour(r, g, b, a);
    native.setTextJustification(align);
 
    if (useOutline) {
        native.setTextOutline();
    }
 
    if (useDropShadow) {
        native.setTextDropShadow();
    }
 
    native.endTextCommandDisplayText(x, y, 0);
}

//GetCameraOffsetX
function getCameraOffsetX(posx, angle, dist)
{
    angle = angle * 0.0174533;
    return posx + dist * Math.cos(angle);
}

//GetCameraOffsetY
function getCameraOffsetY(posy, angle, dist)
{
    angle = angle * 0.0174533;
    return posy + dist * Math.sin(angle);
}

//Speedometer
function getSpeedColor(kmh) {
    if(kmh < 65)
        return "~g~";
    if(kmh >= 65 && kmh < 125)
        return "~y~";
    if(kmh >= 125)
        return "~r~";
}

//Native UI Vehiclespawner
alt.onServer('nativeUITest', () => {
    const ui = new NativeUI.Menu("Fahrzeug Spawner", "Spawne dir ein Fahrzeug", new NativeUI.Point(250,250));
    ui.Open();

    ui.AddItem(new NativeUI.UIMenuListItem(
        "Fahrzeug",
        "Fahrzeugbeschreibung",
        new NativeUI.ItemsCollection(["Kein Fahrzeug","Sultan","Infernus"])
    ));

    ui.ItemSelect.on(item => {
        if(item instanceof NativeUI.UIMenuListItem) {
            alt.emitServer('Event.SpawnVehicle', item.SelectedItem.DisplayText);
        }
    });
})

alt.everyTick(() => {
    const lPlayer = alt.Player.local;
    let vehicle = lPlayer.vehicle;
    const players = alt.Player.all;

    if(vehicle)
    {
        let speed = vehicle.speed*3.6;
        speed = Math.round(speed);
        drawText2d(`${getSpeedColor(speed)}${speed} KMH`,0.45,0.91,1.5,2,255,255,255,255,true);
    }
    //2D Texte
    drawText2d('Nemesus.de', 0.5, 0.005, 0.5, 0, 255, 255, 255, 255);

    let getStreetHash = native.getStreetNameAtCoord(alt.Player.local.pos.x, alt.Player.local.pos.y, alt.Player.local.pos.z, 0, 0);
    let streetName = native.getStreetNameFromHashKey(getStreetHash[1]);
    let zone = native.getFilenameForAudioConversation(native.getNameOfZone(alt.Player.local.pos.x, alt.Player.local.pos.y, alt.Player.local.pos.z));

    drawText2d(`${streetName}\n${zone}`, 0.215, 0.925, 0.5, 4, 244, 210, 66, 255);

    //3D Texte
    //const playerPos = { ...alt.Player.local.pos };
    //drawText3d('Nemesus.de', playerPos.x, playerPos.y, playerPos.z, 0.5, 4, 255, 255, 255, 255, true, true);
    drawText3d('Willkommen auf dem NemesusTV Tutorial Server', -424.85275, 1116.712, 326.76343, 0.7, 4, 255, 255, 255, 255);

    //Cardealer
    for(let i= 0; i < cardealer.length; i++)
    {
        drawText3d(cardealer[i].text, cardealer[i].posx, cardealer[i].posy, cardealer[i].posz+0.5, 0.5, 4, 255, 255, 255, 255, true, true);
    }
    
    //Nametag
    for(let i = 0; i < players.length; i++)
    {
        const player = players[i];
        if(player && player !== lPlayer)
        {
            const [x, y, z] = game.getEntityCoords(player.scriptID, true);
            const playerId = player.id;

            if(nametags.has(playerId))
            {
                const playerName = nametags.get(playerId);
                const distance = game.getDistanceBetweenCoords(x, y, z, lPlayer.pos.x, lPlayer.pos.y, lPlayer.pos.z, true);

                if(distance < 45.0) {
                    drawText3d(playerName, x, y, z+1.2, 0.4, 0, 255, 255, 255, 255, true);
                }
            }
        }
    }
});

//Charcreator
alt.onServer('showCharCreator', () => {
    const lPlayer = alt.Player.local;

    charHud = new alt.WebView("http://resource/charcreator/index.html");
    charHud.focus();

    alt.showCursor(true);
    alt.toggleGameControls(false);
    alt.toggleVoiceControls(false);

    let camValues = {
        Angle: lPlayer.rot.z + 180,
        Dist: 1,
        Height: 0.2
    };

    bodyCamStart = lPlayer.pos;

    bodyCam = native.createCamWithParams('DEFAULT_SCRIPTED_CAMERA', getCameraOffsetX(bodyCamStart.x, camValues.Angle, camValues.Dist), getCameraOffsetY(bodyCamStart.y, camValues.Angle, camValues.Dist), lPlayer.pos.z + camValues.Height, 0, 0, 0, 50, 0, 0);
    
    native.setCamActive(parseFloat(bodyCam), true);
    native.renderScriptCams(true, false, 500, true, false, 0);
    native.setCamAffectsAiming(parseFloat(bodyCam), false);

    alt.emit('setCharCreatorCamera', 3);

    charHud.on('characterCustomize1', (flag, data) => {
        let getData = JSON.parse(data);
        switch(flag)
        {
            case 'hair': {
                alt.emit('setCharCreatorCamera', 1);
                native.setPedComponentVariation(lPlayer, 2, parseInt(getData[0]), 0, 0);
                break;
            }
            case 'faceFeatures': {
                alt.emit('setCharCreatorCamera', 2);
                native.setPedMicroMorph(lPlayer, 0, parseFloat(data[0]));
                native.setPedMicroMorph(lPlayer, 1, parseFloat(data[1]));
                native.setPedMicroMorph(lPlayer, 2, parseFloat(data[2]));
                native.setPedMicroMorph(lPlayer, 3, parseFloat(data[3]));
                native.setPedMicroMorph(lPlayer, 4, parseFloat(data[4]));
                native.setPedMicroMorph(lPlayer, 5, parseFloat(data[5]));
                native.setPedMicroMorph(lPlayer, 6, parseFloat(data[6]));
                native.setPedMicroMorph(lPlayer, 7, parseFloat(data[7]));
                native.setPedMicroMorph(lPlayer, 8, parseFloat(data[8]));
                native.setPedMicroMorph(lPlayer, 9, parseFloat(data[9]));
                native.setPedMicroMorph(lPlayer, 10, parseFloat(data[10]));
                native.setPedMicroMorph(lPlayer, 11, parseFloat(data[11]));
                native.setPedMicroMorph(lPlayer, 12, parseFloat(data[12]));
                native.setPedMicroMorph(lPlayer, 13, parseFloat(data[13]));
                native.setPedMicroMorph(lPlayer, 14, parseFloat(data[14]));
                native.setPedMicroMorph(lPlayer, 15, parseFloat(data[15]));
                native.setPedMicroMorph(lPlayer, 16, parseFloat(data[16]));
                native.setPedMicroMorph(lPlayer, 17, parseFloat(data[17]));
                native.setPedMicroMorph(lPlayer, 18, parseFloat(data[18]));
                native.setPedMicroMorph(lPlayer, 19, parseFloat(data[19]));
                break;
            }
            case 'clothing': {
                alt.emit('setCharCreatorCamera', 0);
                native.setPedComponentVariation(lPlayer, 11, parseInt(data[0]), 0, 0);
                native.setPedComponentVariation(lPlayer, 3, parseInt(data[1]), 0, 0);
                native.setPedComponentVariation(lPlayer, 4, parseInt(data[2]), 0, 0);
                native.setPedComponentVariation(lPlayer, 6, parseInt(data[3]), 0, 0);
            }
        }
    })
});

alt.on('setCharCreatorCamera', (flag) => {
    if(bodyCamSet == flag) return;
    const lPlayer = alt.Player.local;

    let camera = {
        Angle: lPlayer.rot.z + 180,
        Dist: 1,
        Height: 0.2,
    }

    switch(flag)
    {
        case 0: //Torso
        {
            camera = {
                Angle: lPlayer.rot.z + 180,
                Dist: 2.5,
                Height: 0.2
            };
            break;
        }
        case 1: //Kopf
        {
            camera = {
                Angle: lPlayer.rot.z + 180,
                Dist: 0.6,
                Height: 0.7
            };
            break;
        }
        case 2: //Gesicht
        {
            camera = {
                Angle: lPlayer.rot.z + 180,
                Dist: 0.5,
                Height: 0.2
            };
            break;
        }
        case 3: //Torso
        {
            camera = {
                Angle: lPlayer.rot.z + 180,
                Dist: 1,
                Height: 0.2
            };
            break;
        }
    }
    bodyCamSet = flag;
    bodyCamStart = lPlayer.pos;
    native.setCamCoord(parseFloat(bodyCam), getCameraOffsetX(bodyCamStart.x, camera.Angle, camera.Dist), getCameraOffsetY(bodyCamStart.y, camera.Angle, camera.Dist), bodyCamStart.z + camera.Height);
    native.pointCamAtCoord(parseFloat(bodyCam), bodyCamStart.x, bodyCamStart.y, bodyCamStart.z + camera.Height);
});

//Lockpicking
alt.onServer('showLockpicking', () => {
    lockHud = new alt.WebView("http://resource/lockpicking/lockpicking.html");
    lockHud.focus();

    alt.showCursor(true)
    alt.toggleGameControls(false)
    alt.toggleVoiceControls(false)

    lockHud.on('successLockpicking', () => {
        alt.emitServer('Event.successLockpickingServer');

        if(lockHud)
        {
            lockHud.destroy();
        }

        alt.showCursor(false)
        alt.toggleGameControls(true)
        alt.toggleVoiceControls(true)
    })

    lockHud.on('failedLockpicking', () => {
        alt.emitServer('Event.failedLockpickingServer');

        if(lockHud)
        {
            lockHud.destroy();
        }

        alt.showCursor(false)
        alt.toggleGameControls(true)
        alt.toggleVoiceControls(true)
    })
})


//MDC
alt.onServer('showMDC', () => {
    if(mdcShow == false)
    {
        mdcHud = new alt.WebView("http://localhost:8080/");
        mdcHud.focus();
        mdcShow = true;

        alt.showCursor(true);
        alt.toggleGameControls(false);
        alt.toggleVoiceControls(false);
    }
    else
    {
        if(mdcHud)
        {
            mdcHud.destroy();
            mdcShow = false;

            alt.showCursor(false);
            alt.toggleGameControls(true);
            alt.toggleVoiceControls(true);
        }
    }
})

//UpdateMoneyHud
alt.onServer('PetFollowPlayer', (player, ped) => {
    native.freezeEntityPosition(ped, false);
    alt.taskFollowToOffsetOfEntity(ped, player, 2.5, 2.5, 2.5, 1.5, 1, 1.5, true);
})

//Tastendrücke
alt.on('keydown', (key) => {
    const lPlayer = alt.Player.local;
    //Motorsystem
    if(key == 77)
    {
        alt.emitServer('Event.startStopEngine');
    }
    //Funmodus
    let vehicle = lPlayer.vehicle;
    if(funmodus)
    {
        //Speedboost
        if(key == 17)
        {
            if(vehicle)
            {
                let velo = native.getEntityVelocity(vehicle);
                native.setEntityVelocity(vehicle, velo.x*2.25, velo.y*2.25, velo.z);
            }
        }
        //Jump
        if(key == 18)
        {
            if(vehicle)
            {
                let velo = native.getEntityVelocity(vehicle);
                native.setEntityVelocity(vehicle, velo.x+0.1, velo.y+0.1, velo.z+7.5);
            }
        }
    }
})

alt.on('keydown', (key) => {
    //Cursor verstecken
    if(key == 0x71) //F2
    {
        alt.showCursor(false);
        const lPlayer = alt.Player.local.scriptID;
        native.freezeEntityPosition(lPlayer, false);
    }
    //Inventar
    if(key == 73)
    {
        if(showInv == false)
        {
            invHud = new alt.WebView("http://localhost:8080/");
            invHud.focus();
            showInv = true;

            alt.showCursor(true);
            alt.toggleGameControls(false);
            alt.toggleVoiceControls(false);
        }
        else
        {
            if(invHud)
            {
                invHud.destroy();
                showInv = false;
                alt.showCursor(false);
                alt.toggleGameControls(true);
                alt.toggleVoiceControls(true);
            }
        }
    }
})