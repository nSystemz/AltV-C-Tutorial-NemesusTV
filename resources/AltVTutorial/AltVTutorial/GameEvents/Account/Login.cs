using AltV.Net;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using System;

namespace AltVTutorial.GameEvents.Account
{
    internal class Login
    {
        [ClientEvent("Event.Login")]
        public void OnPlayerLogin(TPlayer.TPlayer tplayer, String name, String password)
        {
            if (tplayer.LoggedIn) return; // Bereits eingeloggt? Abbruch!
            if (name.Length < 3) return; // Username weniger als 3 Zeichen? Abbruch!
            if (password.Length < 5) return; // Passwort zu kurz? Abbruch!

            Logic.Account AccountHandle = new Logic.Account(name);
            if (!AccountHandle.DoesUserExist())
            {
                tplayer.Emit("SendErrorMessage", "Es wurde kein Account mit dem Namen gefunden!");
                return;
            }

            if (Datenbank.PasswortCheck(name, password))
            {
                tplayer.Username = name;
                Datenbank.AccountLaden(tplayer);
                if (tplayer.positions[0] != 0.0 && tplayer.positions[1] != 0.0 && tplayer.positions[2] != 0.0)
                {
                    tplayer.Spawn(new AltV.Net.Data.Position(tplayer.positions[0], tplayer.positions[1], tplayer.positions[2]), 0);
                    tplayer.Rotation = new AltV.Net.Data.Rotation(0, 0, tplayer.positions[3]);
                }
                else
                {
                    tplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
                }
                tplayer.Model = (uint)PedModel.FreemodeMale01;
                tplayer.Emit("CloseLoginHud");
                tplayer.Health = 200;
                Utils.UpdateMoneyHud(tplayer, tplayer.Money);
                tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Healthbar, 1.0);
                tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Hungerbar, 0.5);
                tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Thirstbar, 0.3);
                if (!Utils.IsPlayerWhitelisted(tplayer))
                {
                    tplayer.SendChatMessage("{#eb0e27}Du stehst nicht auf der Whitelist!");
                    tplayer.Kick("Du stehst nicht auf der Whitelist");
                    return;
                }
                tplayer.LoggedIn = true;
                tplayer.SendChatMessage("{00c900}Erfolgreich eingeloggt!");
                Alt.Emit("SaltyChat:EnablePlayer", tplayer);
            }
            else
            {
                tplayer.Emit("SendErrorMessage", "Falsches Passwort eingeben oder ungültige Eingabe!");
            }
        }
    }

}
}
}
