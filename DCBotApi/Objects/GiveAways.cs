using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Objects
{
    internal class GiveAways
    {
        public GiveAway[] GiveAwaysCollection { get; set; }

        public List<GiveAway> GetList => this.GiveAwaysCollection.ToList();
    }
}
