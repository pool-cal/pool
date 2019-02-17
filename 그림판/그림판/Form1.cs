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

namespace 그림판
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

        private Socket _mainSock;
        private Socket _client;
        private IPAddress _ipAddress;
        private List<Socket> _connectedClients = new List<Socket>();

        // Setting 클래스 변수 선언 (도형 객체 담을 곳) 
        private List<Setting> _container_setting =new List<Setting>();

        private bool _isSelected = false;

        private string _selectKey = ""; // 선택키
        private int _pointKey = 1;      // 클래스 키

        // 이동에 필요한 변수
        private Point _moveShapePoint = new Point();
        private List<Point> _moveShapeTemp = null;

        // Setting 객체에 Key를 부여하여 순서대로 저장
        private SortedList<string, Setting> _moveShapeSetting = new SortedList<string, Setting>();

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
            for (int i = 0; i < host.AddressList.Length; i++)
            {
                if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    tbox_ServerIP.Text = host.AddressList[i].ToString();
                    break;
                }
            }  
        }

        // '서버실행' 버튼 눌렀을 때
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            int port;

            if (!int.TryParse(tbox_Port.Text, out port))
            {
                lbox_Content.Items.Add("포트번호잘못 또는 연결x");
                tbox_Port.Focus();
                tbox_Port.SelectAll();
                return;
            }
            // 서버에서 클라이언트 연결 요청 대기, 소켓 열어둠

            IPAddress.TryParse(tbox_ServerIP.Text, out _ipAddress); 
       
            IPEndPoint serverIP = new IPEndPoint(_ipAddress, port);

            // 통신 초기화
            _mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _connectedClients.Clear();

            if (!_mainSock.Connected)
            {
                _mainSock.Bind(serverIP);
                _mainSock.Listen(10);
                lbox_Content.Items.Add("클라이언트에서 접속해주세요");
                // 비동기로 클라이언트 연결 요청 받음
                _mainSock.BeginAccept(AcceptCallback, null);
            }        
        }

        // 콜백함수
        private void AcceptCallback(IAsyncResult ar)
        {
            // 클라이언트 연결 요청 수락
            try
            {
                _client = _mainSock.EndAccept(ar);
                // 또 다른 클라이언트 연결 대기
                _mainSock.BeginAccept(AcceptCallback, null);

                AsyncObject obj = new AsyncObject(4096);
                obj._workingSocket = _client;
                // 연결된 클라이언트를 리스트에 추가
                _connectedClients.Add(_client);
                // 처음 통신 시작 할 때, 모든 데이터를 clinet에게 넘겨준다.
                // 문자열이 아닌 객체로 보내는 방법으로 생각해보자.
                // 초기 연결!!!!
                if (_connectedClients.Contains(_client))
                {
                    string message = "";
                    if (_moveShapeSetting.Count >= 1)
                    {
                        foreach (KeyValuePair<string, Setting> kv in _moveShapeSetting)
                        {
                            List<Point> pts = kv.Value.Points;
                            String _shape = kv.Value.Shape;
                            Color _color = kv.Value.Color;
                            int _width = kv.Value.Width;
                            String _pointKey = kv.Key;
                            Console.WriteLine(_shape);
                            string strPoint = string.Format("POINTS:{0},{1},{2},{3}@", pts[0].X, pts[0].Y, pts[1].X, pts[1].Y);
                            string strShpae = string.Format("SHAPE:{0}@", _shape);
                            string strColor = string.Format("COLOR:{0},{1},{2},{3}@", _color.A, _color.R, _color.G, _color.B);
                            string strWidth = string.Format("WIDTH:{0}@", _width);
                            string strKey = string.Format("STR KEY:{0}", _pointKey);

                            message += strShpae;
                            message += strColor;
                            message += strWidth;
                            message += strPoint;
                            message += strKey;
                            message += '+';

                        }

                        // SHAPE:RECT@COLOR:255,255,0,0@WIDTH:2@POINTS:10,10,100,100@STR KEY:_pointKey
                        SendMessage(message, 12);
                    }
                   
                }



                if (lbox_Content.InvokeRequired)
                {
                    lbox_Content.Invoke(new Action(delegate()
                    {
                        lbox_Content.Items.Add("[클라이언트" + _client.RemoteEndPoint + "] : 연결");
                    }));
                }
                else
                {
                    lbox_Content.Items.Add("[클라이언트" + _client.RemoteEndPoint + "] : 연결");
                }
                // 클라이언트 데이터 받기
                _client.BeginReceive(obj._buffer, 0, 4096, 0, DataReceived, obj);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        // 데이터 받는 함수
        private void DataReceived(IAsyncResult ar)
        {
            // BeginRecvied에서 추가로 넘어온 데이터를 AysncObject클래스 형식으로 변환
            // AsyncCallback 메소드는 IAsyncResult 객체의 AsyncState 속성을 사용하여 원래의 소캣 객체 재생성
            // EndAccpet() 호출되면, IAsyncResult 객체를 통해 호출한 BeginAccept()과 대응한다
            // EndAccpet() 은 원격 클라이언트와 통신하는데 사용할 새로운 소켓 객체를 반환
            AsyncObject obj = (AsyncObject)ar.AsyncState;
            // 데이터 수신 끝내기
            if (_client.Connected)
            {
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
                    // "192.168.2.157 : port % t % mms"
                    string text = Encoding.Default.GetString(obj._buffer);

                    string[] token = text.Split('%');
                    string port = token[0];

                    string[] _port = token[0].Split(':');
                    string _portip = _port[1];

                    string check = token[1];
                    string message = token[2];

                    MessageConverter(message, check);


                    if (check.Equals("t@"))
                    {
                        if (lbox_ChatLog.InvokeRequired)
                        {
                            lbox_ChatLog.Invoke(new Action(delegate()
                            {
                                lbox_ChatLog.Items.Add("[클라이언트" + _portip + "] : " + message);
                            }));
                        }
                        else
                            lbox_ChatLog.Items.Add("[클라이언트" + _portip + "] : " + message);
                    }
                    else if (check.Equals("p@"))
                    {
                        if (lbox_Content.InvokeRequired)
                        {
                            lbox_Content.Invoke(new Action(delegate()
                            {
                                lbox_Content.Items.Add("[클라이언트" + _portip + "] : " + message);
                            }));
                        }
                        else                                          //_client.RemoteEndPoint
                            lbox_Content.Items.Add("[클라이언트" + _portip + "] : " + message);
                    }


                    Console.WriteLine(_connectedClients.Count);
                    // 모든 클라이언트들에게 데이터전송
                    for (int i = _connectedClients.Count - 1; i >= 0; i--)
                    {
                        Socket socket = _connectedClients[i];
                        if (socket != obj._workingSocket)
                        {
                            try
                            {
                                socket.Send(obj._buffer);
                            }
                            catch
                            {
                                try // 오류발생하면 전송 취소, 리스트에서 삭제
                                {
                                    socket.Dispose();
                                }
                                catch
                                {
                                    _connectedClients.RemoveAt(i);
                                }
                            }
                        }
                    }

                    obj.ClearBuffer();  //데이터 받고, 버퍼 비움
                    //obj._workingSocket = AcceptCallback 함수의 client
                    obj._workingSocket.BeginReceive(obj._buffer, 0, 4096, 0, DataReceived, obj);  //같은방법으로 수신대기
                }
                catch (SocketException se)
                {
                    se.Message.ToString();
                }
            }
        }

        // client에서 받아온 byte형태의 객체들 string으로 바꿔주고 다시 객체 저장해주는 함수 
        // ★(클라이언트 아이디 부여하면 파라미터 추가하고 DataReceived()에 분기문 태움)★
        private void MessageConverter(string message, string check)
        {
            // p @ SHAPE:RECT @ COLOR:255,255,0,0 @ WIDTH:2 @ POINTS:10,10,100,100 @ STRKEY:_pointKey
            string[] command = message.Split('@');

            if (check.Equals("p@"))
            {
                string curShape;
                Color curColor;
                int curWidth;
                List<Point> curPoints = new List<Point>();

                // SHAPE:RECT
                // COLOR:255,255,0,0
                // WIDTH:2
                // POINTS:10,10,100,100
                // STRKEY:_pointKey
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
                Console.WriteLine("RECEIVED STR KEY: " + _pointKey);
                _moveShapeSetting.Add((_pointKey).ToString(), setting);
                _pointKey++;
                
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
            }         
        }

        private void Check_init(string message, int check,Socket copy_client)
        {
            
            string _check = null;
            if (check == 0)
            {
                _check = "t%";
            }
            else if (check == 1)
            {
                _check = "p%";
            }
            else if (check == 2)
            {
                _check = "g%";
            }

            IPEndPoint ip = (IPEndPoint)_client.RemoteEndPoint;
            string port = ip.Port.ToString();
            string localIP = _client.LocalEndPoint.ToString();

            byte[] bytes = Encoding.Default.GetBytes(localIP + "%" + _check + message + "%");

            //_client.Send(bytes);
            Console.WriteLine("SEND: ip; " + ip + _check + message);
            copy_client.Send(bytes);
        }

        // Server가 쓴 메시지 클라이언트로 보내주는 함수 (0, 1, 2로 구분)
        private void SendMessage(string message, int check)
        {
            // 연결 초기상태냐 아니냐 (통로가 있냐 없냐)
            try
            {
                if (_connectedClients.Count == 0)
                    return;

                // 처음 연결
                if (check == 12)
                {
                    Check_init(message, 1, _client);
                   
                }

                // 기존 연결 
                for (int i = 0; i < _connectedClients.Count; i++)
                {
                    Console.WriteLine(_connectedClients[i]);
                    
                    Check_init(message, check, _connectedClients[i]);

                }
            }
            // 어딘가에서 통로는 막혔는데 아직 보낼게 있을때
            catch (Exception gg)
            {
                lbox_Content.Items.Add("Server에서 연결 끊김 (아직 통로가 남아있음)");
                Console.WriteLine(gg.Message);
                
            }
        }

        // '서버중지' 버튼 눌렀을 때
        private void btn_ServerStop_Click(object sender, EventArgs e)
        {
            if (_client == null)
                return;

            _mainSock.Close();
            _client.Close();
            lbox_Content.Items.Add("서버 해제됨");
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
                SendMessage(message, 2);
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

            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
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

            tbox_Path.Text = imageName[0].Substring(imageName[0].IndexOf(""));
            string message = tbox_Path.Text;
            SendMessage(message, 2);

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

                foreach (KeyValuePair<string, Setting> kv in _moveShapeSetting)
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

                Rectangle _moveShapeRect = new Rectangle(0,0,0,0);  // 초기화
                
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

                string message = "";
                string strPoint = string.Format("POINTS:{0},{1},{2},{3}@", pts[0].X, pts[0].Y, pts[1].X, pts[1].Y);
                string strShpae = string.Format("SHAPE:{0}@", _shape);
                string strColor = string.Format("COLOR:{0},{1},{2},{3}@", _color.A, _color.R, _color.G, _color.B);
                string strWidth = string.Format("WIDTH:{0}@", _width);
                string strKey = string.Format("STRKEY:{0}", _pointKey);

                message += strShpae;
                message += strColor;
                message += strWidth;
                message += strPoint;
                message += strKey;

                // SHAPE:RECT@COLOR:255,255,0,0@WIDTH:2@POINTS:10,10,100,100@STRKEY:_pointKey
                SendMessage(message, 1);

                // 도형 객체 받아서 저장
                Setting setting = new Setting(_shape, _color, _width, pts.ToList());
                _container_setting.Add(setting);

                _moveShapeSetting.Add((_pointKey++).ToString(), setting);
                
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

        // 도형그리기 객체 받아오는 함수
        private void DrawLoad(Graphics g)
        {
            foreach (Setting setting in _container_setting)
            {
                // get함수
                string shape = setting.Shape;
                Color color = setting.Color;
                int width = setting.Width;
                List<Point> Points = setting.Points;

                DrawCurrent(g, color, width, shape, Points[0], Points[1]);
            }

        }

        // 도형그리기 현재 모양 함수
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
            // 서버 대기중인지 확인
            if (!_mainSock.IsBound)
            {
                lbox_ChatLog.Items.Add("[server] : " + tbox_Chat.Text);
                tbox_Chat.Clear();
                return;
            }

            // 보낼 텍스트
            string tts = tbox_Chat.Text;

            if (string.IsNullOrEmpty(tts))
                return;

            SendMessage(tts, 0);

            lbox_ChatLog.Items.Add("[server] : " + tts);
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

        private void 빨강ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _color = Color.Red;
            btn_Colordialog.BackColor = _color;
        }

        private void 파랑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _color = Color.Blue;
            btn_Colordialog.BackColor = _color;
        }

        private void 초록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _color = Color.Green;
            btn_Colordialog.BackColor = _color;
        }

        private void 보통ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _width = 2;
            tbox_Width.Text = unit.unit_width_normal;
        }

        private void 굵게ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_width == 2 || _width == 10)
            {
                _width = 5;
            }
            tbox_Width.Text = unit.unit_width_thick;
        }

        private void 매우굵게ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_width == 2 || _width == 5)
            {
                _width = 10;
            }
            tbox_Width.Text = unit.unit_width_very_thick;
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

        private void 도형삭제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _container_setting.Clear();
            pictureBox1.Refresh();
        }

        private void 불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
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
                Bitmap gBitmap = new Bitmap(image);

                tbox_Path.Text = openFileDialog1.FileName;
            }
        }

        private void 끝내기XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion
    }
}




