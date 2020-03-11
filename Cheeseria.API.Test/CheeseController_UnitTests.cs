using Cheeseria.API.Controllers;
using Cheeseria.Data;
using Cheeseria.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Cheeseria.API.Test
{
    public class CheeseController_UnitTests
    {

        /// <summary>
        /// Fixture contains all per test objects
        /// </summary>
        public class Fixture
        {
            public CheeseContext DBContext;
            public CheeseController SUT;

            public Fixture(string testName)
            {

                //Setting up in memory entity framework for use in unit test.
                var options = new DbContextOptionsBuilder<CheeseContext>()
                    .UseInMemoryDatabase(databaseName: testName)
                    .Options;

                //Test hook to stop database from creating seed data for unit tests.
                CheeseContext.SkipSeedData = true;
                
                DBContext = new CheeseContext(options);
                SUT = new CheeseController(DBContext);

            }

            //Helper method to add cheese into database to make unit tests more readable.
            public void AddEntry(Cheese data)
            {
                DBContext.Cheeses.Add(data);
                DBContext.SaveChanges();
            }
        }

        //Helper method to unwrap OkResult objects and return the data. Help keep unit tests more readable.
        public T GetResultValue<T>(ActionResult<T> result)
        {
            return (T)((OkObjectResult)result.Result).Value;
        }

        [Fact]
        public void When_CallingGetWithUnpublishedSetToFalse_Should_NotReturnUnpublishedCheeses()
        {
            //Arrange
            var fixture = new Fixture("When_CallingGetWithUnpublishedSetToFalse_Should_NotReturnUnpublishedCheeses");

            fixture.AddEntry(new Cheese
            {
                Id = 1,
                Name = "Unpublished Cheese",
                Published = false
            });

            fixture.AddEntry(new Cheese
            {
                Id = 2,
                Name = "Published Cheese",
                Published = true
            });

            //Act
            var results = GetResultValue(fixture.SUT.Get(false));

            //Assert
            Assert.Contains(results, r=> r.Id == 2);
            Assert.DoesNotContain(results, r => r.Id == 1);

        }

        [Fact]
        public void When_CallingGetWithUnpublishedSetToTrue_Should_ReturnUnpublishedCheeses()
        {
            
            //Arrange
            var fixture = new Fixture("When_CallingGetWithUnpublishedSetToTrue_Should_ReturnUnpublishedCheeses");

            fixture.AddEntry(new Cheese
            {
                Id = 1,
                Name = "Unpublished Cheese",
                Published = false
            });

            fixture.AddEntry(new Cheese
            {
                Id = 2,
                Name = "Published Cheese",
                Published = true
            });

            //Act
            var results = GetResultValue(fixture.SUT.Get(true));

            //Assert
            Assert.Contains(results, r => r.Id == 2);
            Assert.Contains(results, r => r.Id == 1);
        }

        [Fact]
        public void When_CallingGetWithValidID_Should_ReturnCheese()
        {
            //Arrange
            var fixture = new Fixture("When_CallingGetWithValidID_Should_ReturnCheese");

            fixture.AddEntry(new Cheese
            {
                Id = 1,
                Name = "Test Cheese",
                Published = true
            });

            //Act
            var result = GetResultValue(fixture.SUT.Get(1));

            //Assert
            Assert.True(result.Name == "Test Cheese");
        }

        [Fact]
        public void When_CallingGetWithInvalidId_Should_Return404()
        {
            //Arrange
            var fixture = new Fixture("When_CallingGetWithInvalidId_Should_Return404");

            //Act
            var result = fixture.SUT.Get(100);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);

        }

        [Fact]
        public void When_CallingPostWithIdOfZero_Should_CreateCheeseAndReturnPositiveId()
        {
            //Arrange
            var fixture = new Fixture("When_CallingPostWithIdOfZero_Should_CreateCheeseAndReturnPositiveId");
            

            //Act
            var result = GetResultValue(fixture.SUT.Post(new Cheese
            {
                Name = "My New Cheese",
                Colour = "Yellow",
                Published = true
            }));

            //Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void When_CallingPostWithIdThatExists_Should_ReturnBadRequest()
        {
            //Arrange
            var fixture = new Fixture("When_CallingPostWithIdThatExists_Should_ReturnBadRequest");

            fixture.AddEntry(new Cheese
            {
                Id = 1,
                Name = "AlreadyExistingCheese",
            });


            //Act
            var result = fixture.SUT.Post(new Cheese
            {
                Id = 1,
                Name = "My New Cheese",
                Colour = "Yellow",
                Published = true
            });

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Theory]
        [InlineData("NewName", "Red", true)]
        [InlineData("", "Red", true)]
        [InlineData("NewName", "", true)]
        [InlineData("", "", false)]
        public void When_CallingPutWithExistingCheese_Should_UpdateValues(string name, string colour, bool published)
        {
            //Arrange
            var fixture = new Fixture("When_CallingPutWithExistingCheese_Should_UpdateValues");

            fixture.AddEntry(new Cheese
            {
                Id = 1,
                Name = "AlreadyExistingCheese",
                Published = false,
                Colour = "ExistingColour"
            });

            //Act
            fixture.SUT.Put(new Cheese
            {
                Id = 1,
                Name = name,
                Colour =  colour,
                Published = published
            });

            var result = GetResultValue(fixture.SUT.Get(1));

            //Assert
            Assert.Equal(string.IsNullOrEmpty(name) ? "AlreadyExistingCheese" : name, result.Name);
            Assert.Equal(string.IsNullOrEmpty(colour) ? "ExistingColour" : colour, result.Colour);
            Assert.Equal(published, result.Published);

            //Clean up so it can be created again next iteration.
            fixture.SUT.Delete(1);
        }

        [Fact]
        public void When_CallingPutWithNonExistingCheese_Should_ReturnNotFound()
        {
            //Arrange
            var fixture = new Fixture("When_CallingPutWithNonExistingCheese_Should_ReturnNotFound");

            //Act
            var result = fixture.SUT.Put(new Cheese
            {
                Id = 1,
                Name = "AnyNameCheese",
                Published = true
            });

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void When_CallingDeleteOnExistingCheese_Should_ReturnOk()
        {
            //Arrange
            var fixture = new Fixture("When_CallingDeleteOnExistingCheese_Should_BeRemoved");

            fixture.AddEntry(new Cheese
            {
                Id = 1,
                Name = "AlreadyExistingCheese",
                Published = false,
                Colour = "ExistingColour"
            });

            //Act
            var result = fixture.SUT.Delete(1);
            

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void When_CallingDeleteOnNonExistingCheese_Should_ReturnNotFound()
        {
            //Arrange
            var fixture = new Fixture("When_CallingDeleteOnNonExistingCheese_Should_ReturnNotFound");

            //Act
            var result = fixture.SUT.Delete(1);


            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
