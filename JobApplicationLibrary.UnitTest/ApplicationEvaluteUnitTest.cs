using JobApplicationLivrary;
using JobApplicationLivrary.Models;
using JobApplicationLivrary.Services;

namespace JobApplicationLibraryUnitTest;

public class ApplicationEvaluteUnitTest
{
    [SetUp]
    public void Setup()
    {
    }
    
    //unitofwork_condition_Expectedresult
    [Test]
    public void ApplicationWithUnderAge_TransferredToAutoRejected()
    {
        var mockValidator = new Mock<IIdentityValidator>();
        mockValidator.Setup(i=>i.IsValid(It.IsAny<string>())).Returns(true);
        //Arrange
        JobApplication application = new()
        {
            Applicant = new(){Age = 10},
        };
        var evulator = new ApplicationEvulator(mockValidator.Object);
        
        //Action
        var appResult = evulator.Evulate(application);
        
        //Assert
        appResult.Should().Be(ApplicationResult.AutoRejected);
    }
    
    [Test]
    public void Application_WithNoTechStack_TransferredToAutoRejected()
    {
        var mockValidator = new Mock<IIdentityValidator>();
        mockValidator.DefaultValue = DefaultValue.Mock; //set default value for properties 
        mockValidator.Setup(i=>i.CountryDataProvider.CountryData.Country).Returns("TURKEY");
        mockValidator.Setup(i=>i.IsValid(It.IsAny<string>())).Returns(true);
        //Arrange
        JobApplication application = new()
        {
            Applicant = new(){Age = 20, IdentityNumber = "sd"},
            YearOfExperience = 20,
            TechStackList = new(){"C#", "RabbitMQ", "Microservice"}
        };
        var evulator = new ApplicationEvulator(mockValidator.Object);
        
        //Action
        var appResult = evulator.Evulate(application);
        
        //Assert
        appResult.Should().Be(ApplicationResult.AutoAccepted);
    }
    
    [Test]
    public void Application_WithTechStackOver75P_TransferredToAutoAccepted()
    {
        var mockValidator = new Mock<IIdentityValidator>();
        mockValidator.DefaultValue = DefaultValue.Mock; //set default value for properties 
        mockValidator.Setup(i=>i.CountryDataProvider.CountryData.Country).Returns("TURKEY");
        mockValidator.Setup(i=>i.IsValid(It.IsAny<string>())).Returns(true);
        
        // mockValidator.Setup(i=>i.IsValid(It.IsAny<string>())).Returns(true);
        //Arrange
        JobApplication application = new()
        {
            Applicant = new(){Age = 20, IdentityNumber = "sd"},
            YearOfExperience = 20,
            TechStackList = new(){"C#", "RabbitMQ", "Microservice"}
        };
        var evulator = new ApplicationEvulator(mockValidator.Object);
        
        //Action
        var appResult = evulator.Evulate(application);
        
        //Assert
        appResult.Should().Be(ApplicationResult.AutoAccepted);
    }
    
    [Test]
    public void Application_WithInvalidIdantityNumber_TransferredToHR()
    {
             var mockValidator = new Mock<IIdentityValidator>();
             mockValidator.DefaultValue = DefaultValue.Mock; //set default value for properties 
             mockValidator.Setup(i=>i.CountryDataProvider.CountryData.Country).Returns("TURKEY");
             mockValidator.Setup(i=>i.IsValid(It.IsAny<string>())).Returns(false);
             //Arrange
             JobApplication application = new()
             {
                 Applicant = new(){Age = 20, IdentityNumber = "sd"},
             };
             var evulator = new ApplicationEvulator(mockValidator.Object);
             
             //Action
             var appResult = evulator.Evulate(application);
             
             //Assert
             appResult.Should().Be(ApplicationResult.TransferredToHR);
    }
         
