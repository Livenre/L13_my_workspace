Предметна область:
Я обрав стандарний домен "Університет" 
з сутностями: Student, Course, StudentEnrollmentService.
В завданні цей домен приведений, як стандартий.
Він простий і зрозумілий, на ньому можно чітко показати роботу governance-підхіду.

Список правил:
| RuleId | Severity | Що перевіряє                              | Чому важливо                                     |
| ARC101 | ERROR    | Забороняє виклик Sum().                   | Захищає від витоку бізнес-логіки в HTTP-шар.     |
| DEP201 | ERROR    | Забороняє using Infrastructure у Domain.  | Зберігає чистоту домену (Clean Architecture).    |
| SEC401 | ERROR    | Шукає захардкоджені токени у token = ".   | Запобігає витоку секретних ключів у репозиторій. |
| UI301  | WARNING  | Шукає прямий fetch( у UI-компонентах.     | Забезпечує використання state/api-шару.          |
| LOG301 | WARNING  | Шукає console.log у компонентах.          | Зберігає чистоту production-коду.                |
| DTO701 | WARNING  | Шукає повернення доменної сутності з API. | Гарантує, що API повертає лише DTO.              |


Результат трьох запусків:

Snapshot: snapshots/baseline_clean
Files scanned: 6
--------------------------------------------------------------------------------
No findings.
--------------------------------------------------------------------------------
Totals -> ERROR: 0, WARNING: 0
Gate decision: PASS

\\\\\\\\\\\\\\\\\\\\\\\\\\\\

Snapshot: snapshots/ai_generated_change
Files scanned: 6
--------------------------------------------------------------------------------
[ERROR] ARC101 :: backend/Controllers/StudentController.cs
  Message: Бізнес-логіка не повинна виконуватись у контролері.
  Recommendation: Винесіть обчислення в Application Service.
[ERROR] SEC401 :: frontend/api/studentApi.ts
  Message: Знайдено захардкоджений токен або пароль.
  Recommendation: Винесіть секретні дані у змінні середовища (.env).
[WARNING] UI301 :: frontend/pages/StudentPage.tsx
  Message: UI-компонент викликає API напряму.
  Recommendation: Використайте state/api-шар для доступу до HTTP.
[WARNING] LOG301 :: frontend/pages/StudentPage.tsx
  Message: Знайдено console.log у production-коді.
  Recommendation: Видаліть console.log або замініть на спеціалізований логер.
--------------------------------------------------------------------------------
Totals -> ERROR: 2, WARNING: 2
Gate decision: FAIL

\\\\\\\\\\\\\\\\\\\\\\\\\\\\

Snapshot: snapshots/fixed_after_review
Files scanned: 6
--------------------------------------------------------------------------------
No findings.
--------------------------------------------------------------------------------
Totals -> ERROR: 0, WARNING: 0
Gate decision: PASS


Порівняльна таблиця:
| Snapshot            | ERROR | WARNING | INFO | Рішення гейту |
| baseline_clean      | 0     | 0       | 0    | PASS          |
| ai_generated_change | 2     | 2       | 0    | FAIL          |
| fixed_after_review  | 0     | 0       | 0    | PASS          |

Опис виправлень:
1) ARC101: З файлу StudentsController.cs видалено прямий виклик courses.Sum(. 
Замість нього додано виклик методу CalculateTotalCredits з окремого сервісу StudentEnrollmentService.
2) SEC401: У файлі studentApi.ts на фронтенді видалено рядок token = 12345secret_key. 
Авторизацію налаштовано безпечно, токен тепер береться з process.env.
3) UI301: З файлу StudentPage.tsx видалено прямий запит fetch. 
Замість цього дані завантажуються через виклик функції loadStudents з API-шару.
4) LOG301: З файлу StudentPage.tsx прибрано рядок console.log, 
щоб не залишати зайву інформацію в готовому коді.

Фінальний висновок
Governance-підхід надає велику допомогу раозробникам при використанні ШІ.
Код генерюється і навіть працює але ШІ може загубити контекст чи зробити якісь моменти 
по своєму. Якщо це і не призведе до неправильної роботи то, точно, додасть 
технічного боргу, чого хотілося б уникнути. І governance-підхід тут допомогає, 
скануючи написаний ШІ код і виводить попередження при знаходжені (FAIL\WARNING) та 
надає рекомендації для ремонту цього коду, захищаючи проєкт від вищеописаних проблем.
