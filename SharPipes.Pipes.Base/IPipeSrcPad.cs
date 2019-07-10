using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base
{
    public interface IPipeSrcPad
    {
        public IPipeSrc Parent
        {
            get;
        }


        public void Unlink();
    }
}
