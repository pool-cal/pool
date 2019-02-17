using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace 그림판
{
    public interface ISetPoints
    {
        List<Point> Points { get; set; }
    }
}
