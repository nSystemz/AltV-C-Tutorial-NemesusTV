using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial
{
    public class Events : IScript
    {
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public void OnPlayerConnect(TPlayer.TPlayer tplayer, string reason)
        {
            Alt.Log($"Der Spieler {tplayer.Name} hat den Server betreten!");
            tplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
            tplayer.Model = (uint)PedModel.Business01AMM;
        }

        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public void OnPlayerDisconnect(TPlayer.TPlayer tplayer, string reason)
        {
            Alt.Log($"Spieler {tplayer.Name} hat den Server verlassen - Grund: {reason}!");
        }

        [ClientEvent("Event.Register")]
        public void OnPlayerRegister(TPlayer.TPlayer tplayer, String name, String password)
        {
            if (!Datenbank.IstAccountBereitsVorhanden(name))
            {
                if (!tplayer.Eingeloggt && name.Length > 3 && password.Length > 5)
                {
                    tplayer.SpielerName = name;
                    Datenbank.NeuenAccountErstellen(name, password);
                    tplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
                    tplayer.Model = (uint)PedModel.Business01AMM;
                    tplayer.Eingeloggt = true;
                    tplayer.Emit("CloseLoginHud");
                    tplayer.SendChatMessage("{00c900}Erfolgreich registriert!");
                }
            }
            else
            {
                tplayer.Emit("SendErrorMessage", "Es existiert bereits ein Account mit dem eingegebenen Namen!");
            }
        }

        [ClientEvent("Event.Login")]
        public void OnPlayerLogin(TPlayer.TPlayer tplayer, String name, String password)
        {
            if(Datenbank.IstAccountBereitsVorhanden(name))
            {
                if(!tplayer.Eingeloggt && name.Length > 3 && password.Length > 5)
                {
                    if(Datenbank.PasswortCheck(name, password))
                    {
                        tplayer.SpielerName = name;
                        Datenbank.AccountLaden(tplayer);
                        tplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
                        tplayer.Model = (uint)PedModel.Business01AMM;
                        tplayer.Eingeloggt = true;
                        tplayer.Emit("CloseLoginHud");
                        tplayer.SendChatMessage("{00c900}Erfolgreich eingeloggt!");
                    }
                    else
                    {
                        tplayer.Emit("SendErrorMessage", "Falsches Passwort eingeben oder ungültige Eingabe!");
                    }
                }
                else
                {
                    tplayer.Emit("SendErrorMessage", "Ungültige Eingaben, bitte korregieren!");
                }
            }
            else
            {
                tplayer.Emit("SendErrorMessage", "Es wurde kein Account mit dem Namen gefunden!");
            }
        }
    }
}
