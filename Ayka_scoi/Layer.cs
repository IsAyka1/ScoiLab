using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayka_scoi
{
    enum EOPER
    {
        No, Sum, Difference, Multy, Division, Min, Max
    }

    enum ECHANEL
    {
        RGB, RG, RB, GB, R, G, B
    }

    class Layer
    {
        public ECHANEL EChanel = 0;
        public EOPER EOper = 0;
        public uint Visible = 100;
        public Bitmap Img = null;

        public Layer(Bitmap img)
        {
            Img = img;
        }
    }

   
}
