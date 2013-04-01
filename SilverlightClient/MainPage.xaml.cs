using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SilverlightClient.TetrisWebService;

namespace SilverlightClient
{
    // <summary>
    /// A Silverlight front-end for the Tetris web service.  Creates and updates a game board using the web service at http://www.simonjstanford.co.uk/tetriswebservice/TetrisWebService.asmx.  Supports shape left/right/rotate movement, high scores and displays the next shape coming.
    /// </summary>
    public partial class MainPage : UserControl
    {
        //class fields
        TetrisWebServiceSoapClient webService = new TetrisWebServiceSoapClient(); //The proxy to the Tetris web service
        private string name = "Unknown"; //Holds the name of the player passed through a query string.  The default value 'Unknown' is used if the player bypasses the name input screen.
        System.Windows.Threading.DispatcherTimer timer;

        //board fields
        Rectangle[][] gameBoard; //Jagged array to hold a group the Rectangle objects that are displayed in the browser and represent the game board
        Rectangle[][] nextShapeBoard; //Jagged array to hold a group the Rectangle objects that are displayed in the browser and represent the next shape board 

        int gameBoardLoctionX = 6; //The X coords of the bottom left rectangle on the game board
        Point nextShapeBoardLocation = new Point(284, 73); //The XY coords of the bottom left rectangle in the next shape board
        Boolean gameInProgress = true; //bool that shows if the game is in progress or not.  Used to stop key presses moving shapes when the game is over
        Boolean scoreSubmitted = false; //bool to trap an error that meant scores were posted twice
        int interval = 1000; //time between each tick.  Starts at 1000 miliseconds

        /// <summary>
        /// The constructor for the Silverlight client.  Assigns the player name from a query string passed by default.aspx, assigns all the web service event handlers and starts the game.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            lblScore.Content = "0";

            //Retrieve and use name from query string       
            if (HtmlPage.Document.QueryString.ContainsKey("Name"))
                name = HtmlPage.Document.QueryString["Name"];
            lblName.Content = name;

            Instructions.Text = "Left/right arrow keys for movement" + Environment.NewLine + "Up arrow key for rotate" + Environment.NewLine + "Down arrow key for drop";

            //Add web service event handlers - web service calls in Silverlight are asynchronous
            webService.StartGameCompleted += new EventHandler<StartGameCompletedEventArgs>(webService_StartGameCompleted); //begins a new game, returns a new empty board
            webService.MoveBlockDownCompleted += new EventHandler<MoveBlockDownCompletedEventArgs>(webService_MoveBlockDownCompleted); //moves the active shape down one row.  Called every second by the timer
            webService.DropBlockCompleted += new EventHandler<DropBlockCompletedEventArgs>(WebService_DropBlockCompleted); //when the users presses the down key, the active shape drops straight to the bottom of the board
            webService.MoveBlockLeftCompleted += new EventHandler<MoveBlockLeftCompletedEventArgs>(WebService_MoveBlockLeftCompleted); //when the user presses the left key, the active shape moves left one column
            webService.MoveBlockRightCompleted += new EventHandler<MoveBlockRightCompletedEventArgs>(WebService_MoveBlockRightCompleted);  //when the user presses the right key, the active shape moves right one column
            webService.RotateBlockCompleted += new EventHandler<RotateBlockCompletedEventArgs>(WebService_RotateBlockCompleted); //when the user presses the up key, the active shape rotates 90 degrees clockwise
            webService.GetScoreCompleted += new EventHandler<GetScoreCompletedEventArgs>(WebService_GetScoreCompleted); //every second, an updated score is retrieved from the web service by the timer
            webService.GetNextShapeCompleted += new EventHandler<GetNextShapeCompletedEventArgs>(WebService_GetNextShapeCompleted); //every second, the next shape that will appear on the board is drawn in a separate grid
            webService.GetGameStateCompleted += new EventHandler<GetGameStateCompletedEventArgs>(WebService_GetGameStateCompleted); //every second, the client will check if it is game over.
            webService.SubmitScoreCompleted += new EventHandler<SubmitScoreCompletedEventArgs>(WebService_SubmitScoreCompleted); //new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(WebService_SubmitScoreCompleted);  //when game state is false (i.e. game over), then the score is posted to the web service.

