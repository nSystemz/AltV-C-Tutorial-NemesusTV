using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
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
    }
}
