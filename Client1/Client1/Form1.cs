using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace Client1
{
    public partial class Form1 : Form
    {
        //초기도형설정
        private int _width = 2;
        private Color _color = Color.Black;
        private string _shape;

        private Point _start;
        private Point _end;

        private Point _startPoint;
        private Point _endPoint;
        private Point _moveStartPoint;
        private Point _moveEndPoint;

        private Point _convertMoveEndPoint;
        private Point _convertMoveStartPoint;

        private bool _isClicked = false;
        private bool _isComplete = false;

        private Socket _mainSock;
        private IPAddress _ipAddress;

        // Setting 클래스 변수 선언 (도형 객체 담을 곳) 
        private List<Setting> _container_setting = new List<Setting>();

        private bool _isSelected = false;

        private string _selectKey = "";         // 선택키
        private int _pointKey_Server= 0;       // 서버 키 
        private int _pointKey_Client = 0;       // 클라이언트 키
       
        // 이동에 필요한 변수
        private Point _moveShapePoint = new Point();
        private List<Point> _moveShapeTemp = null;
        //private Rectangle _moveShapeRect = new Rectangle(0, 0, 0, 0);

        // Setting 객체에 Key를 부여하여 순서대로 저장(server, client 구분)
        private SortedList<string, Setting> _moveShapeSetting_Server = new SortedList<string, Setting>();
        private SortedList<string, Setting> _moveShapeSetting_Client = new SortedList<string, Setting>();

        // 좌표값 바뀌는 함수
        private Point[] ConvertPoints(Point pt1, Point pt2)
        {
            int max_X, max_Y, min_X, min_Y;
            Point re_pt1 = pt1;
            Point re_pt2 = pt2;

            min_X = re_pt1.X < re_pt2.X ? re_pt1.X : re_pt2.X;
            min_Y = re_pt1.Y < re_pt2.Y ? re_pt1.Y : re_pt2.Y;
            max_X = re_pt1.X > re_pt2.X ? re_pt1.X : re_pt2.X;
            max_Y = re_pt1.Y > re_pt2.Y ? re_pt1.Y : re_pt2.Y;

            Point[] conPoints = new Point[2];
            conPoints[0] = new Point(min_X, min_Y);
            conPoints[1] = new Point(max_X, max_Y);

            return conPoints;
        }

        // picturebox 캡쳐함수
        private void screen_capture()
        {
            Bitmap bitmap = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(PointToScreen(new Point(this.pictureBox1.Location.X, this.pictureBox1.Location.Y)), new Point(0, 0), this.pictureBox1.Size);
            bitmap.Save("_copy.jpg");  //비트맵에 캡쳐한 화면 저장
            pictureBox1.ImageLocation = "_copy.jpg";   //picturebox에 보여줌
        }

        public Form1()
        {
            InitializeComponent();
            ((Control)pictureBox1).AllowDrop = true;        //드래그앤드랍 되도록 설정
            this.DoubleBuffered = true;                     //그릴때 안끊기도록 
            btn_Colordialog.ForeColor = Color.Black;
            btn_Colordialog.BackColor = Color.Black;
            tbox_BackColor.BackColor = Color.Red;
            tbox_BackColor.Text = "OFF";
            _mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        // 폼 로드 (기본값)
        private void Form1_Load(object sender, EventArgs e)
        {
            tbox_Path.Text = Convert.ToString("");
            btn_Colordialog.BackColor = _color;
            tbox_Shape.Text = Convert.ToString("");
            tbox_Width.Text = Convert.ToString(unit.unit_width_normal);

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress address in host.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    _ipAddress = address;
                    break;
                }
            }
            if (_ipAddress == null)
                _ipAddress = IPAddress.Loopback;
            tbox_ServerIP.Text = _ipAddress.ToString();
        }

        // 'Connet' 버튼 클릭 시 서버 아이피와 연결 시도
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            // 통신 초기화
            _mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (_mainSock.Connected)
            {
                lbox_Content.Items.Add("이미 연결됨");
                return;
            }

            int port;

            if (!int.TryParse(tbox_Port.Text, out port))
            {
                lbox_Content.Items.Add("포트번호잘못 또는 입력x");
                tbox_Port.Focus();
                tbox_Port.SelectAll();
                return;
            }

            try
            {


                _mainSock.Connect(tbox_ServerIP.Text, port);
                
                lbox_Content.Items.Add("서버와 연결성공");
                lbox_Content.Items.Add("메시지를 입력하세요");
                tbox_BackColor.BackColor = Color.Green;
                tbox_BackColor.Text = "ON";

                _moveShapeSetting_Server.Clear(); //sortlist <string, Setting> server init() 

                // 연결완료, 서버에서 데이터 올 수 있도록 수신 대기
                AsyncObject obj = new AsyncObject(4096, _mainSock.LocalEndPoint);
                obj._workingSocket = _mainSock;
                _mainSock.BeginReceive(obj._buffer, 0, obj._bufferSize, 0, DataReceived, obj);
                
            }
            catch (Exception ex)
            {
                lbox_ChatLog.Items.Add(ex.Message.ToString());
                _moveShapeSetting_Server.Clear(); //sortlist <string, Setting> server init() 
                return;
            }
        }

        // 데이터 받는 함수
        private void DataReceived(IAsyncResult ar)
        {
            // BeginReceive에서 추가로 온 데이터를 변환
            // AsyncCallback 메소드로 전달된 객체 반환
            AsyncObject obj = (AsyncObject)ar.AsyncState;

            // 데이터 수신 끝
            try
            {

                int received = obj._workingSocket.EndReceive(ar);
                // 받은 데이터 없으면 비동기 연결끝
                if (received <= 0)
                {
                    obj._workingSocket.Close();
                    return;
                }
                
                // 텍스트로 변환
                // 서버가 보낼 떄 : "192.168.145.1 : 3000 % t % www"
                // 클라가 보낼 때 : "192.168.2.157 : port : t : mms"
                string text = Encoding.Default.GetString(obj._buffer);

                string[] token = text.Split('%');

                string[] port = token[0].Split(':');       //port or ip
                string check = token[1];
                string message = token[2];
                //Console.WriteLine(message);
                Console.WriteLine("dfdf");
                if (check.Equals("p") || check.Equals("p@"))
                {
                    Console.WriteLine(message);
                    MessageConverter(message, check, port[1]);
                }

                else if (check.Equals("g"))
                {
                    Image image = Image.FromFile(message);
                    this.pictureBox1.Image = image;
                    image = Image.FromFile(message, true);                  
                }
                else
                {
                    //  if (ip == "192.168.2.157")
                        if (port[1].Equals("3000"))
                        {
                            if (lbox_ChatLog.InvokeRequired)
                            {
                                lbox_ChatLog.Invoke(new Action(delegate()
                                {
                                    lbox_ChatLog.Items.Add("[server] : " + message);
                                }));
                            }
                            else
                                lbox_ChatLog.Items.Add("[server] : " + message);
                        }
                        else
                        {
                            if (lbox_ChatLog.InvokeRequired)
                            {
                                lbox_ChatLog.Invoke(new Action(delegate()
                                {
                                    lbox_ChatLog.Items.Add("[클라이언트] : " + message);
                                }));
                            }
                            else
                                lbox_ChatLog.Items.Add("[클라이언트] : " + message);
                        }

                }           
                // 데이터 받고 버퍼 비움
                obj.ClearBuffer();
                // 수신 대기
                obj._workingSocket.BeginReceive(obj._buffer, 0, 4096, 0, DataReceived, obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine("chekc");
                ex.Message.ToString();
            }

        }

        private void MessageCheck(string[] command, string message, string port)
        {
            string curShape;
            Color curColor;
            int curWidth;
            List<Point> curPoints = new List<Point>();

            // SHAPE:RECT
            // COLOR:255,255,0,0
            // WIDTH:2
            // POINTS:10,10,100,100
            // STRKEY:_pointkey
            string[] strShape = command[0].Split(':');
            string[] strColor = command[1].Split(':');
            string[] strWidth = command[2].Split(':');
            string[] strPoints = command[3].Split(':');
            string[] strKey = command[4].Split(':');

            switch (strShape[1])
            {
                case "RECT":
                    curShape = "RECT";
                    break;
                case "CIRCLE":
                    curShape = "CIRCLE";
                    break;
                case "LINE":
                    curShape = "LINE";
                    break;
                default:
                    curShape = "NONE";
                    break;

            }

            int colorA = int.Parse(strColor[1].Split(',')[0]);
            int colorR = int.Parse(strColor[1].Split(',')[1]);
            int colorG = int.Parse(strColor[1].Split(',')[2]);
            int colorB = int.Parse(strColor[1].Split(',')[3]);

            curColor = Color.FromArgb(colorA, colorR, colorG, colorB);

            int width = int.Parse(strWidth[1]);

            curWidth = width;

            int startX = int.Parse(strPoints[1].Split(',')[0]);
            int startY = int.Parse(strPoints[1].Split(',')[1]);
            int endX = int.Parse(strPoints[1].Split(',')[2]);
            int endY = int.Parse(strPoints[1].Split(',')[3]);

            Point[] conPoints = new Point[2];
            conPoints[0] = new Point(startX, startY);
            conPoints[1] = new Point(endX, endY);

            curPoints.Add(conPoints[0]);
            curPoints.Add(conPoints[1]);
            // 객체 각각 잘라낸 후, 생성자에 객체 넣어주고 쌓이는 객체 계속 뿌려줌
            Setting setting = new Setting(curShape, curColor, curWidth, curPoints);
            _container_setting.Add(setting);
            _pointKey_Server = Convert.ToInt32(strKey[1]); // server key 갱신
            Console.WriteLine("RECEIVED STR KEY: " + _pointKey_Server);
            _moveShapeSetting_Server.Add(_pointKey_Server.ToString(), setting);
            _pointKey_Server++;

            if (pictureBox1.InvokeRequired)
            {
                pictureBox1.Invoke(new Action(delegate()
                {
                    pictureBox1.Refresh();
                }));
            }
            else
            {
                pictureBox1.Refresh();
            }

            if (port.Equals("3000"))
            {
                if (lbox_Content.InvokeRequired)
                {
                    lbox_Content.Invoke(new Action(delegate()
                    {
                        lbox_Content.Items.Add("[server] : " + message);
                    }));
                }
                else
                    lbox_Content.Items.Add("[server] : " + message);
            }
            else
            {
                if (lbox_Content.InvokeRequired)
                {
                    lbox_Content.Invoke(new Action(delegate()
                    {
                        lbox_Content.Items.Add("[클라이언트] : " + message);
                    }));
                }
                else
                    lbox_Content.Items.Add("[클라이언트] : " + message);
            }
        }

        // 받아온 byte형태의 객체들 string으로 바꿔주고 다시 객체 저장해주는 함수 (서버, 클라 구분위해 port 파라미터 추가 - 아이디만들면 ID로 바꾸기)
        // 현재는 ★도형그리기 전용 (사진은 원래 필요없고, 채팅은 DataReceive에서 처리 가능)★
        private void MessageConverter(string message, string check, string port)
        {
            // p @ SHAPE:RECT @ COLOR:255,255,0,0 @ WIDTH:2 @ POINTS:10,10,100,100 @ STRKEY:_pointkey

            string[] command;
            string[] message_copy;

                // 한 번에 받았을 때
            if (message.Contains('+'))
            {
                message_copy = message.Split('+');
                for (int i = 0; i < message_copy.Count() - 1; i++)
                {
                    string cp = message_copy[i];
                    command = cp.Split('@');
                    if (check.Equals("p") || check.Equals("p@"))
                    {
                        MessageCheck(command, message, port);
                    }
                }
            }
                
            
            else
            {
                command = message.Split('@');

                if (check.Equals("p") || check.Equals("p@"))
                {
                    MessageCheck(command, message, port);
                }
            }

        }

        // 서버로 객체 보내주는 메시지 (텍스트, 그림 구분)
        // 서버가 보낼 때 : "192.168.2.157 % mms"
        // 클라가 보낼 때 : "192.168.2.157 : port % t % mms"
        private void SendCommand(string command, int check)
        {
            if (!_mainSock.IsBound)
            {
                return;
            }

            if (_mainSock.Connected)
            {
                string _check = null;

                if (check == 0)
                {
                    _check = "t@";
                }
                else if (check == 1)
                {
                    _check = "p@";
                }
                else if (check == 2)
                {
                    _check = "g@";
                }

                IPEndPoint ip = (IPEndPoint)_mainSock.RemoteEndPoint;
                string port = ip.Port.ToString();
                string localIP = _mainSock.LocalEndPoint.ToString();

                byte[] bytes = Encoding.Default.GetBytes(localIP + "%" + _check + "%" + command);
                //byte[] bytes = Encoding.Default.GetBytes(localIP + "%" + _check + command + "%");
                _mainSock.Send(bytes);
            }
        }

        // 'Stop' 버튼 클릭 시 연결 해제 (통로를 아예 없앤다)
        private void btn_ServerStop_Click(object sender, EventArgs e)
        {
            try
            {
                _mainSock.Close();
                lbox_Content.Items.Add("연결 해제됨");
                tbox_BackColor.BackColor = Color.Red;
                tbox_BackColor.Text = "OFF";

            }
            catch (Exception gg)
            {
                Console.WriteLine(gg.Message);
            }

        }

        // '불러오기' 버튼 클릭
        private void btn_Path_Click(object sender, EventArgs e)
        {
            tbox_Path.Clear();

            string openstrFilename;

            openFileDialog1.Title = "이미지가져오기";
            openFileDialog1.Filter = "All Files(*.*)|*.*|Bitmap File(*.bmp)|*.bmp|JPEG File(*.jpg)|*.jpg";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openstrFilename = openFileDialog1.FileName;
                Image image = Image.FromFile(openstrFilename);
                this.pictureBox1.Image = image;

                image = Image.FromFile(openstrFilename, true);
                tbox_Path.Text = openstrFilename.Substring(openstrFilename.IndexOf(""));
                string message = tbox_Path.Text;
                SendCommand(message, 2);
            }
        }

        // '저장하기' 버튼 클릭
        private void btn_save_Click(object sender, EventArgs e)
        {
            screen_capture();   //캡쳐하기

            saveFileDialog1.Title = "저장하기";
            saveFileDialog1.Filter = "Jpeg |*.jpg|Bitmap |*.bmp|Gif |*.gif|All Files(*.*)|*.*";
            saveFileDialog1.DefaultExt = "jpg";
            saveFileDialog1.FilterIndex = 0;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);
            }
        }

        // '색상변경' 버튼 클릭 
        private void btn_Colordialog_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = false;
            colorDialog1.ShowHelp = true;
            colorDialog1.Color = btn_Colordialog.BackColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_Colordialog.ForeColor = colorDialog1.Color;
                btn_Colordialog.BackColor = colorDialog1.Color;
                _color = colorDialog1.Color;
            }
            // Refresh();
        }

        // 드래그앤드롭 - 이미지 끌어왓을 때
        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            var imageName = (string[])e.Data.GetData(DataFormats.FileDrop);

            pictureBox1.Image = Image.FromFile(imageName[0], true);
            Console.WriteLine(imageName[0]);
            tbox_Path.Text = imageName[0].Substring(imageName[0].IndexOf(""));
            string message = tbox_Path.Text;
            SendCommand(message, 2);
        }

        // 드래그앤드롭 - 이미지 들어감
        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        // 그리기이벤트 - 시작좌표 설정 (마우스 눌렀을 때)
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _startPoint = e.Location;
                _moveStartPoint = e.Location;
                _moveEndPoint = e.Location;

                Point[] conPoints = ConvertPoints(_moveStartPoint, _moveEndPoint);
                _convertMoveStartPoint = conPoints[0];
                _convertMoveEndPoint = conPoints[1];

                _isClicked = true;
            }

            else if (e.Button == MouseButtons.Right)
            {
                _isClicked = false;

                _moveShapePoint = e.Location;
                
                Rectangle _moveShapeRect = new Rectangle(0, 0, 0, 0);   // 초기화

                if (_mainSock.Connected)
                {
                    foreach (KeyValuePair<string, Setting> kv in _moveShapeSetting_Server)
                    {
                        _moveShapeRect.X = kv.Value.Points[0].X;
                        _moveShapeRect.Y = kv.Value.Points[0].Y;
                        _moveShapeRect.Width = kv.Value.Points[1].X - kv.Value.Points[0].X;
                        _moveShapeRect.Height = kv.Value.Points[1].Y - kv.Value.Points[0].Y;

                        // 도형 하나씩만 캐치할 수 있도록 (평면상에 스택처럼 순서대로 쌓여있는 도형같은 형태) - 안하면 전부 선택됨
                        _isSelected = _moveShapeRect.Contains(_moveShapePoint);

                        if (_isSelected)
                        {
                            Console.WriteLine(kv.Key);
                            _selectKey = kv.Key;
                            _moveShapeTemp = kv.Value.Points;
                            //_moveShapeRect = kv.Value.Region;
                            break;
                        }
                    }
                } 

                else
                {
                    foreach (KeyValuePair<string, Setting> kv in _moveShapeSetting_Client)
                    {
                        _moveShapeRect.X = kv.Value.Points[0].X;
                        _moveShapeRect.Y = kv.Value.Points[0].Y;
                        _moveShapeRect.Width = kv.Value.Points[1].X - kv.Value.Points[0].X;
                        _moveShapeRect.Height = kv.Value.Points[1].Y - kv.Value.Points[0].Y;

                        // 도형 하나씩만 캐치할 수 있도록 (평면상에 스택처럼 순서대로 쌓여있는 도형같은 형태) - 안하면 전부 선택됨
                        _isSelected = _moveShapeRect.Contains(_moveShapePoint);

                        if (_isSelected)
                        {
                            Console.WriteLine(kv.Key);
                            _selectKey = kv.Key;
                            _moveShapeTemp = kv.Value.Points;
                            //_moveShapeRect = kv.Value.Region;
                            break;
                        }
                    }
                }

            }

        }

        // 그리기이벤트 - MouseMove하면 크기변함
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // 도형 그릴 때
            if (_isClicked && _convertMoveStartPoint != null)
            {
                _moveEndPoint = e.Location;

                Point[] conPoints = ConvertPoints(_moveStartPoint, _moveEndPoint);
                _convertMoveStartPoint = conPoints[0];
                _convertMoveEndPoint = conPoints[1];

                pictureBox1.Refresh();
            }

            // 도형 이동할 때
            else if (_isSelected)
            {
                //_tmp = _Points[_SelectKey].Points;
                Point moveTempPoint;

                Rectangle _moveShapeRect = new Rectangle(0, 0, 0, 0);   // 초기화

                for (int i = 0; i < _moveShapeTemp.Count; i++)
                {
                    // 좌표 이동
                    moveTempPoint = _moveShapeTemp[i];

                    moveTempPoint.X += e.Location.X - _moveShapePoint.X;
                    moveTempPoint.Y += e.Location.Y - _moveShapePoint.Y;

                    _moveShapeTemp[i] = moveTempPoint;
                }

                //_moveShapeSetting[_selectKey].Points = _moveShapeTemp;

                // 영역(Rectangle) 이동
                //_rct = _Points[_SelectKey].Region;

                _moveShapeRect.X += e.Location.X - _moveShapePoint.X;
                _moveShapeRect.Y += e.Location.Y - _moveShapePoint.Y;

                _moveShapeRect.X = _moveShapeTemp[0].X;
                _moveShapeRect.Y = _moveShapeTemp[0].Y;
                _moveShapeRect.Width = _moveShapeTemp[1].X - _moveShapeTemp[0].X;
                _moveShapeRect.Height = _moveShapeTemp[1].Y - _moveShapeTemp[0].Y;

                //_moveShapeSetting[_selectKey].Region = _moveShapeRect;

                using (Graphics g = Graphics.FromHwnd(this.Handle))
                {
                    // 이동시 잔상 제거 및 다시 그리기.
                    pictureBox1.Invalidate(Rectangle.Truncate(new Rectangle(_moveShapeRect.X - 100, _moveShapeRect.Y - 100, _moveShapeRect.Width + 200, _moveShapeRect.Height + 200)), true);
                }

                _moveShapePoint = e.Location;
            }   
        }

        // 그리기 이벤트 - MouseUp하면 포인트 가져와서 시작포인트와 비교, 차이값으로 도형지정
        // 객체 하나씩 담아서 SendMessage함수로 보내줌 (서버와 통신)
        // 도형 객체도 Setting클래스에서 받아와서 보내줌
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isClicked)
            {
                _endPoint = e.Location;

                Point[] pts = ConvertPoints(_startPoint, _endPoint);

                // 도형 객체 받아서 저장
                Setting setting = new Setting(_shape, _color, _width, pts.ToList());
                _container_setting.Add(setting);

                if (_mainSock.Connected)
                {

                    string message = "";
                    string strPoint = string.Format("POINTS:{0},{1},{2},{3}@", pts[0].X, pts[0].Y, pts[1].X, pts[1].Y);
                    string strShpae = string.Format("SHAPE:{0}@", _shape);
                    string strColor = string.Format("COLOR:{0},{1},{2},{3}@", _color.A, _color.R, _color.G, _color.B);
                    string strWidth = string.Format("WIDTH:{0}@", _width);
                    string strKey = string.Format("STRKEY:{0}@", _pointKey_Server);

                    message += strShpae;
                    message += strColor;
                    message += strWidth;
                    message += strPoint;
                    message += strKey;

                    Console.WriteLine(_pointKey_Server);
                    // SHAPE:RECT@COLOR:255,255,0,0@WIDTH:2@POINTS:10,10,100,100@STRKEY:_pointKey
                    SendCommand(message, 1);

                    _moveShapeSetting_Server.Add((_pointKey_Server++).ToString(), setting);     // 여기 에러
                }

                else
                {
                    _moveShapeSetting_Client.Add((_pointKey_Client++).ToString(), setting);
                }

                // 해제
                _isClicked = false;
                pictureBox1.Refresh();
            }

            if (_isSelected)
            {
                _isSelected = false;
                _moveShapeTemp = null;
            }
        }

        // 도형객체불러오기
        private void DrawLoad(Graphics g)
        {
            foreach (Setting setting in _container_setting)
            {
                //get함수
                string shape = setting.Shape;
                Color color = setting.Color;
                int width = setting.Width;
                List<Point> Points = setting.Points;

                DrawCurrent(g, color, width, shape, Points[0], Points[1]);
            }
        }

        // 도형현재설정값
        private void DrawCurrent(Graphics g, Color color, int width, string shape, Point start, Point end)
        {
            Pen pen = new Pen(color, width);
            Size size = new Size(end.X - start.X, end.Y - start.Y);

            switch (shape)
            {
                case "RECT":
                    g.DrawRectangle(pen, new Rectangle(start, size));
                    break;
                case "LINE":
                    g.DrawLine(pen, start, end);
                    break;
                case "CIRCLE":
                    g.DrawEllipse(pen, new Rectangle(start, size));
                    break;
            }
            pen.Dispose();
        }

        // 'picturBox에 도형그리기'이벤트
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            DrawLoad(e.Graphics);
            DrawCurrent(e.Graphics, _color, _width, _shape, _start, _end);
            // e.Graphics.DrawRectangle(Pens.RoyalBlue, Rectangle.Truncate(_moveShapeRect)); // 

            _start = _convertMoveStartPoint;
            _end = _convertMoveEndPoint;

        }

        // 채팅 보내기 버튼
        private void btn_Send_Click(object sender, EventArgs e)
        {
            // 보낼 텍스트
            string tts = tbox_Chat.Text.Trim();

            if (string.IsNullOrEmpty(tts))
                return;

            SendCommand(tts, 0);

            lbox_ChatLog.Items.Add("[나] : " + tts); // _ipAddress.ToString()
            tbox_Chat.Clear();
        }

        // enter키로 채팅보내기
        private void tbox_Chat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btn_Send_Click(sender, e);
            }
        }

        #region 메뉴

        Unit unit = new Unit();

        private void rbtn_None_CheckedChanged(object sender, EventArgs e)
        {
            _shape = "NONE";
            tbox_Shape.Text = "";
        }

        private void rbtn_Line_CheckedChanged(object sender, EventArgs e)
        {
            _shape = "LINE";
            tbox_Shape.Text = unit.unit_Line;
        }

        private void rbtn_Rect_CheckedChanged(object sender, EventArgs e)
        {
            _shape = "RECT";
            tbox_Shape.Text = unit.unit_rect;
        }

        private void rbtn_Circle_CheckedChanged(object sender, EventArgs e)
        {
            _shape = "CIRCLE";
            tbox_Shape.Text = unit.unit_circle;
        }

        private void rbtn_normal_CheckedChanged(object sender, EventArgs e)
        {
            _width = 2;
            tbox_Width.Text = unit.unit_width_normal;
        }

        private void rbtn_thick_CheckedChanged(object sender, EventArgs e)
        {
            if (_width == 2 || _width == 10)
            {
                _width = 5;
            }
            tbox_Width.Text = unit.unit_width_thick;
        }

        private void rbtn_very_thick_CheckedChanged(object sender, EventArgs e)
        {
            if (_width == 2 || _width == 5)
            {
                _width = 10;
            }
            tbox_Width.Text = unit.unit_width_very_thick;
        }
        
        private void btn_Remove_Click(object sender, EventArgs e)
        {
                _container_setting.Clear();
                pictureBox1.Refresh();
        }
        #endregion

    }
}