            //Start the game
            webService.StartGameAsync(name);
        }

        /// <summary>
        /// Method that is executed after completion of the asynchronous method webService.StartGameAsync().  
        /// - Creates the gameboard using the dimensions in the array returned by the web service
        /// - Calls the GetNextShape web service to retrieve the next shape that will appear on the board
        /// - Instantiates and starts the timer that will tick every second to update the game board with data from the web service - taken from http://msdn.microsoft.com/en-us/library/cc189084%28v=vs.95%29.aspx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webService_StartGameCompleted(object sender, StartGameCompletedEventArgs e)
        {
            try
            {
                //create the gameboard.  The numbers passed are the XY coords of the bottom left Rectangle object on the board.  All other rectangles are drawn in relation to that object
                gameBoard = createBoard(e.Result, new Point(gameBoardLoctionX, ((e.Result[0].Count() - 1) * 21) + 10));

                webService.GetNextShapeAsync(); //call the web service GetNextShape() method asynchronously
                updateBoard(e.Result, gameBoard); //change the gameboard to display the new the board - this would be a blank board with one shape at the top

                //instantiate and start the timer object that will tick every second
                timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, interval); // This sets the timer to tick every second
                timer.Tick += new EventHandler(Each_Tick); //specify the method that will be executed on every tick

                timer.Start();  //start the timer
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Method that is executed on every timer tick.
        /// - Moves the active block down one row
        /// - Gets the next shape that will appear on the board
        /// - Gets the current score
        /// - Checks if the game is over.  If so, the score is posted to the web service
        /// </summary>
        /// <param name="o">The calling object</param>
        /// <param name="sender">Any arguments passed</param>
        private void Each_Tick(object o, EventArgs sender)
        {
            HtmlPage.Plugin.Focus(); //let the silverlight plugin take focus - this stops a bug my making the keyboard presses work without first clicking in the window

            webService.MoveBlockDownAsync();    //Move the active block down one row
            webService.GetNextShapeAsync();     //Get and draw the next shape that will appear in the game board
            webService.GetScoreAsync();         //Get the current score
            webService.GetGameStateAsync();     //Check that the game is not over


            //change the speed of the timer as the score goes up
            int score = Convert.ToInt32(lblScore.Content);

            if (score >= 50 && score < 100)
                interval = 800;
            else if (score >= 100 && score < 150)
                interval = 600;
            else if (score >= 150 && score < 200)
                interval = 400;
            else if (score >= 200)
                interval = 200;
            timer.Stop();
            timer.Interval = new TimeSpan(0, 0, 0, 0, interval); // This sets the timer to tick every second (1000 milliseconds)
            timer.Start();
        }

        #region Board Create/Update, Next Shape and Brush Methods

        /// <summary>
        /// Draws a new two dimensional board using Rectangle objects onto the client window.
        /// </summary>
        /// <param name="webServiceArray">The jagged array used to base the width and height of the two dimensional board.</param>
        /// <param name="BottomLeftRectanglePosition">The co-ordinates of the bottom left Rectangle.  All other rectangles are placed in the browser window in relation to this point.</param>
        /// <returns>References to each Rectangle using a jagged array</returns>
        private Rectangle[][] createBoard(string[][] webServiceArray, Point BottomLeftRectanglePosition)
        {
            Rectangle[][] displayArray = new Rectangle[webServiceArray.Count()][]; //instantiate a new jagged array using the size of the array passed in the parameter
            double intitalYValue = BottomLeftRectanglePosition.Y; //store the starting Y coord - this is used to reset the coords when drawing a new row

            for (int x = 0; x < displayArray.Count(); x++) //for each x co-ordinate in the new array...
            {
                displayArray[x] = new Rectangle[webServiceArray[x].Count()]; //instantiate a new array of Y co-ordinates

                for (int y = 0; y < webServiceArray[x].Count(); y++) //for each XY pair...
                {
                    Rectangle rectangle = new Rectangle(); //create a new Rectangle object
                    //Assign the relevant properties
                    rectangle.Fill = getBrush("e2e2e2");
                    rectangle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    rectangle.Height = 20;
                    rectangle.Stroke = getBrush("e2e2e2");
                    rectangle.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    rectangle.Width = 20;
                    rectangle.Margin = new Thickness((int)BottomLeftRectanglePosition.X, (int)BottomLeftRectanglePosition.Y, 0, 0); //this sets the coords of the rectange

                    displayArray[x][y] = rectangle;  //and add a reference to the Rectangle object to the array, to be returned
                    this.LayoutRoot.Children.Add(rectangle); //add the Rectangle object into the browser window

                    BottomLeftRectanglePosition.Y -= 21; //decrement the Y coord.  This puts all Rectangle objects on a different vertical point
                }

                BottomLeftRectanglePosition.Y = intitalYValue;  //when a full vertical row has been completed, reset the Y co-ordinate...
                BottomLeftRectanglePosition.X += 21; ///and increment the X co-ordinate
            }
            return displayArray; //when completed, return the jagged array
        }

        /// <summary>
        /// Updates the Fill property of all Rectangle objects in jagged array. Used for both the game board and next shape board
        /// </summary>
        /// <param name="webServiceArray">The array that contains the new Fill property values, in hexidecimal colour code format</param>
        /// <param name="displayArray">The array containing the Rectangle objects to be updated</param>
        private void updateBoard(string[][] webServiceArray, Rectangle[][] displayArray)
        {
            //for each XY coord, assign a new colour.  The hexidecimal colour code is converted to a Brush object using getBrush()
            for (int x = 0; x < webServiceArray.Count(); x++)
                for (int y = 0; y < webServiceArray[x].Count(); y++)
                    displayArray[x][y].Fill = getBrush(webServiceArray[x][y]);
        }

        /// <summary>
        /// Creates and updates the next shape board in the browser window.  Executed after GetNextShape() is called on the web service.
        /// </summary>
        /// <param name="sender">The calling object</param>
        /// <param name="e">Any arguments passed</param>
        private void WebService_GetNextShapeCompleted(object sender, GetNextShapeCompletedEventArgs e)
        {
            try
            {
                //check if the next shape board has already been created...
                if (nextShapeBoard == null)
                    nextShapeBoard = createBoard(e.Result, nextShapeBoardLocation); //if not, create it using the array passed by the web service

                else
                    updateBoard(e.Result, nextShapeBoard); //if the next shape board already exists, update it
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Converts a hexidecimal colour code to a SolidColorBrush object.
        /// Taken from http://stackoverflow.com/questions/6211388/how-to-convert-00e4ff-to-brush-in-code
        /// </summary>
        /// <param name="hex">A colour, in hexidecimal code format</param>
        /// <returns>A SolidColourBrush object for the colour specified in the 'hex' parameter.</returns>
        private SolidColorBrush getBrush(string hex)
        {
            //check if the string passed is empty or null
            if (hex == "" || hex == null)
                hex = "FFFFFF"; //if so, create a white brush

            //break apart the hexidecimal code and convert to RGB
            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));

            //instantiate a new brush object with the RGB code and return it
            SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, r, g, b));
            return myBrush;
        }

        #endregion

        #region Web Service Block Movement Methods

        /// <summary>
        /// Passes player keyboard commands for block movement to the web service.  All web service method executions are asynchronous.
        /// </summary>
        /// <param name="sender">The calling object</param>
        /// <param name="e">Any arguments passed</param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    //If the game is in progess, move the active shape to the bottom of the board 
                    if (gameInProgress)
                        webService.DropBlockAsync();
                    break;
                case Key.Left:
                    //If the game is in progess, move the active shape left
                    if (gameInProgress)
                        webService.MoveBlockLeftAsync();
                    break;
                case Key.Right:
                    //If the game is in progess, move the active shape right
                    if (gameInProgress)
                        webService.MoveBlockRightAsync();
                    break;
                case Key.Up:
                    //If the game is in progess, rotate the active shape clockwise
                    if (gameInProgress)
                        webService.RotateBlockAsync();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Updates the game board with the latest shape positions after the active block drops down one row.  Executes after MoveBlockDown() on the web service completes.
        /// </summary>
        /// <param name="sender">The calling object</param>
        /// <param name="e">Any arguments passed from the web service.  Contains the updated game array.</param>
        private void webService_MoveBlockDownCompleted(object sender, MoveBlockDownCompletedEventArgs e)
        {
            //update the game board with the array returned by the web service
            try
            {
                updateBoard(e.Result, gameBoard);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// Updates the game board with the latest shape positions after the active block drops all the way to the bottom of the game board.  Executes after DropBlock() on the web service completes.
        /// </summary>
        /// <param name="sender">The calling object</param>
        /// <param name="e">Any arguments passed from the web service.  Contains the updated game array.</param>
        private void WebService_DropBlockCompleted(object sender, DropBlockCompletedEventArgs e)
        {
            try
            {
                //update the game board with the array returned by the web service
                updateBoard(e.Result, gameBoard);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Updates the game board with the latest shape positions after the active block moves left.  Executes after MoveBlockLeft() on the web service completes.
        /// </summary>
        /// <param name="sender">The calling object</param>
        /// <param name="e">Any arguments passed from the web service.  Contains the updated game array.</param>
        private void WebService_MoveBlockLeftCompleted(object sender, MoveBlockLeftCompletedEventArgs e)
        {
            try
            {
                //update the game board with the array returned by the web service
                updateBoard(e.Result, gameBoard);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Updates the game board with the latest shape positions after the active block moves right.  Executes after MoveBlockRight() on the web service completes.
        /// </summary>
        /// <param name="sender">The calling object</param>
        /// <param name="e">Any arguments passed from the web service.  Contains the updated game array.</param>
        private void WebService_MoveBlockRightCompleted(object sender, MoveBlockRightCompletedEventArgs e)
        {
            try
            {
                //update the game board with the array returned by the web service
                updateBoard(e.Result, gameBoard);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Updates the game board with the latest shape positions after the active block rotates.  Executes after RotateBlock() on the web service completes.
        /// </summary>
        /// <param name="sender">The calling object</param>
        /// <param name="e">Any arguments passed from the web service.  Contains the updated game array.</param>
        private void WebService_RotateBlockCompleted(object sender, RotateBlockCompletedEventArgs e)
        {
            try
            {
                //update the game board with the array returned by the web service
                updateBoard(e.Result, gameBoard);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Updates the browser window with the current score.  Executes after GetScore() on the web service completes.
        /// </summary>
        /// <param name="sender">The calling object</param>
        /// <param name="e">Any arguments passed from the web service.  Contains the updated score.</param>
        private void WebService_GetScoreCompleted(object sender, GetScoreCompletedEventArgs e)
        {
            try
            {
                //update the score label with the current score
                lblScore.Content = e.Result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region End of game methods

        /// <summary>
        /// Stops the game play if the game is over.  Executes after GetGameState() on the web service completes.
        /// </summary>
        /// <param name="sender">The calling object</param>
        /// <param name="e">Any arguments passed from the web service.  Contains the updated score.</param>
        private void WebService_GetGameStateCompleted(object sender, GetGameStateCompletedEventArgs e)
        {
            if (e.Result)
            {
                //if game is active, set local variable to true.  
                //This variable is used by the KeyDown event in the UserControl_KeyDown() method to stop the player moving shapes if the game is over
                gameInProgress = true;
            }
            else
            {
                //if false...
                gameInProgress = false; //stop the player moving shapes
                if (!scoreSubmitted)
                {
                    webService.SubmitScoreAsync(); //submit the final score to the web service
                    scoreSubmitted = true;
                }
                timer.Stop(); //stop the timer
                lblName.Content = "Game Over"; //replace the players name with 'Game Over'
            }

        }

        /// <summary>
        /// Checks the score and if it is a high score, tell the player.  Executes after SubmitScore() on the web service completes.
        /// </summary>
        /// <param name="sender">The calling object</param>
        /// <param name="e">Any arguments passed from the web service.  Contains the updated score.</param>
        private void WebService_SubmitScoreCompleted(object sender, SubmitScoreCompletedEventArgs e)
        {
            if (e.Result)
            {
                MessageBox.Show("You got a high Score! Visit the high scores page to see your ranking.");
            }
        }
        #endregion
    }
}
