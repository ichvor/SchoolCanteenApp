CREATE TRIGGER trg_PreventClassDelete
ON Class
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Student WHERE IdClass IN (SELECT IdClass FROM deleted))
    BEGIN
        RAISERROR('Невозможно удалить класс, так как в нем есть студенты.', 16, 1);
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        DELETE FROM Class WHERE IdClass IN (SELECT IdClass FROM deleted);
    END
END

GO

CREATE TRIGGER trg_PreventDishDelete
ON dbo.Dish
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM MealDish WHERE IdDish IN (SELECT IdDish FROM deleted))
    BEGIN
        RAISERROR('Невозможно удалить блюдо, так как оно используется в планах питания.', 16, 1);
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        DELETE FROM dbo.Dish WHERE IdDish IN (SELECT IdDish FROM deleted);
    END
END

GO

CREATE TRIGGER trg_PreventStudentDelete
ON Student
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM MealPlan WHERE IdStudent IN (SELECT IdStudent FROM deleted))
    BEGIN
        RAISERROR('Невозможно удалить студента, так как он связан с планами питания.', 16, 1);
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        DELETE FROM Student WHERE IdStudent IN (SELECT IdStudent FROM deleted);
    END
END
GO


