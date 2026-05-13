namespace University.Api.Domain;

public sealed class Student
{
    public required string Id { get; init; }
    public required List<CourseItem> EnrolledCourses { get; init; }
}

public sealed class CourseItem
{
    public required string Code { get; init; }
    public int Credits { get; init; }
}