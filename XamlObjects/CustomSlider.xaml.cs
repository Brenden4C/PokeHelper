using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PokeHelper.XamlObjects
{
    public partial class CustomSlider : UserControl
    {
        // Event that will be raised when the slider value changes
        public event Action<double> ValueChanged;
        private bool isDragging = false;

        public CustomSlider()
        {
            InitializeComponent();
            UpdateSlider( .5 );
        }

        private void SliderGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(SliderGrid);
            double percentage = pos.X / SliderGrid.ActualWidth; // Get the percentage of where the mouse is clicked
            UpdateSlider(percentage);

            // Start dragging when mouse is pressed on the thumb
            if (e.OriginalSource == Thumb)
            {
                isDragging = true;
                Mouse.Capture(Thumb); // Capture mouse so that dragging can happen even if it leaves the thumb
            }
        }

        private void SliderGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var pos = e.GetPosition(SliderGrid);
                double percentage = pos.X / SliderGrid.ActualWidth; // Calculate percentage position of the mouse
                UpdateSlider(percentage); // Update the thumb and fill bar position
            }
        }

        private void SliderGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Stop dragging when mouse is released
            if (isDragging)
            {
                isDragging = false;
                Mouse.Capture(null); // Release mouse capture
            }
        }

        private void UpdateSlider(double value)
        {
            double trackWidth = SliderGrid.ActualWidth;
            double clampedValue = Math.Max(0, Math.Min(value, 1)); // Ensure value is between 0 and 1

            // Calculate target width for the MusicFill
            double targetWidth = clampedValue * trackWidth;

            // Set the width of the fill bar directly (no animation)
            MusicFill.Width = targetWidth;

            // Set the thumb position to be at the end of the fill
            ThumbTransform.X = targetWidth - (Thumb.Width / 2);

            // Prevent the thumb from going past the end of the track
            ThumbTransform.X = Math.Min(ThumbTransform.X, trackWidth - Thumb.Width);

            // Raise the ValueChanged event to notify external listeners (e.g., QuizPage)
            double valuePercentage = clampedValue * 100; // Convert the value to a percentage (0 - 100)
            ValueChanged?.Invoke(valuePercentage);
        }



    }
}