#region 안씀
// 스레드로 만든 서버실행 함수 (예외가 걸리는 이유 찾기) - 안씀?
//private void AcceptClient()
//{
//    try
//    {
//        while (!isConnect)
//        {
//            tcpClient = tcpListener.AcceptTcpClient();

//            if (tcpClient == null)
//                return;

//            if (tcpClient.Connected)
//            {
//                isConnect = true;
//                // 크로스스레드
//                if (tbox_ClientIP.InvokeRequired)
//                {
//                    tbox_ClientIP.Invoke(new Action(delegate()
//                    {
//                        tbox_ClientIP.Text = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
//                        lbox_Content.Items.Add("Tcp Client 연결 요청 수락 : " + tbox_ClientIP.Text);
//                        tbox_ClientIP.Refresh();
//                    }));
//                }
//                else
//                {
//                    tbox_ClientIP.Text = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
//                    lbox_Content.Items.Add("Tcp Client 연결 요청 수락 : " + tbox_ClientIP.Text);
//                    tbox_ClientIP.Refresh();
//                }
//            }
//            //trGetString = new Thread(new ThreadStart(GetTcpString));
//            //trGetString.Start();
//        }
//        Console.WriteLine("Thread 종료 : AcceptClient()");
//    }
//    catch (Exception ce)
//    {
//        if (networkStream == null)
//            return;
//        networkStream.Flush();
//        Console.WriteLine("AcceptClient IOException: " + ce.ToString());
//        trGetString.Abort();
//    }

