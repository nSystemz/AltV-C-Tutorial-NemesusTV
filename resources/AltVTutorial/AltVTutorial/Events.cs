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
        public void OnPlayerConnect(IPlayer iplayer, string reason)
        {
            Alt.Log($"Der Spieler {iplayer.Name} hat den Server betreten!");
            iplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
            iplayer.Model = (uint)PedModel.Business01AMM;
        }

        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public void OnPlayerDisconnect(IPlayer iplayer, string reason)
        {
            Alt.Log($"Spieler {iplayer.Name} hat den Server verlassen - Grund: {reason}!");
        }
    }
}
