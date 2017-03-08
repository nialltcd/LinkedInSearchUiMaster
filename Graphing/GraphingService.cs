using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkedInSearchUi.Model;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace LinkedInSearchUi.Graphing
{
    public class GraphingService
    {
        private readonly Model.Model _model;
        public GraphingService()
        {
            _model = new Model.Model();
        }

        public PlotModel GenerateTopCompanyJobPairs()
        {
            var plot = new PlotModel { Title = "Top Company Job Pairs" };

            var topCompanyJobPairs = _model.ParseTopCompanyJobPairsFromXml();

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
    }
}
