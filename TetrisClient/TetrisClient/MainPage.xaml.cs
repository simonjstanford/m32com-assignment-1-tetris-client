using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using TetrisClient.TetrisWebService;
using System.Net.Browser;


namespace TetrisClient
{
    public partial class MainPage : UserControl
    {
       
        
        List<Rectangle> displayBoard;
        List<Rectangle> nextShapeBoard;
        TetrisWebServiceSoapClient webService = new TetrisWebServiceSoapClient();
        System.Windows.Threading.DispatcherTimer myDispatcherTimer;

        private string name = "Unknown";
        private int boardWidth = 12;
        private int nextShapeBoxWidth = 4;

        public MainPage()
        {
            InitializeComponent();
           
            if (HtmlPage.Document.QueryString.ContainsKey("Name"))
                name = HtmlPage.Document.QueryString["Name"];

            webService.StartGameCompleted += new EventHandler<StartGameCompletedEventArgs>(webService_StartGameCompleted);
            webService.MoveBlockDownCompleted += new EventHandler<MoveBlockDownCompletedEventArgs>(webService_MoveBlockDownCompleted);
            webService.HelloWorldCompleted += new EventHandler<HelloWorldCompletedEventArgs>(webService_HelloWorldCompleted);
            webService.DropBlockCompleted += new EventHandler<DropBlockCompletedEventArgs>(WebService_DropBlockCompleted);
            webService.MoveBlockLeftCompleted += new EventHandler<MoveBlockLeftCompletedEventArgs>(WebService_MoveBlockLeftCompleted);
            webService.MoveBlockRightCompleted += new EventHandler<MoveBlockRightCompletedEventArgs>(WebService_MoveBlockRightCompleted);
            webService.RotateBlockCompleted += new EventHandler<RotateBlockCompletedEventArgs>(WebService_RotateBlockCompleted);
            
            lblName.Content = name;

            displayBoard = new List<Rectangle>();
            nextShapeBoard = new List<Rectangle>();

            //detect the rectangles and put them in a list - either the board or the next shape box
            foreach (UIElement child in LayoutRoot.Children)
            {
                if (child is Rectangle)
                {
                    displayBoard.Add((Rectangle)child);
                }
            }

            for (int i = 360; i < displayBoard.Count; i++)
                nextShapeBoard.Add(displayBoard[i]);

            displayBoard.RemoveRange(360, 16);
           // displayBoard.Reverse();
           // webService.HelloWorldAsync();

            StartTimer(null, null);
        }

        //taken from http://msdn.microsoft.com/en-us/library/cc189084%28v=vs.95%29.aspx
        private void StartTimer(object o, RoutedEventArgs sender)
        {
            myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 100 Milliseconds 
            myDispatcherTimer.Tick += new EventHandler(Each_Tick);

            webService.StartGameAsync(name);
            myDispatcherTimer.Start();
            

        }

        // Raised every 100 miliseconds while the DispatcherTimer is active.
        private void Each_Tick(object o, EventArgs sender)
        {
            HtmlPage.Plugin.Focus(); //let the silverlight plugin take focus - this makes the keyboard presses work without first clicking in the window
            
            webService.MoveBlockDownAsync();
           
        }

        //taken from http://stackoverflow.com/questions/6211388/how-to-convert-00e4ff-to-brush-in-code
        private SolidColorBrush getBrush(string hex)
        {
            if (hex == "")
                hex = "FFFFFF";

            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));

            SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, r, g, b));

            return myBrush;
        }

        //a test method - should request the string array from the web service for the board
        private string[][] getArray()
        {
            string[][] webServiceData = new string[12][];
            webServiceData[11] = new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", };
            webServiceData[10] = new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", };
            webServiceData[9] =  new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", };
            webServiceData[8] =  new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", };
            webServiceData[7] =  new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "800080", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", };
            webServiceData[6] =  new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "800080", "800080", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", };
            webServiceData[5] =  new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "800080", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", };
            webServiceData[4] =  new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", };
            webServiceData[3] =  new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", };
            webServiceData[2] =  new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", };
            webServiceData[1] =  new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", };          
            webServiceData[0] =  new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF",};

            


            return webServiceData;
        }

        //a test method - should request the string array from the web service for the next shape
        private string[][] getNextShape()
        {
            string[][] webServiceData = new string[4][];
            webServiceData[3] = new string[] { "FFFFFF", "FFFFFF", "FFFFFF", "FFFFFF", };
            webServiceData[2] = new string[] { "FFFFFF", "FFFFFF", "FF0000", "FFFFFF", };
            webServiceData[1] = new string[] { "FFFFFF", "FF0000", "FF0000", "FFFFFF", };
            webServiceData[0] = new string[] { "FFFFFF", "FF0000", "FFFFFF", "FFFFFF", };




            return webServiceData;
        }

        private void updateBoard(string[][] webServiceArray)
        {
            for (int x = 0; x < webServiceArray.Count(); x++)
            {
                for (int y = 0; y < webServiceArray[x].Count(); y++)
                {
                    lblName.Content = x + "," + y;
                    displayBoard[(y * boardWidth) + x].Fill = getBrush(webServiceArray[x][y]);
                }
            }

            displayBoard[0].Fill = getBrush(webServiceArray[0][0]);
           // lblName.Content = webServiceArray[0][0];

        }

        private void updateNextShapeBoard(string[][] board)
        {
            string[][] nextShape = getNextShape();
            for (int x = 0; x < nextShape.Count(); x++)
            {
                for (int y = 0; y < nextShape[x].Count(); y++)
                {
                    nextShapeBoard[(y * nextShapeBoxWidth) + x].Fill = getBrush(nextShape[x][y]);
                }
            }
        }

        private void UserControl_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    //call game.down() on the web service
                    webService.DropBlockAsync();
                    break;
                case Key.Left:
                    //call game.left() on the web service
                    webService.MoveBlockLeftAsync();
                    break;
                case Key.Right:
                    //call game.right() on the web service
                    webService.MoveBlockRightAsync();
                    break;
                case Key.Up:
                    //call game.rotate() on the web service
                    webService.RotateBlockAsync();
                    break;
                default:
                    break;
            }
        }

        private void webService_MoveBlockDownCompleted(object sender, MoveBlockDownCompletedEventArgs e)
        {
            lblScore.Content = e.Result.Count();
            updateBoard(e.Result);
           // updateBoard(getArray());
        }

        private void webService_StartGameCompleted(object sender, StartGameCompletedEventArgs e)
        {
            try
            {
                //updateBoard(getArray());
            updateBoard(e.Result);
           // myDispatcherTimer.Start();
            }
            catch (Exception ex)
            {
                lblScore.Content = e.Result.Count() * e.Result[0].Count(); ;
                lblTest.Content = ex.ToString();
               
            }
            
        }

        private void WebService_DropBlockCompleted(object sender, DropBlockCompletedEventArgs e)
        {
            try
            {
                updateBoard(e.Result);
            }
            catch (Exception ex)
            {
                
                 lblTest.Content = (ex.ToString());
            }
        }

        private void WebService_MoveBlockLeftCompleted(object sender, MoveBlockLeftCompletedEventArgs e)
        {
            try
            {
                updateBoard(e.Result);
            }
            catch (Exception ex)
            {

                lblTest.Content = (ex.ToString());
            }
        }

         private void WebService_MoveBlockRightCompleted(object sender, MoveBlockRightCompletedEventArgs e)
        {
            try
            {
                updateBoard(e.Result);
            }
            catch (Exception ex)
            {

                lblTest.Content = (ex.ToString());
            }
        }

         private void WebService_RotateBlockCompleted(object sender, RotateBlockCompletedEventArgs e)
         {
             try
             {
                 updateBoard(e.Result);
             }
             catch (Exception ex)
             {

                 lblTest.Content = (ex.ToString());
             }
         }

        private void webService_HelloWorldCompleted(object sender, HelloWorldCompletedEventArgs e)
        {
            try
            {
                lblScore.Content = e.Result.ToString();
            }
            catch (Exception Ex)
            {

                lblTest.Content = (Ex.ToString()); 
            }
            
            lblName.Content = "test";
        }
    }
}
