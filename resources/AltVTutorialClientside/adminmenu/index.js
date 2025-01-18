/// <reference types ="@altv/types-client" />
import * as alt from "alt-client";
import * as NativeUI from "nativeui/nativeui.js";

let menu = null;

function createAdminMenu() {
    menu = new NativeUI.Menu("Adminmenu", "Verwalte den Spieler", new NativeUI.Point(50, 50));
    menu.Visible = false;

    alt.onServer('admin:showMenu', (players) => {
        menu.Clear();
        players.forEach(player => {
            let playerItem = new NativeUI.UIMenuItem(player.name, `Interaktionen für ${player.name}`);
            menu.AddItem(playerItem);
            menu.OnItemSelect.connect((selectedItem, index) => {
                if (selectedItem === playerItem) {
                    showPlayerActions(player.id);
                }
            });
        });
        menu.Visible = true;
    });
}

function showPlayerActions(playerId) {
    let playerMenu = new NativeUI.Menu("Spieleraktionen", "Aktionen für den Spieler", new NativeUI.Point(50, 50));
    playerMenu.Visible = true;

    let kickItem = new NativeUI.UIMenuItem("Kick", "Kickt den Spieler vom Server");
    let banItem = new NativeUI.UIMenuItem("Ban", "Bannt den Spieler vom Server");

    playerMenu.AddItem(kickItem);
    playerMenu.AddItem(banItem);

    playerMenu.OnItemSelect.connect((selectedItem, index) => {
        if (selectedItem === kickItem) {
            alt.emitServer('admin:kickPlayer', playerId);
        } else if (selectedItem === banItem) {
            alt.emitServer('admin:banPlayer', playerId);
        }
        playerMenu.Visible = false;
    });
}

createAdminMenu();

//Tastendrücke
alt.on('keydown', (key) => {
    if (key == 77) { // M Taste
        menu.Visible = !menu.Visible;
    }
})