//}

// stream으로 tcp연결하고 받아온 데이터 객체 각각 변환 후 계속 뿌려주는 함수
//private void GetTcpString()
//{
//    try
//    {
//        while (isConnect)
//        {
//            networkStream = tcpClient.GetStream();

//            if (networkStream == null)
//                continue;

//            int length = 0;
//            string data = null;
//            byte[] bytes = new byte[1024];

//            //Read() 메소드는 상대방이 보내온 데이터 읽음. 연결이 끊어지면 0 반환
//            while ((length = networkStream.Read(bytes, 0, bytes.Length)) != 0)
//            {
//                data = Encoding.Default.GetString(bytes, 0, length);

//                // 도형객체, 이미지객체 변환해서 받아오는 함수
//                MessageConverter(data);

//                // 쓰레드 안전하게 호출
//                if (lbox_Content.InvokeRequired)
//                {
//                    lbox_Content.Invoke(new Action(delegate()
//                    {
//                        lbox_Content.Items.Add("서버->클라이언트 수신 : " + data);

//                    }));

//                }
//                else
//                {
//                    lbox_Content.Items.Add("서버->클라이언트 수신 : " + data);
//                }
//                // 안전하기 않게 호출
//                //lbox_Content.Items.Add("서버->클라이언트 수신한 메시지 : " + data);
//            }
//            networkStream.Flush();
//            Thread.Sleep(100);
//        }
//    }
//    catch (Exception e)
//    {
//        networkStream.Flush();
//        Console.WriteLine("GetTcpString IOException: " + e.ToString());
//    }
//}
#endregion