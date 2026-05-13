import { useEffect, useState } from "react";
import { loadStudents } from "../state/studentsStore";
import type { StudentVm } from "../api/studentsApi";

export function StudentsPage() {
  const [students, setStudents] = useState<StudentVm[]>([]);

  useEffect(() => {
    loadStudents().then(setStudents);
  }, []);

  return (
    <main>
      <h2>Students</h2>
      <ul>
        {students.map((x) => (
          <li key={x.id}>
            {x.id} - {x.totalCredits} credits
          </li>
        ))}
      </ul>
    </main>
  );
}