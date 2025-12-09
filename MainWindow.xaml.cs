using System;
using System.Windows;
using OxyPlot;
using OxyPlot.Series;
using AngouriMath;
using AngouriMath.Extensions;

namespace FuncLab
{
    public partial class MainWindow : Window
    {
        private PlotModel plotModel;

        public MainWindow()
        {
            InitializeComponent();

            // Plot alanını başlat
            plotModel = new PlotModel { Title = "Function Plot" };
            PlotView.Model = plotModel;
        }

        // Draw Button
        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            string funcText = FunctionInput.Text;

            try
            {
                plotModel.Series.Clear();
                var series = new LineSeries { Title = funcText };

                double xMin = -10;
                double xMax = 10;
                double step = 0.1;

                // Parse the function
                var expr = funcText.ToEntity();

                for (double x = xMin; x <= xMax; x += step)
                {
                    var val = expr.Substitute("x", x).EvalNumerical();
                    series.Points.Add(new DataPoint(x, (double)val.RealPart)); // Updated to use RealPart
                }

                plotModel.Series.Add(series);
                plotModel.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Derivative Button
        private void DerivativeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var expr = FunctionInput.Text.ToEntity();

                var derivative = expr.Differentiate("x");
                var derivativeRational = derivative.Simplify();

                DerivativeResult.Text = $"Original: {derivative}\nRational Simplified: {derivativeRational}";
            }
            catch (Exception ex)
            {
                DerivativeResult.Text = "Error: " + ex.Message;
            }
        }

        // Updated code to fix CS0029 error in the IntegralButton_Click method
        private void IntegralButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var expr = FunctionInput.Text.ToEntity();

                var integral = expr.Integrate("x").Simplify();

                IntegralResult.Text = integral.Stringize() + " + C"; // Use Stringize() to convert Entity to string
            }
            catch (Exception ex)
            {
                IntegralResult.Text = "Error: " + ex.Message;
            }
        }
        // Definite Integral butonuna basınca limit inputları aç
        private void DefiniteIntegralButton_Click(object sender, RoutedEventArgs e)
        {
            DefIntegralPanel.Visibility = Visibility.Visible;
        }

        // OK butonuna basınca hesapla
        private void DefIntegralOkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var expr = FunctionInput.Text;
                var f = expr.ToEntity();
                double a = double.Parse(LowerLimitInput.Text);
                double b = double.Parse(UpperLimitInput.Text);

                // Belirli integral (numeric)
                var integralExpr = f.Integrate("x");
                var Fa = integralExpr.Substitute("x", a).EvalNumerical();
                var Fb = integralExpr.Substitute("x", b).EvalNumerical();
                var result = Fb - Fa;

                IntegralResult.Text = $"∫[{a},{b}] f(x) dx = {result}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Clear butonu
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            FunctionInput.Text = "";
            DerivativeResult.Text = "";
            IntegralResult.Text = "";
            LowerLimitInput.Text = "";
            UpperLimitInput.Text = "";
            DefIntegralPanel.Visibility = Visibility.Collapsed;

            plotModel = new PlotModel { Title = "Function Plot" };
            PlotView.Model = plotModel;
            PlotView.InvalidatePlot();
        }
    }
}
