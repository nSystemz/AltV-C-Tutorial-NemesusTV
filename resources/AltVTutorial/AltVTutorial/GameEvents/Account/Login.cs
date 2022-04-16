using AltV.Net;
using AltV.Net.Enums;
using System;

namespace AltVTutorial.GameEvents.Account
{
    internal class Login
    {
        [ClientEvent("Event.Login")]
        public void OnPlayerLogin(TPlayer.TPlayer player, String name, String password)
        {
            if (player.LoggedIn) return; // Bereits eingeloggt? Abbruch!
            if (name.Length < 3) return; // Username weniger als 3 Zeichen? Abbruch!
            if (password.Length < 5) return; // Passwort zu kurz? Abbruch!

            Logic.Account AccountHandle = new Logic.Account(name);
            if (!AccountHandle.DoesUserExist())
            {
                player.Emit("SendErrorMessage", "Es wurde kein Account mit dem Namen gefunden!");
                return;
            }

            if (!AccountHandle.Login(password))
            {
                player.Emit("SendErrorMessage", "Falsches Passwort eingeben oder ungültige Eingabe!");
                return;
            };

            if (!Utils.IsPlayerWhitelisted(player))
            {
                player.Emit("SendErrorMessage", "Du stehst nicht auf der Whitelist!");
                player.Kick("Du stehst nicht auf der Whitelist");
                return;
            }

            player.DBID = AccountHandle.LastQueriedUser.Id;
            player.AdminLevel = AccountHandle.LastQueriedUser.AdminLevel;
            player.Money = AccountHandle.LastQueriedUser.Money;
            player.Fraktion = AccountHandle.LastQueriedUser.Fraktion;
            player.Payday = AccountHandle.LastQueriedUser.Payday;

            player.Model = (uint)PedModel.FreemodeMale01;
            player.Spawn(new AltV.Net.Data.Position(AccountHandle.LastQueriedUser.PosX, AccountHandle.LastQueriedUser.PosY, AccountHandle.LastQueriedUser.PosZ), 0);
            player.Rotation = new AltV.Net.Data.Rotation(0, 0, AccountHandle.LastQueriedUser.PosZ);

            player.Emit("CloseLoginHud");
            Utils.UpdateMoneyHud(player, player.Money);
            player.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Healthbar, 1.0);
            player.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Hungerbar, 0.5);
            player.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Thirstbar, 0.3);
            player.LoggedIn = true;
            player.Emit("SendMessage", "Erfolgreich eingeloggt!");
            Alt.Emit("SaltyChat:EnablePlayer", player);
        }
    }
}
