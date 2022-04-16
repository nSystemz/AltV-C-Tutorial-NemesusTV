using AltV.Net;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;

namespace AltVTutorial.GameEvents.Account
{
    internal class Register : IScript
    {

        [ClientEvent("Event.Register")]
        public void OnPlayerRegister(TPlayer.TPlayer player, string name, string password)
        {
            if (!player.LoggedIn) return; // Bereits einen Account? Abbruch!
            if (name.Length < 3) return; // Username weniger als 3 Zeichen? Abbruch!
            if (password.Length < 5) return; // Passwort zu kurz? Abbruch!

            Logic.Account AccountHandle = new Logic.Account(name);
            if (AccountHandle.DoesUserExist())
            {
                player.Emit("SendErrorMessage", "Es existiert bereits ein Account mit dem eingegebenen Namen!");
                return;
            }

            if (!AccountHandle.Register(password))
            {
                player.Emit("SendErrorMessage", "Es konnte aufgrund eines internen Fehlers, kein Account erstellt werden.");
                return;
            }

            // ToDo: Refactor

            player.Username = name;
            player.Model = (uint)PedModel.FreemodeMale01;
            player.Dimension = 0;
            player.LoggedIn = true;
            player.DBID = AccountHandle.LastInsertedId;


            if (!Utils.IsPlayerWhitelisted(player))
            {
                player.SendChatMessage("{#eb0e27}Du stehst nicht auf der Whitelist!");
                player.Kick($"Du stehst nicht auf der Whitelist - (Socialclubid: {player.SocialClubId})!");
                return;
            }

            player.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
            player.Emit("CloseLoginHud");
            Utils.UpdateMoneyHud(player, player.Money);
            player.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Healthbar, 1.0);
            player.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Hungerbar, 0.5);
            player.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Thirstbar, 0.3);
            player.SendChatMessage("{00c900}Erfolgreich registriert!");
            Alt.Emit("SaltyChat:EnablePlayer", player);


        }
    }
}
