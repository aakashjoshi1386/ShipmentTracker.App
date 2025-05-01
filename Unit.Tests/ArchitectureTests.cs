using NetArchTest.Rules;

namespace Unit.Tests
{
    public class ArchitectureTests
    {
        private const string DomainNamespace = "ShipmentTracker.App.Domain";
        private const string ApplicationNamespace = "ShipmentTracker.App.Application";
        private const string InfrastructureNamespace = "ShipmentTracker.App.Infrastructure";
        private const string ApiNamespace = "ShipmentTracker.App.API";
        private const string UINamespace = "ShipmentTracker.App.UI";

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = typeof(ShipmentTracker.App.Domain.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
            ApplicationNamespace,
            InfrastructureNamespace,
            ApiNamespace,
            UINamespace
        };

            // Act
            var testResult = Types
                              .InAssembly(assembly)
                              .ShouldNot()
                              .HaveDependencyOnAll(otherProjects)
                              .GetResult();

            // Assert
            Assert.True(testResult.IsSuccessful);
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = typeof(ShipmentTracker.App.Application.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
            InfrastructureNamespace,
            ApiNamespace,
            UINamespace
        };
            // Act
            var testResult = Types
                                  .InAssembly(assembly)
                                  .ShouldNot()
                                  .HaveDependencyOnAll(otherProjects)
                                  .GetResult();
            // Assert
            Assert.True(testResult.IsSuccessful);
        }
        [Fact]
        public void InFrastructure_Should_Not_HaveDependencyOnOtherProejcts()
        {
            // Arrange
            var assembly = typeof(ShipmentTracker.App.Infrastructure.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
            ApiNamespace,
            UINamespace
        };
            // Act
            var testResult = Types
                                  .InAssembly(assembly)
                                  .ShouldNot()
                                  .HaveDependencyOnAll(otherProjects)
                                  .GetResult();
            // Assert
            Assert.True(testResult.IsSuccessful);
        }
        [Fact]
        public void Handlers_Should_Have_DependencyOnDomain()
        {
            // Arrange
            var assembly = typeof(ShipmentTracker.App.Application.AssemblyReference).Assembly;

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Handlers")
                .Should()
                .HaveDependencyOn(DomainNamespace)
                .GetResult();

            // Assert
            Assert.True(testResult.IsSuccessful);
        }
        [Fact]
        public void Controllers_Should_HaveDependencyOnMediatR()
        {
            // Arrange
            var assembly = typeof(ShipmentTracker.App.API.AssemblyReference).Assembly;
            // Act
            var testResult = Types
                                .InAssembly(assembly)
                                .That()
                                .HaveNameEndingWith("Controllers")
                                .Should()
                                .HaveDependencyOn("MediatR")
                                .GetResult();
            // Assert
            Assert.True(testResult.IsSuccessful);
        }
        [Fact]
        public void Api_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assmebly = typeof(ShipmentTracker.App.API.AssemblyReference).Assembly;
            var otherProjects = new[]
            {
           DomainNamespace
        };
            // Act
            var testResult = Types
                                .InAssembly(assmebly)
                                .ShouldNot()
                                .HaveDependencyOnAll(otherProjects)
                                .GetResult();
            // Assert
            Assert.True(testResult.IsSuccessful);
        }
    }
}