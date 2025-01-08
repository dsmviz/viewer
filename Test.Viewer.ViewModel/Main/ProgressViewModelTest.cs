using Moq;
using Dsmviz.Interfaces.Util;
using Dsmviz.Viewer.ViewModel.Main;

namespace Dsmviz.Tests.ViewModel
{
    [TestClass]
    public class ProgressViewModelTests
    {
        private ProgressViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new ProgressViewModel();
        }

        [TestMethod]
        public void Given_InitialState_When_GettingTitle_Then_TitleIsEmpty()
        {
            // Arrange  
            // (Setup already done in Setup method)  

            // Act  
            var title = _viewModel.Title;

            // Assert  
            Assert.AreEqual(string.Empty, title);
        }

        [TestMethod]
        public void Given_BusyProgressState_When_UpdateCalled_Then_TitleAndProgressTextUpdated()
        {
            // Arrange  
            var progressInfoMock = new Mock<IFileProgressInfo>();
            progressInfoMock.Setup(p => p.State).Returns(ProgressState.Busy);
            progressInfoMock.Setup(p => p.ActionText).Returns("Processing");
            progressInfoMock.Setup(p => p.CurrentItemCount).Returns(1);
            progressInfoMock.Setup(p => p.TotalItemCount).Returns(5);
            progressInfoMock.Setup(p => p.ItemType).Returns("Files");
            progressInfoMock.Setup(p => p.Percentage).Returns(20);

            // Act  
            _viewModel.Update(progressInfoMock.Object);

            // Assert  
            Assert.AreEqual("Processing", _viewModel.Title);
            Assert.AreEqual("1/5 Files", _viewModel.ProgressText);
            Assert.AreEqual(20, _viewModel.ProgressValue);
            Assert.AreEqual(ProgressState.Busy, _viewModel.State);
        }

        [TestMethod]
        public void Given_ErrorProgressState_When_UpdateCalled_Then_ErrorTextUpdated()
        {
            // Arrange  
            var progressInfoMock = new Mock<IFileProgressInfo>();
            progressInfoMock.Setup(p => p.State).Returns(ProgressState.Error);
            progressInfoMock.Setup(p => p.ErrorText).Returns("An error occurred.");

            // Act  
            _viewModel.Update(progressInfoMock.Object);

            // Assert  
            Assert.AreEqual("An error occurred.", _viewModel.ErrorText);
            Assert.AreEqual(ProgressState.Error, _viewModel.State);
        }

        [TestMethod]
        public void Given_BusyProgressState_When_StateChanged_Then_StateChangedEventIsTriggered()
        {
            // Arrange  
            bool eventTriggered = false;
            _viewModel.StateChanged += (sender, state) => eventTriggered = true;

            var progressInfoMock = new Mock<IFileProgressInfo>();
            progressInfoMock.Setup(p => p.State).Returns(ProgressState.Busy);
            progressInfoMock.Setup(p => p.ActionText).Returns("Processing");

            // Act  
            _viewModel.Update(progressInfoMock.Object);

            // Assert  
            Assert.IsTrue(eventTriggered);
            Assert.AreEqual(ProgressState.Busy, _viewModel.State);
        }
    }
}