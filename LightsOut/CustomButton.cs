using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LightsOut
{

    /// <summary>
    /// A custm button object for our game
    /// Stores the Column, Row and IsLit property ontop of the standard button
    /// </summary>
    public class CustomButton : Button
    {
        // Define our on/off colours
        private static readonly Brush _offColour = new SolidColorBrush(new Windows.UI.Color() { A = 255, R = 255, G = 255, B = 255 });
        private static readonly Brush _onColour = new SolidColorBrush(new Windows.UI.Color() { A = 255, R = 0, G = 250, B = 0 });

        public CustomButton()
            : base()
        {
            IsLit = false;

            base.BorderThickness = new Thickness(2, 2, 2, 2);

            // Set the alignment to stretch to fill the grid
            base.HorizontalAlignment = HorizontalAlignment.Stretch;
            base.VerticalAlignment = VerticalAlignment.Stretch;
        }

        private bool _isLit = false;
        public bool IsLit
        {
            get { return _isLit; }
            set
            {
                if (value)
                {
                    Background = _onColour;
                }
                else
                {
                    Background = _offColour;
                }

                _isLit = value;
            }
        }

        public int Row { get; set; }
        public int Column { get; set; }

    }
}
