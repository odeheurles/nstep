using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay.Charts;

namespace NStep.Viewer
{
    public partial class MainWindow : Window
    {
        private static readonly SolidColorBrush RedBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        private static readonly SolidColorBrush YellowBrush = new SolidColorBrush(Color.FromRgb(255, 255, 0));
        private static readonly SolidColorBrush GreenBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "SampleSteps.txt");
            var steps = Step.LoadFromFile(filePath).ToList();
            var stepsByDuration = steps.OrderByDescending(s => s.DurationMilli).ToList();

            var start = steps.Min(s => s.Timestamp);
            var end = steps.Max(s => s.End);
            var totalDurationMilli = (end - start).TotalMilliseconds;

            for (int i = 0; i < steps.Count; i++)
            {
                var step = steps[i];
                var ranking = stepsByDuration.IndexOf(step);
                AddStep(i, step, totalDurationMilli, ranking, steps.Count);
            }

            SetViewport(steps, start, end);
        }

        private void SetViewport(List<Step> steps, DateTime start, DateTime end)
        {
            var x = dateAxis.ConvertToDouble(start);
            var width = dateAxis.ConvertToDouble(end) - x;

            var xMargin = width / 10;
            x -= xMargin;
            width += 2 * xMargin;

            plotter.Viewport.Visible = new Rect(x, -(steps.Count), width, steps.Count + 1);
        }

        private void AddStep(int index, Step step, double totalDurationMilli, int ranking, int totalSteps)
        {
            var x = dateAxis.ConvertToDouble(step.Timestamp);
            var endTimestamp = step.Timestamp.AddMilliseconds(step.DurationMilli);
            var width = dateAxis.ConvertToDouble(endTimestamp) - x;
            var percentage = step.DurationMilli / totalDurationMilli;
            var text = string.Format("{0} - {1} - {2}ms - {3:0.00}%", ranking, step.Name, step.DurationMilli, percentage * 100);

            SolidColorBrush brush;
            if (ranking < totalSteps / 3)
            {
                brush = RedBrush;
            }
            else if (ranking < 2 * totalSteps / 3)
            {
                brush = YellowBrush;
            }
            else
            {
                brush = GreenBrush;
            }

            var rect = new RectangleHighlight
            {
                Bounds = new Rect(x, -index, width, .1),
                Fill = brush,
                Stroke = brush,
                ToolTip = text
            };

            plotter.Children.Add(rect);

            var container = new ViewportUIContainer
            {
                Position = new Point(x, -index + .5),
                Content = text
            };

            plotter.Children.Add(container);
        }
    }
}
