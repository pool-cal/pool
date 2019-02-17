using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 그림판
{
    // 모양, 색상, 좌표, 굵기 객체 생성
    public class Setting //: ISetShape, ISetColor, ISetWidth, ISetPoints
    {
        private string _shape;
        private Color _color;
        private int _width;
        private List<Point> _points = new List<Point>();

        //Rectangle _region = new Rectangle();

        //생성자
        // 처음에 내가 원하는 객체만 챙기기 위해 (기본생성자로 냅두면 컴퓨터 안에서의 defalut값으로 이상한 모양이 튀어나올수있음)
        public Setting(string Initshape, Color Initcolor, int Initwidth, List<Point> Initpoints)
        {
            this._shape = Initshape;
            this._color = Initcolor;
            this._width = Initwidth;
            this._points = Initpoints;

            //this._region = InitRegion;


        }

        public string Shape
        {
            get { return _shape; }
            set { _shape = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public List<Point> Points
        {
            get { return _points; }
            set { _points = value; }
        }



        //public Rectangle Region
        //{
        //    get { return _region; }
        //    set { _region = value; }
        //}

 
    }

}