         [Test]
         public void Application_WithOfficeLocation_TransferredToCTO()
         {
             var mockValidator = new Mock<IIdentityValidator>();
             mockValidator.Setup(i=>i.CountryDataProvider.CountryData.Country).Returns("SPAIN");
             //Arrange
             JobApplication application = new()
             {
                 Applicant = new(){Age = 20},
             };
             var evulator = new ApplicationEvulator(mockValidator.Object);
        
             //Action
             var appResult = evulator.Evulate(application);
        
             //Assert
             appResult.Should().Be(ApplicationResult.TransferredToCTO);
         }

         [Test]
         public void Application_WithOver50_ValidationModeToDetailed()
         {
             var mockValidator = new Mock<IIdentityValidator>();
             mockValidator.DefaultValue = DefaultValue.Mock;
             mockValidator.SetupAllProperties();
             //Arrange
             JobApplication application = new()
             {
                 Applicant = new(){Age = 51},
             };
             var evulator = new ApplicationEvulator(mockValidator.Object);
        
             //Action
             var appResult = evulator.Evulate(application);
        
             //Assert
             mockValidator.Object.ValidationMode.Should().Be(ValidationMode.Detailed);
         }
         
         [Test]
         public void Application_WithNullApplication_ThrowArgumentNullExpection()
         {
             var mockValidator = new Mock<IIdentityValidator>();
             //Arrange
             JobApplication application = new()
             {
                 // Applicant = new(){Age = 51},
             };
             var evulator = new ApplicationEvulator(mockValidator.Object);
        
             //Action
             Action appResult = () =>  evulator.Evulate(application);
             //Assert
             appResult.Should().Throw<ArgumentNullException>();
             // mockValidator.Object.ValidationMode.Should().Be(ValidationMode.Detailed);
         }

         [Test]
         public void Application_WithDefaultValue_IsValidCalled()
         {
             var mockValidator = new Mock<IIdentityValidator>();
             mockValidator.DefaultValue = DefaultValue.Mock;
             mockValidator.Setup(i=>i.CountryDataProvider.CountryData.Country).Returns("TURKEY");
             
             //Arrange
             JobApplication application = new()
             {
                 Applicant = new(){Age = 19, IdentityNumber = "123"},
             };
             var evulator = new ApplicationEvulator(mockValidator.Object);
        
             //Action
             var appResult = evulator.Evulate(application);
        
             //Assert
             mockValidator.Verify(i=>i.IsValid(It.IsAny<string>()),failMessage: "ısValidMethod should be called with 123");
         }
         
         [Test]
         public void Application_WithYoungAge_IsValidNeverCalled()
         {
             var mockValidator = new Mock<IIdentityValidator>();
             mockValidator.DefaultValue = DefaultValue.Mock;
             mockValidator.Setup(i=>i.CountryDataProvider.CountryData.Country).Returns("TURKEY");
             
             //Arrange
             JobApplication application = new()
             {
                 Applicant = new(){Age = 15},
             };
             var evulator = new ApplicationEvulator(mockValidator.Object);
        
             //Action
             var appResult = evulator.Evulate(application);
        
             //Assert
             mockValidator.Verify(i=>i.IsValid(It.IsAny<string>()),times: Times.Never);
         }
         
         [Test]
         public void Application_WithYoungAge_IsValidThreeTimesCalled()
         {
             var mockValidator = new Mock<IIdentityValidator>();
             mockValidator.DefaultValue = DefaultValue.Mock;
             mockValidator.Setup(i=>i.CountryDataProvider.CountryData.Country).Returns("TURKEY");
             
             //Arrange
             JobApplication application = new()
             {
                 Applicant = new(){Age = 15},
             };
             var evulator = new ApplicationEvulator(mockValidator.Object);
        
             //Action
             var appResult = evulator.Evulate(application);
        
             //Assert
             mockValidator.Verify(i=>i.IsValid(It.IsAny<string>()),times: Times.Exactly(3));
         }
}