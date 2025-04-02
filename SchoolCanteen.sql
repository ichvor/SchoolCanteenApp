create database SchoolCanteen
GO
CREATE TABLE Class
  IdClass int IDENTITY(1,1) NOT NULL,
  Class nvarchar(15) NOT NULL,
  IdTeacher int NULL)
GO
CREATE TABLE Day(
  Id int IDENTITY(1,1) NOT NULL,
  DayName nvarchar(25) NOT NULL)
GO
CREATE TABLE Dish(
  IdDish int IDENTITY(1,1) NOT NULL,
  DishName nvarchar(200) NOT NULL,
  Price decimal(10, 2) NOT NULL)
GO
CREATE TABLE DishIngredient(
  IdDish int NOT NULL,
  IdIngredient int NOT NULL,)
GO
CREATE TABLE Ingredient(
  IdIngredient int IDENTITY(1,1) NOT NULL,
  IngredientName nvarchar(100) NOT NULL)
GO
CREATE TABLE Meal(
  IdMeal int IDENTITY(1,1) NOT NULL,
  MealType nvarchar(10) NOT NULL)
GO
CREATE TABLE dbo.MealDish(
  IdMeal int NOT NULL,
  IdDish int NOT NULL)
GO
CREATE TABLE MealPlan(
  IdMealPlan int IDENTITY(1,1) NOT NULL,
  IdStudent int NULL,
  IdMeal int NULL,
  IdPaid int NULL,
  IdDay int NULL)
GO
CREATE TABLE Paid(
  IdPaid int IDENTITY(1,1) NOT NULL,
  Paid nvarchar(35) NOT NULL)
GO
CREATE TABLE Student(
  IdStudent int IDENTITY(1,1) NOT NULL,
  FirstName nvarchar(100) NOT NULL,
  LastName nvarchar(50) NOT NULL)
  IdClass int NULL,
GO
CREATE TABLE Teacher(
  IdTeacher int IDENTITY(1,1) NOT NULL,
  FirstName nvarchar(100) NOT NULL,
  LastName nvarchar(50) NOT NULL)

GO
ALTER TABLE Class  WITH CHECK ADD FOREIGN KEY(IdTeacher)
REFERENCES Teacher (IdTeacher)
GO
ALTER TABLE DishIngredient  WITH CHECK ADD FOREIGN KEY(IdDish)
REFERENCES Dish (IdDish)
ON DELETE CASCADE
GO
ALTER TABLE DishIngredient  WITH CHECK ADD FOREIGN KEY(IdIngredient)
REFERENCES Ingredient (IdIngredient)
ON DELETE CASCADE
GO
ALTER TABLE MealDish  WITH CHECK ADD FOREIGN KEY(IdDish)
REFERENCES Dish (IdDish)
GO
ALTER TABLE MealDish  WITH CHECK ADD FOREIGN KEY(IdMeal)
REFERENCES Meal (IdMeal)
GO
ALTER TABLE MealPlan  WITH CHECK ADD FOREIGN KEY(IdMeal)
REFERENCES Meal (IdMeal)
GO
ALTER TABLE MealPlan  WITH CHECK ADD FOREIGN KEY(IdPaid)
REFERENCES Paid (IdPaid)
GO
ALTER TABLE MealPlan  WITH CHECK ADD FOREIGN KEY(IdStudent)
REFERENCES Student (IdStudent)
GO
ALTER TABLE MealPlan  WITH CHECK ADD  CONSTRAINT FK_MealPlan_Day FOREIGN KEY(IdDay)
REFERENCES Day (Id)
GO
ALTER TABLE MealPlan CHECK CONSTRAINT FK_MealPlan_Day
GO
ALTER TABLE Student  WITH CHECK ADD FOREIGN KEY(IdClass)
REFERENCES Class (IdClass)
GO
