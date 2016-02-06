using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGJ.Damages
{
    public interface IAttacker
    {
        string AttackerId { get; }
        string AttackerName { get; }
    }
}
