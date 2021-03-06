﻿using LinkedInSearchUi.Graphing;
using LinkedInSearchUi.Indexing;
using LinkedInSearchUi.KMeansInfrastructure;
using LinkedInSearchUi.MachineLearning;
using LinkedInSearchUi.Model;
using LinkedInSearchUi.Resources;
using LinkedInSearchUi.ViewModel;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.DependencyInjection
{
    public class DiLinkedIn : NinjectModule
    {
        public override void Load()
        {
            Bind<IWindowFactory>().To<WindowFactory>();
            Bind<IModel>().To<Model.Model>().InSingletonScope();
            Bind<ISearchViewModel>().To<SearchViewModel>();
            Bind<IStatisticsViewModel>().To<StatisticsViewModel>();
            Bind<IMainViewModel>().To<MainViewModel>();
            Bind<IGraphingService>().To<GraphingService>();
            Bind<ILuceneService>().To<LuceneService>();
            Bind<IHtmlParser>().To<HtmlParser>();
            Bind<ITrainingAndTestingService>().To<TrainingAndTestingService>();
            Bind<ICompanyJobPairService>().To<CompanyJobPairService>();
            Bind<IJobService>().To<JobService>();
            Bind<ICompanyService>().To<CompanyService>();
            Bind<ISkillService>().To<SkillService>();
            Bind<IKmeansService>().To<KMeansService>();
            Bind<IDataPointService>().To<DataPointService>();
            Bind<ISupportVectorMachineService>().To<SupportVectorMachineService>();
            Bind<IRandomForestService>().To<RandomForestService>();
        }
    }
}
