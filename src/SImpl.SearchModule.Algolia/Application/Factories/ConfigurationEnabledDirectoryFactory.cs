using Microsoft.Extensions.DependencyInjection;
using SImpl.SearchModule.Algolia.Configuration;

namespace SImpl.SearchModule.Algolia.Application.Factories
{
    public class ConfigurationEnabledDirectoryFactory : DirectoryFactoryBase
    {
        private readonly IServiceProvider _services;
        private readonly ExamineSearchConfiguration _examineSearchConfiguration;
        private readonly IApplicationRoot _applicationRoot;
        private IDirectoryFactory _directoryFactory;

        public ConfigurationEnabledDirectoryFactory(
            IServiceProvider services,
            ExamineSearchConfiguration examineSearchConfiguration,
            IApplicationRoot applicationRoot)
        {
            _services = services;
            _examineSearchConfiguration = examineSearchConfiguration;
            _applicationRoot = applicationRoot;
        }

        protected override Lucene.Net.Store.Directory CreateDirectory(
            LuceneIndex luceneIndex,
            bool forceUnlock)
        {
            _directoryFactory = CreateFactory();
            return _directoryFactory.CreateDirectory(luceneIndex, forceUnlock);
        }

        /// <summary>
        /// Creates a directory factory based on the configured value and ensures that
        /// </summary>
        private IDirectoryFactory CreateFactory()
        {
            DirectoryInfo applicationRoot = _applicationRoot.ApplicationRoot;
            if (!applicationRoot.Exists)
                System.IO.Directory.CreateDirectory(applicationRoot.FullName);

            return (IDirectoryFactory) this._services.GetRequiredService(_examineSearchConfiguration
                .LuceneDirectoryFactory);
        }
    }
}