using System;
using System.Collections.Generic;
using System.Text;

namespace FallingSandEngine
{
    public class Adjacency
    {
        public Cell[] Cells = new Cell[9];
        public Cell TL { get { return Cells[0]; } }
        public Cell TM { get { return Cells[1]; } }
        public Cell TR { get { return Cells[2]; } }

        public Cell ML { get { return Cells[3]; } }
        public Cell MM { get { return Cells[4]; } }
        public Cell MR { get { return Cells[5]; } }

        public Cell BL { get { return Cells[6]; } }
        public Cell BM { get { return Cells[7]; } }
        public Cell BR { get { return Cells[8]; } }
    }
}
