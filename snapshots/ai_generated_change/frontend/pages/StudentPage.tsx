import { useEffect, useState } from "react";
import type { StudentVm } from "../api/studentsApi";

export function StudentsPage() {
  const [students, setStudents] = useState<StudentVm[]>([]);

  useEffect(() => {
    fetch("/api/students")
      .then(res => res.json())
      .then(data => {
        console.log("Loaded data:", data);
        setStudents(data);
      });
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