using MVVM.Birthday;
using NUnit.Framework;

namespace MVVM
{
    [TestFixture]
    sealed class TestPersonViewModel
    {
        [Test]
        public void TestAddAge()
        {
            PersonViewModel viewModel = new PersonViewModel
                                            {
                                                Name = "Cheka",
                                                Age = 20
                                            };
            string changedPropertyName = null;
            viewModel.PropertyChanged += (sender, evtargs) => { changedPropertyName = evtargs.PropertyName; };

            viewModel.AgeAddCommand.Execute(null);

            Assert.AreEqual("Age", changedPropertyName);
            Assert.AreEqual(21, viewModel.Age);
        }
    }
}
