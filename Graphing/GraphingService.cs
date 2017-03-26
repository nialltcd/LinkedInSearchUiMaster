using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkedInSearchUi.Model;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using LinkedInSearchUi.DataTypes;

namespace LinkedInSearchUi.Graphing
{
    public class GraphingService : IGraphingService
    {
        private readonly IModel _model;
        public GraphingService(IModel model)
        {
            _model = model;
        }

        public PlotModel GenerateTopCompanyJobPairs()
        {
            var plot = new PlotModel { Title = "Top Company Job Pairs" };

            var topCompanyJobPairs = _model.GetCompanyJobPairs();

            List<BarItem> barItems = new List<BarItem>();
            List<string> axis = new List<string>();
            foreach (var companyJobPair in topCompanyJobPairs.Take(30))
            {
                barItems.Add(new BarItem { Value = companyJobPair.Count });
                axis.Add(companyJobPair.CompanyName + "," + companyJobPair.JobName);
            }
            var barSeries = new BarSeries
            {
                ItemsSource =barItems,
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0:.00}"
            };

            plot.Series.Add(barSeries);

            plot.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "Company Job Pairing",
                ItemsSource = axis
            });

            return plot;
        }

        public PlotModel GenerateTopJobStats()
        {
            var plot = new PlotModel { Title = "Top Job Statistics" };

            var topJobStats = _model.GetTopJobStats();

            List<BarItem> barItems = new List<BarItem>();
            List<string> axis = new List<string>();
            foreach (var jobStat in topJobStats.Take(30))
            {
                barItems.Add(new BarItem { Value = jobStat.Count });
                axis.Add(jobStat.JobName);
            }
            var barSeries = new BarSeries
            {
                ItemsSource = barItems,
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0:.00}"
            };

            plot.Series.Add(barSeries);

            plot.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "Job Name",
                ItemsSource = axis
            });

            return plot;
        }

        public PlotModel GenerateTopCompanyStats()
        {
            var plot = new PlotModel { Title = "Top Company Statistics" };

            var topCompanyStats = _model.GetTopCompanyStats();

            List<BarItem> barItems = new List<BarItem>();
            List<string> axis = new List<string>();
            foreach (var companyStat in topCompanyStats.Take(30))
            {
                barItems.Add(new BarItem { Value = companyStat.Count });
                axis.Add(companyStat.CompanyName);
            }
            var barSeries = new BarSeries
            {
                ItemsSource = barItems,
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0:.00}"
            };

            plot.Series.Add(barSeries);

            plot.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "Company Name",
                ItemsSource = axis
            });

            return plot;
        }

        public PlotModel GenerateUsefulProfilePieChart()
        {            
            int useful=0;
            int notUseful=0;

            var people = _model.GetPeople();

            foreach (var person in people)
            {
                if (person.Experiences.Count == 0)
                    notUseful++;
                else
                    useful++;
            }
            var model = new PlotModel { Title = "Profile Overview" };

            dynamic seriesP1 = new PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };

            seriesP1.Slices.Add(new PieSlice("Useful Profiles", useful) { IsExploded = false, Fill = OxyColors.PaleVioletRed });
            seriesP1.Slices.Add(new PieSlice("Un-Useful Profiles", notUseful) { IsExploded = true });

            model.Series.Add(seriesP1);
            return model;
        }
    }
}
