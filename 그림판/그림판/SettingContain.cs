using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace 그림판
{
    public class SettingContain : ISetPoints
    {
        #region 상속받은 것
        private List<Point> _points = new List<Point>();

        public List<Point> Points
        {
            get { return _points; }
            set { _points = value; }
        }
        #endregion

        private Rectangle _region = new Rectangle();
        //private List<Point> _childPoints = new List<Point>();

        // 선택된 도형 수정
        public SettingContain(Rectangle rt, List<Point> lp)
        {
            _region = rt;
            _points = lp;
        }

        public Rectangle Region
        {
            get { return _region; }
            set { _region = value; }
        }

        public List<Point> ChildPoints
        {
            get { return _points; }
            set { _points = value; }
        }
    }
}
