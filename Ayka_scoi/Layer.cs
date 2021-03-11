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
        public uint id = 0;
        public ECHANEL EChanel = 0;
        public EOPER EOper = 0;
        public uint Visible = 100;
        public Bitmap Img = null;

        public Layer(Bitmap img, uint _id)
        {
            Img = img;
            id = _id;
        }
    }

   
}
