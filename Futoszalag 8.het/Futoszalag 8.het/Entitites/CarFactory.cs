using Futoszalag_8.het.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futoszalag_8.het.Entitites
{
    public class CarFactory:IToyFactory
    {
        public Toy CreateNew()
        {
            return new Car();
        }
    }
}
