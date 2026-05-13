import { fetchStudents } from "../api/studentsApi";

export async function loadStudents() {
  return fetchStudents();
}