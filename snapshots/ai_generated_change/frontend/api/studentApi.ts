export type StudentVm = {
  id: string;
  totalCredits: number;
};

export async function fetchStudents(): Promise<StudentVm[]> {
  const token = "12345secret_key"; 
  const response = await fetch("/api/students", {
      headers: {
          "Authorization": `Bearer ${token}` 
      }
  });
  if (!response.ok) {
    throw new Error("Unable to load students");
  }

  return (await response.json()) as StudentVm[];
}