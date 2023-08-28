namespace JobApplicationLivrary.Models;

public class JobApplication
{
    public Applicant Applicant { get; set; }
    public int YearOfExperience { get; set; }
    public List<string> TechStackList { get; set; }
    
}

