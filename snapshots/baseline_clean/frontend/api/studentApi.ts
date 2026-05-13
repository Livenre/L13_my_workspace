export type StudentVm = {
  id: string;
  totalCredits: number;
};

export async function fetchStudents(): Promise<StudentVm[]> {
  const response = await fetch("/api/students");
  if (!response.ok) {
    throw new Error("Unable to load students");
  }

  return (await response.json()) as StudentVm[];
}