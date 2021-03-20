using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LightsOut
{
    /// <summary>
    /// Our main page for our LightsOut game
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Define how many rows the game has
        public static readonly int GridRows = 5;
        // Define how many columns the game has
        public static readonly int GridColumns = 5;

        public MainPage()
        {
            this.InitializeComponent();

            // Setup the grid with the row and column definitions
            SetupGrid();

            // Initialise the buttons and assign them to the grid
            SetupButtons();
            SetStartLights();
        }

        private void SetupGrid()
        {
            // Setup the grid rows
            for (int i = 0; i < GridRows; ++i)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Setup the grid columns
            for (int i = 0; i < GridColumns; ++i)
            {
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void SetupButtons()
        {
            for (int row = 0; row < GridRows; ++row)
            {
                for (int column = 0; column < GridColumns; ++column)
                {
                    CustomButton btn = new CustomButton();
                    // Attach a new Click event to the button
                    btn.Click += Btn_Click;

                    // Assign the row and column to the button
                    btn.Row = row;
                    btn.Column = column;

                    // Add the button to the grid
                    mainGrid.Children.Add(btn);

                    // Set the column and row variables for the button so they're in the correct place on the grid
                    btn.SetValue(Grid.RowProperty, row);
                    btn.SetValue(Grid.ColumnProperty, column);
                }
            }
        }

        /// <summary>
        /// Set the start lights to a random pattern
        /// </summary>
        private void SetStartLights()
        {
            // Create a new random object so the lights have true random
            Random rnd = new Random();
            foreach (CustomButton btn in mainGrid.Children.Where(child => child is CustomButton))
            {
                // Pick a random number between 0 and 10
                int randomNumber = rnd.Next(10);
                // Is the number a multiple of 5?
                if (randomNumber % 5 == 1)
                {
                    // If so, set the light to on
                    btn.IsLit = true;
                }
            }
        }

        private async void Btn_Click(object sender, RoutedEventArgs e)
        {
            CustomButton btn = (CustomButton)sender;

            // If the button is lit, turn it off
            btn.IsLit = !btn.IsLit;
            
            // Toggle the buttons nearby
            ToggleNearbyButtons(btn);

            // Did we turn them all off?
            if (CheckForWin())
            {
                // We won! - Show a dialog box telling the user
                ContentDialog winDlg = new ContentDialog
                {
                    Title = "Congratulations",
                    Content = "You Won!",
                    PrimaryButtonText = "Retry",
                    CloseButtonText = "Close"
                };

                ContentDialogResult result = await winDlg.ShowAsync();

                switch (result)
                {
                    case ContentDialogResult.Primary:
                        {
                            mainGrid.Children.Clear();

                            // Set the buttons back up
                            SetupButtons();
                            SetStartLights();
                        }
                        break;

                    default:
                        {
                            App.Current.Exit();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Toggle the buttons to the top, left, right and bottom of the pressed button
        /// </summary>
        /// <param name="toggledButton">Which button was pressed</param>
        private void ToggleNearbyButtons(CustomButton toggledButton)
        {
            // Grab a list of buttons assigned to mainGrid. Make sure they're a 'CustomButton' object, just in case we add elements later!
            IEnumerable<CustomButton> buttonsList = from child in mainGrid.Children
                                                    where child is CustomButton
                                                    select child as CustomButton;

            // See if we're not the top row, there's nothing above!
            if (toggledButton.Row > 0)
            {
                int aboveRow = toggledButton.Row - 1;
                // Find the button above
                var upperButtonQuery = from btn in buttonsList
                                           where btn.Row == aboveRow
                                           && btn.Column == toggledButton.Column
                                           select btn;

                CustomButton upperButton = upperButtonQuery.FirstOrDefault();
                if (upperButton != null)
                {
                    upperButton.IsLit = !upperButton.IsLit;
                }
            }

            // Check if we're not the left-most button
            if (toggledButton.Column > 0)
            {
                int leftColumn = toggledButton.Column - 1;
                // Find the button to the left
                var leftButtonQuery = from btn in buttonsList
                                       where btn.Row == toggledButton.Row
                                       && btn.Column == leftColumn
                                      select btn;

                CustomButton leftButton = leftButtonQuery.FirstOrDefault();
                if (leftButton != null)
                {
                    leftButton.IsLit = !leftButton.IsLit;
                }
            }

            // See if we're not on the bottom row
            if (toggledButton.Row < GridRows)
            {
                int lowerRow = toggledButton.Row + 1;
                // Find the button below
                var lowerButtonQuery = from btn in buttonsList
                                       where btn.Row == lowerRow
                                       && btn.Column == toggledButton.Column
                                       select btn;

                CustomButton lowerButton = lowerButtonQuery.FirstOrDefault();
                if (lowerButton != null)
                {
                    lowerButton.IsLit = !lowerButton.IsLit;
                }
            }

            // Make sure we're not the right-most button
            if (toggledButton.Column < GridColumns)
            {
                int rightColumn = toggledButton.Column + 1;
                // Find the button to the right
                var rightButtonQuery = from btn in buttonsList
                                      where btn.Row == toggledButton.Row
                                      && btn.Column == rightColumn
                                      select btn;

                CustomButton rightButton = rightButtonQuery.FirstOrDefault();
                if (rightButton != null)
                {
                    rightButton.IsLit = !rightButton.IsLit;
                }
            }
        }

        /// <summary>
        /// Win conditions are when all the lights are turned off
        /// </summary>
        private bool CheckForWin()
        {
            bool hasLitButtons = false;
            foreach (CustomButton btn in mainGrid.Children.Where(child => child is CustomButton))
            {
                // See if this button is lit
                if (btn.IsLit)
                {
                    hasLitButtons = true;

                    // Stop processing the loop, we have a lit button
                    break;
                }
            }

            // If we have lit buttons, we haven't won (yet) so return the opposite value
            return !hasLitButtons;
        }
    }
}
