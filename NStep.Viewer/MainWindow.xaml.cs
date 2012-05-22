using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay.Charts;

namespace NStep.Viewer
{
    public partial class MainWindow
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
            var report = ReportBuilder.LoadFromFile(filePath);

            foreach (var step in report.Steps)
            {
                AddStep(report, step);
            }

            SetViewport(report);
        }

        private void SetViewport(Report report)
        {
            var x = dateAxis.ConvertToDouble(report.Start);
            var width = dateAxis.ConvertToDouble(report.End) - x;

            var xMargin = width / 10;
            x -= xMargin;
            width += 2 * xMargin;

            plotter.Viewport.Visible = new Rect(x, -report.StepsCount, width, report.StepsCount + 1);
        }

        private void AddStep(Report report, Step step)
        {
            var x = dateAxis.ConvertToDouble(step.Timestamp);
            var width = dateAxis.ConvertToDouble(step.End) - x;
            var brush = GetSeverityBrush(report, step);
            var text = step.ToString();

            var rect = new RectangleHighlight
            {
                Bounds = new Rect(x, -step.Index, width, .1),
                Fill = brush,
                Stroke = brush,
                ToolTip = text
            };

            plotter.Children.Add(rect);

            var container = new ViewportUIContainer
            {
                Position = new Point(x, -step.Index + .5),
                Content = text
            };

            plotter.Children.Add(container);
        }

        private static SolidColorBrush GetSeverityBrush(Report report, Step step)
        {
            switch (step.GetSeverity(report.StepsCount))
            {
                case Severity.High:
                    return RedBrush;
                case Severity.Medium:
                    return YellowBrush;
                case Severity.Low:
                    return GreenBrush;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
