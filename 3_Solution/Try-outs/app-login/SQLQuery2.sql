select * from students
select * from Profesors

delete from Students
ALTER TABLE students
ADD Email VARCHAR(255);

ALTER TABLE profesors
ADD Email VARCHAR(255);

insert into students(Username) values('/3734');

CREATE TABLE Admins(
ID_Admin INT IDENTITY(1,1),
Nume VARCHAR(50),
Prenume VARCHAR(50),
Username VARCHAR(50),
AdminPassword VARCHAR(50),
Email VARCHAR(255)
);
select * from admins
select * from Students
select * from Profesors

SELECT COUNT(*) FROM Students WHERE Username='m4sh4' AND StdPassword='p2jhS56uF8Yt2XnHoRV/P+1WpzjwtWQ6gMeEASl/YG0=';


CREATE TABLE Users(
Username VARCHAR(255) NOT NULL PRIMARY KEY,
UserPassword VARCHAR(255) NOT NULL,
UserType VARCHAR(50) NOT NULL
);

ALTER TABLE Profesors
ADD CONSTRAINT FK_USER_PROF FOREIGN KEY(Username) REFERENCES Users(Username);

DROP TABLE Users

DROP TABLE Profesors

CREATE TABLE Professor (
    ID_Prof INT NOT NULL PRIMARY KEY,
    Nume VARCHAR(255) NOT NULL,
    Prenume VARCHAR(255) NOT NULL,
    Profesie_de_baza VARCHAR(255) NOT NULL,
	Username VARCHAR(255) NOT NULL,
	ProfPassword VARCHAR(255) NOT NULL,
	Email VARCHAR(255) NOT NULL
);

ALTER TABLE Professors
ADD CONSTRAINT FK_ID_PROF_USER FOREIGN KEY (ID_Prof) REFERENCES Users(ID_User)

-- Tabela "student"
CREATE TABLE student (
    ID_STUDENT INT IDENTITY(1,1) PRIMARY KEY,
    nume VARCHAR(255),
    prenume VARCHAR(255),
    universitate VARCHAR(255)
);
