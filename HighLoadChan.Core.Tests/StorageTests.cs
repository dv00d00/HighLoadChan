namespace HighLoadChan.Core.Tests
{
    using System.Linq;

    using FluentAssertions;

    using HighLoadChan.Infrastructure;
    using HighLoadChan.Storage;

    using NUnit.Framework;

    using Ninject;
    using Ninject.Modules;

    public class UnitTesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IReadModel>().To<InMemmoryReadModel>().InSingletonScope();
            this.Bind<IWriteModel>().To<InMemmoryWriteModel>().InSingletonScope();
            this.Bind<IMessanger>().To<SingleMachineMessanger>().InSingletonScope();
        }
    }

    [TestFixture]
    public class StorageTests
    {
        private IKernel kernel;

        private IReadModel queryFacade;
        private CommandFaccade commandFaccade;

        private IWriteModel writeModel;

        [SetUp]
        public void SetUp()
        {
            kernel = new StandardKernel();
            kernel.Load(new UnitTesModule());
            this.queryFacade = kernel.Get<IReadModel>();
            this.commandFaccade = kernel.Get<CommandFaccade>();
            this.writeModel = this.kernel.Get<IWriteModel>();
        }

        [Test]
        public void ShouldStore()
        {
            var thread = new Thread
            {
                BoardName = "b",
                Name = "Test shit",
            };

            commandFaccade.CreateThread(thread);

            var resultThread = queryFacade.GetWholeThread(1);

            resultThread.BoardName.Should().Be(thread.BoardName);
            resultThread.Name.Should().Be(thread.Name);

            var post = new Post
            {
                Author = "Anon",
                Content = "Hello",
                ThreadId = resultThread.Id
            };

            commandFaccade.AddPost(post);

            resultThread.Posts.Count.Should().Be(1);
            var addedPost = resultThread.Posts.ToList()[0];
            addedPost.Author.Should().Be(post.Author);
            addedPost.Content.Should().Be(post.Content);
        }
    }

    
}