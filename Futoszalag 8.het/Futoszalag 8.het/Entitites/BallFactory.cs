﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Futoszalag_8.het.Abstractions;

namespace Futoszalag_8.het.Entitites
{
    public class BallFactory:IToyFactory
    {
        public Toy CreateNew()
        {
            return new Ball();
        }
    }
}
