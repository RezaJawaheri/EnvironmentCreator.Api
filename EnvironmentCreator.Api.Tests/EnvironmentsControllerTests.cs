using EnvironmentCreator.Api.Controllers;
using EnvironmentCreator.Api.Data;
using EnvironmentCreator.Api.Dtos;
using EnvironmentCreator.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Xunit;

namespace EnvironmentCreator.Api.Tests
{
    public class EnvironmentsControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        private EnvironmentsController GetController(ApplicationDbContext context, string userId = "test-user")
        {
            var controller = new EnvironmentsController(context);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }

        [Fact]
        public async Task CreateEnvironment_ReturnsBadRequest_WhenUserAlreadyHasFiveWorlds()
        {
            var context = GetDbContext();

            for (int i = 1; i <= 5; i++)
            {
                context.Environments2D.Add(new Environment2D
                {
                    Name = "World" + i,
                    UserId = "test-user"
                });
            }

            await context.SaveChangesAsync();

            var controller = GetController(context);

            var dto = new CreateEnvironmentDto
            {
                Name = "World6"
            };

            var result = await controller.CreateEnvironment(dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateEnvironment_ReturnsBadRequest_WhenWorldNameAlreadyExists()
        {
            var context = GetDbContext();

            context.Environments2D.Add(new Environment2D
            {
                Name = "MijnWereld",
                UserId = "test-user"
            });

            await context.SaveChangesAsync();

            var controller = GetController(context);

            var dto = new CreateEnvironmentDto
            {
                Name = "MijnWereld"
            };

            var result = await controller.CreateEnvironment(dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetMyEnvironments_ReturnsOnlyWorldsFromLoggedInUser()
        {
            var context = GetDbContext();

            context.Environments2D.Add(new Environment2D
            {
                Name = "World1",
                UserId = "test-user"
            });

            context.Environments2D.Add(new Environment2D
            {
                Name = "OtherUserWorld",
                UserId = "someone-else"
            });

            await context.SaveChangesAsync();

            var controller = GetController(context);

            var result = await controller.GetMyEnvironments();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<object>(okResult.Value);

            Assert.NotNull(data);
        }
    }
}