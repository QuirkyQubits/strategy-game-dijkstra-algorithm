using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    public class Stats
    {
        public int atk;
        public int hp;

        public Stats(int atk, int hp)
        {
            this.atk = atk;
            this.hp = hp;
        }

        public override bool Equals(Object obj)
        {
            Stats item = obj as Stats;

            return item != null && atk == item.atk && hp == item.hp;
        }
    }
}
