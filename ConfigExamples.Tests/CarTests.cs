 using ConigExamples.Configuration;
using ConigExamples.Infrastructure;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ConfigExamples.Tests
{
    //Unit testing wit options
    //2 Examples for IOptions -> there are convenience methods
    //No convenience methods for IOptionsMonitor and IOptionsSnapshot => good idea is using Moq
    public class CarTests
    {
        [Fact]
        public void CarCanGreet_1()
        {
            //1. Using Options.Create

            //Need only the properties that will be used in the test

            //Arrange
            var options = Options.Create(new CarConfig { Color = "Blue" });
            Car c = new Car(options);

            //Act
            string greeting = c.PresentYourself();

            //Assert
            Assert.True(!string.IsNullOrEmpty(greeting));
        }

        [Fact]
        public void CarCanGreet_2()
        {
            //2. Using Moq
            //Need only the properties that will be used in the test

            Mock<IOptions<CarConfig>> optMock = new Mock<IOptions<CarConfig>>();
            optMock.SetupGet(x => x.Value).Returns(new CarConfig { Color = "Red" });

            //Arrange
            Car c = new Car(optMock.Object);

            //Act
            string greeting = c.PresentYourself();

            //Assert
            Assert.True(!string.IsNullOrEmpty(greeting));
        }
    }
}
