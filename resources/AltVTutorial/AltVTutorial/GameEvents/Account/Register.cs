using AltV.Net;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;

namespace AltVTutorial.GameEvents.Account
{
    internal class Register : IScript
    {

        [ClientEvent("Event.Register")]
        public void OnPlayerRegister(TPlayer.TPlayer tplayer, string name, string password)
        {
            if (!tplayer.Eingeloggt) return; // Bereits einen Account? Abbruch!
            if (name.Length < 3) return; // Username weniger als 3 Zeichen? Abbruch!
            if (password.Length < 5) return; // Passwort zu kurz? Abbruch!

            Logic.Account AccountHandle = new Logic.Account(name);
            if (AccountHandle.DoesUserExist())
            {
                tplayer.Emit("SendErrorMessage", "Es existiert bereits ein Account mit dem eingegebenen Namen!");
                return;
            }

            if (!AccountHandle.Register(password))
            {
                tplayer.Emit("SendErrorMessage", "Es konnte aufgrund eines internen Fehlers, kein Account erstellt werden.");
                return;
            }

            // ToDo: Refactor
            tplayer.SpielerName = name;
            tplayer.Model = (uint)PedModel.FreemodeMale01;
            tplayer.Dimension = 0;
            tplayer.Eingeloggt = true;
            tplayer.SpielerID = AccountHandle.LastInsertedId;

            tplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);

            tplayer.Emit("CloseLoginHud");
            if (!Utils.IsPlayerWhitelisted(tplayer))
            {
                tplayer.SendChatMessage("{#eb0e27}Du stehst nicht auf der Whitelist!");
                tplayer.Kick($"Du stehst nicht auf der Whitelist - (Socialclubid: {tplayer.SocialClubId})!");
                return;
            }
            Utils.UpdateMoneyHud(tplayer, tplayer.Geld);
            tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Healthbar, 1.0);
            tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Hungerbar, 0.5);
            tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Thirstbar, 0.3);
            tplayer.SendChatMessage("{00c900}Erfolgreich registriert!");
            Alt.Emit("SaltyChat:EnablePlayer", tplayer);


        }
    }
}
