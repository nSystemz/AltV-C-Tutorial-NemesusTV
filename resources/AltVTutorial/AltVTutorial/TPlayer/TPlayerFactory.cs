using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial.TPlayer
{
    public class TPlayerFactory : IEntityFactory<IPlayer>
    {
        public IPlayer Create(IServer server, IntPtr entityPointer, ushort id)
        {
            return new TPlayer(server, entityPointer, id);
        }
    }
}
