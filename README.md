# מערכת תורים למספרת כלבים

פרויקט מבחן בית למימוש מערכת תורים למספרת כלבים..
קובץ יצירת מסד הנתונים המלא נמצא בתיקיית Database.

## טכנולוגיות

### צד שרת
- .NET 8 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- BCrypt

### צד לקוח
- React
- TypeScript
- Material UI
- Axios

## יכולות המערכת

- הרשמת משתמש חדש
- התחברות למערכת
- אימות משתמשים באמצעות JWT
- צפייה ברשימת תורים
- הוספת תור חדש
- עריכת תור
- מחיקת תור
- סינון לפי שם לקוח
- סינון לפי טווח תאריכים
- הצגת פרטי תור בחלון קופץ
-הנחה של 10% ללקוחות בעלי יותר מ־3 תורים בעבר

## אבטחת מידע

- סיסמאות נשמרות בצורה מוצפנת באמצעות BCrypt
- אימות משתמשים באמצעות JWT
- משתמש יכול לערוך ולמחוק רק תורים השייכים לו
-לא ניתן למחוק תור שנקבע לאותו יום

## מסד נתונים
לנוחות הבדיקה צורף לפרויקט קובץ:

`Database/DatabaseScript.sql`

הקובץ כולל:

- יצירת בסיס הנתונים DogGroomingQueue
- יצירת הטבלאות
- הזנת נתוני בסיס לטבלת סוגי התספורות
- יצירת ה־View
- יצירת ה־Stored Procedure

ניתן להריץ את הקובץ ב־SQL Server לצורך הקמה מלאה של מסד הנתונים.

## מבנה בסיס הנתונים

### טבלאות

#### User_Ta
טבלת משתמשי המערכת.

שדות עיקריים:
- UserId_Int
- Username_Vch
- PasswordHash_Vch
- FirstName_Vch
- CreatedAt_Dat

#### DogGroomingTypes_Ta
טבלת סוגי התספורות.

נתונים לדוגמה:
- כלב קטן – 30 דקות – ₪100
- כלב בינוני – 45 דקות – ₪150
- כלב גדול – 60 דקות – ₪200

#### Appointments_Ta
טבלת התורים.

שדות עיקריים:
- AppointmentId_Int
- UserId_Int
- GroomingTypeId_Int
- AppointmentDateTime_Dat
- CreatedAt_Dat
- IsDeleted_Bit

### View

#### vw_AppointmentsDetailsSwish

ה־View משמש להצגת נתוני התורים ומבצע Join בין:

- User_Ta
- Appointments_Ta
- DogGroomingTypes_Ta

ה־View מחזיר:
- שם הלקוח
- סוג הכלב
- מחיר
- משך הטיפול
- תאריך התור
- תאריך יצירת התור

### Stored Procedure

#### sp_CreateAppointment

משמש ליצירת תור חדש במערכת.

פרמטרים:
- UserId
- GroomingTypeId
- AppointmentDateTime

הפרוצדורה מוסיפה תור חדש ומחזירה את מזהה התור שנוצר
## הרצת הפרויקט

### צד שרת

```bash
dotnet restore
dotnet run
```

### צד לקוח

```bash
npm install
npm run dev
```
.
קישורים לפרויקט

Backend:
https://github.com/dvorak880/DogGroomingQueue

Frontend:
https://github.com/dvorak880/dog-grooming-client
## מפתחת

דבורה
