namespace University.Api.Application;

public sealed class StudentEnrollmentService
{
    public int CalculateTotalCredits(List<Course> courses)
    {
        return courses.Sum(x => x.Credits);
    }
}

public sealed class Course
{
    public required string Name { get; init; }
    public int Credits { get; init; }
}