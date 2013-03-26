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
        TetrisWebServiceSoapClient webService = new TetrisWebServiceSoapClient();

        private string name = "Unknown";

        //board fields
        Rectangle[][] gameBoard;
        Rectangle[][] nextShapeBoard;
        StackPanel panel = new StackPanel();
            

        public MainPage()
        {
            InitializeComponent();

            //retrieve and use name from query string       
            if (HtmlPage.Document.QueryString.ContainsKey("Name"))
                name = HtmlPage.Document.QueryString["Name"];
            lblName.Content = name;

            //add web service event handlers
            webService.StartGameCompleted += new EventHandler<StartGameCompletedEventArgs>(webService_StartGameCompleted);
            webService.MoveBlockDownCompleted += new EventHandler<MoveBlockDownCompletedEventArgs>(webService_MoveBlockDownCompleted);
            webService.DropBlockCompleted += new EventHandler<DropBlockCompletedEventArgs>(WebService_DropBlockCompleted);
            webService.MoveBlockLeftCompleted += new EventHandler<MoveBlockLeftCompletedEventArgs>(WebService_MoveBlockLeftCompleted);
            webService.MoveBlockRightCompleted += new EventHandler<MoveBlockRightCompletedEventArgs>(WebService_MoveBlockRightCompleted);
            webService.RotateBlockCompleted += new EventHandler<RotateBlockCompletedEventArgs>(WebService_RotateBlockCompleted);
            webService.GetScoreCompleted += new EventHandler<GetScoreCompletedEventArgs>(WebService_GetScoreCompleted);
            webService.GetNextShapeCompleted += new EventHandler<GetNextShapeCompletedEventArgs>(WebService_GetNextShapeCompleted);


            //start game
            webService.StartGameAsync(name);

        }

        // Raised every second while the DispatcherTimer is active.
        private void Each_Tick(object o, EventArgs sender)
        {
            HtmlPage.Plugin.Focus(); //let the silverlight plugin take focus - this makes the keyboard presses work without first clicking in the window

            webService.MoveBlockDownAsync();
            webService.GetNextShapeAsync();
            webService.GetScoreAsync();

        }

        //taken from http://msdn.microsoft.com/en-us/library/cc189084%28v=vs.95%29.aspx
        private void webService_StartGameCompleted(object sender, StartGameCompletedEventArgs e)
        {
            try
            {
                gameBoard = createBoard(e.Result, 6, 619);

                webService.GetNextShapeAsync();
                updateBoard(e.Result, gameBoard);

                System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 1 second 
                timer.Tick += new EventHandler(Each_Tick);


                timer.Start();
            }
            catch (Exception ex)
            {
                lblTest.Content = ex.ToString();
            }
        }

        private void WebService_GetNextShapeCompleted(object sender, GetNextShapeCompletedEventArgs e)
        {
            try
            {
                if (nextShapeBoard == null)
                    nextShapeBoard = createBoard(e.Result, 284, 73);

                else
                    updateBoard(e.Result, nextShapeBoard);

            }
            catch (Exception ex)
            {

                lblTest.Content = (ex.ToString());
            }
        }

        #region Board Update and Brush Methods

        private Rectangle[][] createBoard(string[][] webServiceArray, int RectangleX, int RectangleY)
        {
            Rectangle[][] displayArray = new Rectangle[webServiceArray.Count()][];
            int intitalYValue = RectangleY;

            for (int x = 0; x < displayArray.Count(); x++)
            {
                displayArray[x] = new Rectangle[webServiceArray[x].Count()];

                for (int y = 0; y < webServiceArray[x].Count(); y++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Fill = getBrush("FFF4F4F5");
                    rectangle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    rectangle.Height = 20;
                    rectangle.Stroke = getBrush("FF514C4C");
                    rectangle.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    rectangle.Width = 20;

                    rectangle.Margin = new Thickness(RectangleX, RectangleY, 0, 0);

                    displayArray[x][y] = rectangle;

                    RectangleY -= 21;

                    this.LayoutRoot.Children.Add(rectangle);
                }

                RectangleY = intitalYValue;
                RectangleX += 21;
            }

            return displayArray;
        }

        private void updateBoard(string[][] webServiceArray, Rectangle[][] displayArray)
        {
            for (int x = 0; x < webServiceArray.Count(); x++)
            {
                for (int y = 0; y < webServiceArray[x].Count(); y++)
                {
                    displayArray[x][y].Fill = getBrush(webServiceArray[x][y]);
                }
            }
        }

        //taken from http://stackoverflow.com/questions/6211388/how-to-convert-00e4ff-to-brush-in-code
        private SolidColorBrush getBrush(string hex)
        {
            if (hex == "" || hex == null)
                hex = "FFFFFF";

            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));

            SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, r, g, b));

            return myBrush;
        }

        #endregion

        #region Web Service Block Movement Methods

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
            updateBoard(e.Result, gameBoard);
        }

        private void WebService_DropBlockCompleted(object sender, DropBlockCompletedEventArgs e)
        {
            try
            {
                updateBoard(e.Result, gameBoard);
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
                updateBoard(e.Result, gameBoard);
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
                updateBoard(e.Result, gameBoard);
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
                updateBoard(e.Result, gameBoard);
            }
            catch (Exception ex)
            {

                lblTest.Content = (ex.ToString());
            }
        }

        private void WebService_GetScoreCompleted(object sender, GetScoreCompletedEventArgs e)
        {
            try
            {
                lblScore.Content = e.Result.ToString();
            }
            catch (Exception ex)
            {

                lblTest.Content = (ex.ToString());
            }
        }

        #endregion

    }
}
