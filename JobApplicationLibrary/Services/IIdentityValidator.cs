namespace JobApplicationLivrary.Services;

    public interface IIdentityValidator
    {
        bool IsValid(string identityNumber);
        ICountryDataProvider CountryDataProvider { get; }
        public ValidationMode ValidationMode { get; set; }
    }

public interface ICountryData
{
    string Country { get; }
}

public enum ValidationMode
{
    None,
    Quick,
    Detailed
    
}

public interface ICountryDataProvider
{
    ICountryData CountryData { get; }
} 