using JobApplicationLivrary.Models;
using JobApplicationLivrary.Services;

namespace JobApplicationLivrary;

public class ApplicationEvulator
{
    private readonly IIdentityValidator _identityValidator;
    private const int minAge = 18;
    private const int autoAcceptedYearOfExperience = 15;
    private List<string> techStackList = new() { "C#", "RabbitMQ", "Microservice", "Visual Studio" };

    public ApplicationEvulator(IIdentityValidator identityValidator)
    {
        _identityValidator = identityValidator;
    }

    public ApplicationResult Evulate(JobApplication jobApplication)
    {
        if (jobApplication.Applicant is null) throw new ArgumentNullException();

        if (jobApplication.Applicant.Age < minAge)
            return ApplicationResult.AutoRejected;

        _identityValidator.ValidationMode =
            jobApplication.Applicant.Age > 50 ? ValidationMode.Detailed : ValidationMode.Quick; 

        if (_identityValidator.CountryDataProvider.CountryData.Country != "TURKEY") return ApplicationResult.TransferredToCTO;
        
        var validIdentity = _identityValidator.IsValid(jobApplication.Applicant.IdentityNumber);
        if (!validIdentity)
            return ApplicationResult.TransferredToHR;

        var sr = GetTechStackSimilarityRate(jobApplication.TechStackList);
        if (sr < 25)
            return ApplicationResult.AutoRejected;

        if (sr > 75 && jobApplication.YearOfExperience >= autoAcceptedYearOfExperience)
            return ApplicationResult.AutoAccepted;

        return ApplicationResult.AutoAccepted;
    }
    private int GetTechStackSimilarityRate(List<string> techStacks)
    {
        var matchedCount =
            techStacks
                .Where(i => techStackList.Contains(i))
                .Count();

        return (int)(((double)matchedCount / techStackList.Count) * 100);
    }
}



public enum ApplicationResult
{
    AutoRejected,
    TransferredToHR,
    TransferredToLead,
    TransferredToCTO,
    AutoAccepted
}