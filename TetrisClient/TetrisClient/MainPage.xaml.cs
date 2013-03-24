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


namespace TetrisClient
{
    public partial class MainPage : UserControl
    {
        List<Rectangle> displayBoard;
        List<Rectangle> nextShapeBoard;

        int Xcounter = 0; //counter for testing grid
        int Ycounter = 0; //counter for testing grid
        int lineCounter = 0;

        private string name = "Unknown";
        private int boardWidth = 12;
        private int nextShapeBoxWidth = 4;


        public MainPage()
        {
            InitializeComponent();

            if (HtmlPage.Document.QueryString.ContainsKey("Name"))
                name = HtmlPage.Document.QueryString["Name"]; 
            
      
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

            

            StartTimer(null, null);
        }

        //taken from http://msdn.microsoft.com/en-us/library/cc189084%28v=vs.95%29.aspx
        private void StartTimer(object o, RoutedEventArgs sender)
        {
            System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 5); // 100 Milliseconds 
            myDispatcherTimer.Tick += new EventHandler(Each_Tick);
            myDispatcherTimer.Start();

        }

        // Raised every 100 miliseconds while the DispatcherTimer is active.
        private void Each_Tick(object o, EventArgs sender)
        {
            HtmlPage.Plugin.Focus(); //let the silverlight plugin take focus - this makes the keyboard presses work without first clicking in the window

            if ((Ycounter * boardWidth) + Xcounter < displayBoard.Count)
            {
                //this code is only to demonstrate and test the order the squares are filled - can be removed in the release
                SolidColorBrush blackBrush = new SolidColorBrush();
                blackBrush.Color = Colors.Black;
                
                displayBoard[(Ycounter * boardWidth) + Xcounter].Fill = blackBrush;

                Xcounter++;
                lineCounter++;

                if (lineCounter == 12)
                {
                    lineCounter = 0;
                    Ycounter++;
                    Xcounter = 0;
                }
            }
            else
            {
                //this code is the real implementation and must stay in the release - the getArray() method will be called here every second.  
                //getArray will get the updated array from the web service
                string[][] webServiceArray = getArray();

                for (int x = 0; x < webServiceArray.Count(); x++)
                {
                    for (int y = 0; y < webServiceArray[x].Count(); y++)
                    {
                       displayBoard[(y * boardWidth) + x].Fill = getBrush(webServiceArray[x][y]);
                    }
                }

                string[][] nextShape = getNextShape();
                for (int x = 0; x < nextShape.Count(); x++)
                {
                    for (int y = 0; y < nextShape[x].Count(); y++)
                    {
                        nextShapeBoard[(y * nextShapeBoxWidth) + x].Fill = getBrush(nextShape[x][y]);
                    }
                }


            }
        }

        //taken from http://stackoverflow.com/questions/6211388/how-to-convert-00e4ff-to-brush-in-code
        private SolidColorBrush getBrush(string hex)
        {

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

        private void UserControl_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    //call game.down() on the web service
                    lblScore.Content = "down";
                    Each_Tick(null, null); //this should refresh the grid to make the change quicker
                    break;
                case Key.Left:
                    //call game.left() on the web service
                    lblScore.Content = "left";
                    Each_Tick(null, null);
                    break;
                case Key.Right:
                    //call game.right() on the web service
                    lblScore.Content = "right";
                    Each_Tick(null, null);
                    break;
                case Key.Up:
                    //call game.rotate() on the web service
                    lblScore.Content = "rotate";
                    Each_Tick(null, null);
                    break;
                default:
                    break;
            }
        }
    }
}
