CREATE DATABASE QuanLyQuanCafe
GO

USE QuanLyQuanCafe
GO

-- Food
-- Table
-- FoodCategory
-- Account
-- Bill
-- BillInfo

CREATE TABLE TableFood
(
	ID	INT IDENTITY PRIMARY KEY,
	Name	NVARCHAR(100) NOT NULL DEFAULT N'Chưa Đặt Tên',
	Status	NVARCHAR(100) DEFAULT N'Trống'    -- Trống || Có Người
)
GO

CREATE TABLE Account
(
	UserName	NVARCHAR(100) PRIMARY KEY,
	DisplayName	NVARCHAR(100)	NOT	NULL DEFAULT N'Chưa Đặt Tên',
	PassWord	NVARCHAR(100)	NOT NULL DEFAULT N'123',
	Type	INT NOT NULL DEFAULT 0 -- 1: Admin || 0: Staff
)
GO

CREATE TABLE FoodCategory
(
	ID INT IDENTITY PRIMARY KEY,
	Name	NVARCHAR(100) NOT NULL DEFAULT N'Chưa Đặt Tên'
)
GO

CREATE TABLE Food
(
	ID INT IDENTITY PRIMARY KEY,
	Name	NVARCHAR(100) NOT NULL DEFAULT N'Chưa Đặt Tên',
	IDCategory	INT	NOT NULL,
	Price	Float	NOT NULL DEFAULT 0

	FOREIGN KEY (IDCategory) REFERENCES dbo.FoodCategory(id)
)
GO

CREATE TABLE Bill
(
	ID INT IDENTITY PRIMARY KEY,
	DateCheckIn	DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE,
	IDTable INT NOT NULL,
	Status	INT NOT NULL DEFAULT 0, -- 1: đã thanh toán || 0: chưa thanh toán
	Discount INT DEFAULT 0, 
	TotalPrice FLOAT,

	FOREIGN KEY (IDTable) REFERENCES dbo.TableFood(id)
)
GO

CREATE TABLE BillInfo
(
	ID INT IDENTITY PRIMARY KEY,
	IDBill	INT NOT NULL,
	IDFood	INT NOT NULL,
	COUNT INT NOT NULL DEFAULT 0

	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
)
GO

INSERT INTO dbo.Account
(
    UserName,
    DisplayName,
    PassWord,
    Type
)
VALUES
(   N'VoXuanAn', -- UserName - nvarchar(100)
    N'Art7mis', -- DisplayName - nvarchar(100)
    N'3244185981728979115075721453575112', -- PassWord - nvarchar(100)
    1    -- Type - int
    )
GO

INSERT INTO dbo.Account
(
    UserName,
    DisplayName,
    PassWord,
    Type
)
VALUES
(   N'staff', -- UserName - nvarchar(100)
    N'staff', -- DisplayName - nvarchar(100)
    N'3244185981728979115075721453575112', -- PassWord - nvarchar(100)
    0    -- Type - int
    )
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetAccountByUserName')
	DROP PROCEDURE	USP_GetAccountByUserName
GO

CREATE PROC USP_GetAccountByUserName
	@userName nvarchar(100)
AS
	BEGIN
		SELECT * FROM dbo.Account WHERE UserName = @userName;
	END
GO


IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_Login')
	DROP PROCEDURE	USP_Login
GO

CREATE PROC USP_Login
	@userName nvarchar(100),
	@passWord NVARCHAR(100)
AS
	BEGIN
		SELECT * FROM dbo.Account WHERE UserName = @userName AND PassWord = @passWord;
	END
GO

DECLARE @i INT = 0;
WHILE @i< 11 
BEGIN
	SET @i = @i+1;
	IF NOT EXISTS (SELECT * FROM dbo.TableFood WHERE Name =  N'Bàn ' +  CAST(@i AS NVARCHAR(100)))
	INSERT INTO dbo.TableFood (Name) VALUES(  N'Bàn ' +  CAST(@i AS NVARCHAR(100)))
END
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetTableList')
	DROP PROCEDURE	USP_GetTableList
GO

CREATE PROC USP_GetTableList
AS SELECT * FROM dbo.TableFood
GO

--EXEC dbo.USP_GetTableList
--GO

-- Thêm category
INSERT dbo.FoodCategory(Name) VALUES(N'Coffe')
INSERT dbo.FoodCategory(Name) VALUES(N'Soft Drink')
INSERT dbo.FoodCategory(Name) VALUES(N'CockTail')
INSERT dbo.FoodCategory(Name) VALUES(N'Smoothies')

-- Thêm food
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Espresso',1,35000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Latte macchiato',1,40000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Caramel macchiato',1,45000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Flat white',1,43000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Cappuccino',1,45000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Doppio',1,39000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Irish coffee',1,5000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Americano',1,30000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Coke',2,15000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Red bull',2,20000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'7-Up',2,15000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Daiquiri',3,60000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Margarita',3,65000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Mojito',3,45000.0)
INSERT dbo.Food(Name,IDCategory,Price)VALUES(N'Fruit Smoothie',4,40000.0)

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetUncheckBillIDByTableID')
	DROP PROCEDURE	USP_GetUncheckBillIDByTableID
GO

CREATE PROC USP_GetUncheckBillIDByTableID
	@idTable INT,
	@status INT
AS
BEGIN
	SELECT * FROM dbo.Bill WHERE IDTable = @idTable AND Status = @status
END
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetListBillInfoByBillID')
	DROP PROCEDURE	USP_GetListBillInfoByBillID
GO

CREATE PROC  USP_GetListBillInfoByBillID
	@BillID INT
AS SELECT * FROM dbo.BillInfo WHERE IDBill = @BillID
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetListMenuByTable')
	DROP PROCEDURE	USP_GetListMenuByTable
GO

CREATE PROC USP_GetListMenuByTable
	@Idtable int
AS
BEGIN
	SELECT Name, COUNT, Price, Price * COUNT AS TotalPrice
	FROM Food AS F
		JOIN BillInfo AS BI ON F.ID = BI.IDFood
		JOIN BILL AS B ON BI.IDBill = B.ID
	WHERE B.status = 0 AND B.idTable = @Idtable
END
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_SelectCategory')
	DROP PROCEDURE	USP_SelectCategory
GO

CREATE PROC USP_SelectCategory
AS
	SELECT * FROM FoodCategory
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetListFoodByCategoryID')
	DROP PROCEDURE	USP_GetListFoodByCategoryID
GO

CREATE PROC USP_GetListFoodByCategoryID
@categoryID int
AS
	SELECT * FROM Food WHERE IDCategory = @categoryID
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_InsertBill')
	DROP PROCEDURE	USP_InsertBill
GO

CREATE PROC USP_InsertBill
@idTable int
AS
BEGIN
	INSERT dbo.Bill
	(
	    DateCheckIn,
	    DateCheckOut,
	    IDTable,
	    Status,
		Discount
	)
	VALUES
	(   GETDATE(), -- DateCheckIn - date
	    NULL, -- DateCheckOut - date
	    @idTable,         -- IDTable - int
	    0,
		0-- Status - int
	    )
END
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_InsertBillInfo')
	DROP PROCEDURE	USP_InsertBillInfo
GO

CREATE PROC USP_InsertBillInfo
@idBill int, @idFood int, @count int
AS
BEGIN
	DECLARE @isExistBillInfo INT;
	DECLARE @foodcount INT = 0;

	SELECT @isExistBillInfo = ID, @foodcount = COUNT FROM dbo.BillInfo WHERE idBill = @idBill AND IDFood = @idFood

	IF (@isExistBillInfo > 0) 
	BEGIN
		DECLARE @newCount INT = @foodcount + @count
		
		IF (@newCount > 0)
			UPDATE dbo.BillInfo SET COUNT = @foodcount + @count WHERE IDFood =@idFood AND IDBill = @idBill
		ELSE
				DELETE dbo.BillInfo WHERE IDBill = @idBill AND IDFood = @idFood
	END
	ELSE IF (@count > 0)
		BEGIN
			INSERT dbo.BillInfo(IDBill,IDFood,COUNT)
			VALUES
			(   @idBill, -- IDBill - int
				@idFood, -- IDFood - int
				@count  -- COUNT - int
			)
		END
END
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetMaxIdBill')
	DROP PROCEDURE	USP_GetMaxIdBill
GO

CREATE PROC USP_GetMaxIdBill
AS SELECT MAX(id) FROM dbo.Bill
GO 

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_UpdateTable')
	DROP PROCEDURE	USP_UpdateTable
GO

CREATE PROC USP_UpdateTable
	@id INT, @discount INT, @totalPrice FLOAT
AS UPDATE dbo.Bill SET DateCheckOut = GETDATE(),  Status = 1, Discount = @discount, TotalPrice = @totalPrice WHERE ID = @id
GO	

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'UTG_UpdateBillInfo')
	DROP TRIGGER	UTG_UpdateBillInfo
GO

CREATE TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idbill INT
	
	SELECT @idbill = idbill FROM Inserted

	DECLARE @ibtable INT

	SELECT @ibtable = idtable FROM dbo.Bill WHERE id = @idbill AND Status = 0

	UPDATE dbo.TableFood SET Status = N'Có người' WHERE id = @ibtable

END
GO	

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'UTG_UpdateBill')
	DROP TRIGGER	UTG_UpdateBill
GO

CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR	UPDATE
AS
BEGIN
	DECLARE @idbill INT
	
	SELECT @idbill =id FROM Inserted

	DECLARE @ibtable INT

	SELECT @ibtable = idtable FROM dbo.Bill WHERE id = @idbill 
	
	DECLARE @count INT = 0

	SELECT @count = COUNT(*) FROM dbo.Bill  WHERE IDTable = @ibtable AND	Status = 0

	IF (@count = 0) UPDATE dbo.TableFood SET Status = N'Trống' WHERE ID = @ibtable
END
GO	


IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_SwitchTable1')
	DROP PROCEDURE	USP_SwitchTable1
GO

CREATE PROC USP_SwitchTable1
@IdTable1 INT, @IdTable2 INT
AS
	BEGIN
		DECLARE @idFirstBill INT
		DECLARE @idSecondBill INT

		SELECT @idFirstBill  = MAX(id) FROM dbo.Bill WHERE IDTable = @IdTable1 AND Status = 0
		SELECT @idSecondBill = MAX(id) FROM dbo.Bill WHERE IDTable = @IdTable2 AND Status = 0

		IF (@idSecondBill IS NULL)
		BEGIN
		INSERT dbo.Bill
			(
			    DateCheckIn,
			    DateCheckOut,
			    IDTable,
			    Status
			)
			VALUES
			(   GETDATE(), -- DateCheckIn - date
			    NULL, -- DateCheckOut - date
			    @IdTable2,         -- IDTable - int
			    0          -- Status - int
			    ) 

			SELECT @idSecondBill = MAX(id) FROM dbo.Bill WHERE IDTable = @IdTable2 AND Status = 0

			UPDATE dbo.Bill SET Status = 1 WHERE id = @idFirstBill
			
			UPDATE dbo.TableFood SET Status = N'Trống' WHERE ID = @IdTable1
		END

		
		
		SELECT ID INTO IDBillInfoTable FROM dbo.BillInfo WHERE IDBill = @idSecondBill;

		UPDATE dbo.BillInfo SET IDBill = @idSecondBill WHERE IDBill = @idFirstBill

		UPDATE dbo.BillInfo SET IDBill = @idFirstBill WHERE ID IN (SELECT * FROM IDBillInfoTable)

		DROP TABLE IDBillInfoTable
END
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetListBillByDate')
	DROP PROCEDURE	USP_GetListBillByDate
GO

CREATE PROC USP_GetListBillByDate
@dateCheckIN DATE, @dateCheckOut DATE
AS BEGIN
	SELECT t.Name AS [Name], B.DateCheckIn AS [DateCheckIn], B.DateCheckOut AS [DateCheckOut],B.Discount AS [Discount], totalPrice [Totalprice]
	FROM dbo.Bill AS B
		JOIN dbo.TableFood AS T ON T.ID = B.IDTable
	WHERE DateCheckIn >= @dateCheckIN AND DateCheckOut <= @dateCheckOut AND B.Status = 1
END
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_UpdateAccount')
	DROP PROCEDURE	USP_UpdateAccount
GO

CREATE PROC USP_UpdateAccount
	@userName NVARCHAR(100), @displayName NVARCHAR(100), @password NVARCHAR(100), @newPassword NVARCHAR(100)
AS
	BEGIN
		DECLARE @isRightPass INT

		SELECT @isRightPass = COUNT(*) FROM	 dbo.Account WHERE @userName = UserName AND @password = PassWord

		IF (@isRightPass = 1)
		BEGIN
			IF (@newPassword IS NULL) OR (@newPassword = N'')
			BEGIN
				UPDATE dbo.Account SET DisplayName = @displayName WHERE UserName = @userName
			END
			ELSE
				UPDATE dbo.Account SET DisplayName = @displayName, PassWord = @newPassword WHERE UserName = @userName
		END
	END
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetListFood')
	DROP PROCEDURE	USP_GetListFood
GO

CREATE PROC USP_GetListFood
AS
	SELECT * FROM Food
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetCategoryByID')
	DROP PROCEDURE	USP_GetCategoryByID
GO

CREATE PROC USP_GetCategoryByID
@categoryId INT
AS
	SELECT * FROM dbo.FoodCategory WHERE ID = @categoryId
GO


IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_InsertFood')
	DROP PROCEDURE	USP_InsertFood
GO

CREATE PROC USP_InsertFood
@name NVARCHAR(100), @idCategory INT, @price FLOAT 
AS
	INSERT dbo.Food
	(
	    Name,
	    IDCategory,
	    Price
	)
	VALUES
	(   @name, -- Name - nvarchar(100)
	    @idCategory,   -- IDCategory - int
		@price  -- Price - float
	    )
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_UpdateFood')
	DROP PROCEDURE	USP_UpdateFood
GO

CREATE PROC USP_UpdateFood
@id INT ,@name NVARCHAR(100), @idCategory INT, @price FLOAT 
AS
	UPDATE dbo.Food SET Name = @name, IDCategory = @idCategory, Price = @price WHERE id = @id
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_DeleteBillInfoByFoodId')
	DROP PROCEDURE	USP_DeleteBillInfoByFoodId
GO

CREATE PROC USP_DeleteBillInfoByFoodId
@id INT 
AS
	DELETE FROM dbo.BillInfo WHERE IDFood = @id
GO


IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_DeleteFood')
	DROP PROCEDURE	USP_DeleteFood
GO

CREATE PROC USP_DeleteFood
@id INT
AS
	DELETE FROM dbo.Food WHERE ID = @id
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'UTG_DeleteBillInfo')
	DROP TRIGGER UTG_DeleteBillInfo
GO

CREATE TRIGGER UTG_DeleteBillInfo
ON dbo.BillInfo FOR DELETE
AS
BEGIN
	DECLARE @idBillInfo INT
	DECLARE @idBill INT
	SELECT @idBillInfo = id, @idBill = Deleted.IDBill FROM Deleted

	DECLARE @idTable INT
	SELECT @idTable = IDTable FROM dbo.Bill WHERE id = @idBill

	DECLARE @count INT = 0
	SELECT @count = COUNT(*) FROM dbo.BillInfo AS BI, dbo.Bill AS B WHERE b.ID = bi.IDBill AND b.ID = @idBill AND b.Status = 0

	IF (@count = 0)
		UPDATE dbo.TableFood SET Status = N'Trống' WHERE id = @idTable
END
GO

CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_SearchFoodByName')
	DROP PROCEDURE	USP_SearchFoodByName
GO

CREATE PROC USP_SearchFoodByName
@name NVARCHAR(100)
AS
	SELECT * FROM Food WHERE dbo.fuConvertToUnsign1(Name) LIKE dbo.fuConvertToUnsign1('%'+@name+'%')
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_InsertCategory')
	DROP PROCEDURE	USP_InsertCategory
GO

CREATE PROC USP_InsertCategory
@name NVARCHAR(100)
AS
	INSERT dbo.FoodCategory
	(
	    Name
	)
	VALUES
	(@name -- Name - nvarchar(100)
	)
GO	

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_UpdateCategory')
	DROP PROCEDURE	USP_UpdateCategory
GO

CREATE PROC USP_UpdateCategory
@id INT ,@name NVARCHAR(100)
AS
	UPDATE dbo.FoodCategory SET Name = @name WHERE ID = @id
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_DeleteBillInfoByCategoryId')
	DROP PROCEDURE	USP_DeleteBillInfoByCategoryId
GO

CREATE PROC USP_DeleteBillInfoByCategoryId
@id INT 
AS
	DELETE FROM dbo.BillInfo WHERE IDFood IN (SELECT IDFood FROM dbo.FoodCategory WHERE ID = @id)
GO


IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_DeleteFoodByCategoryId')
	DROP PROCEDURE	USP_DeleteFoodByCategoryId
GO

CREATE PROC USP_DeleteFoodByCategoryId
@id INT 
AS
	DELETE FROM dbo.Food WHERE IDCategory = @id
GO



IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_DeleteCategory')
	DROP PROCEDURE	USP_DeleteCategory
GO

CREATE PROC USP_DeleteCategory
@id INT
AS
	DELETE FROM dbo.FoodCategory WHERE ID = @id
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_InsertTable')
	DROP PROCEDURE	USP_InsertTable
GO

CREATE PROC USP_InsertTable
@name NVARCHAR(100)
AS
	INSERT dbo.TableFood
	(
	    Name,
	    Status
	)
	VALUES
	(   @name, -- Name - nvarchar(100)
	    N'Trống'  -- Status - nvarchar(100)
	    )
GO	

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_UpdateTable2')
	DROP PROCEDURE	USP_UpdateTable2
GO

CREATE PROC USP_UpdateTable2
@id INT, @name NVARCHAR(100)
AS
	UPDATE dbo.TableFood SET Name = @name WHERE ID = @id
GO	


IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_DeleteBillInfoByTableId')
	DROP PROCEDURE	USP_DeleteBillInfoByTableId
GO

CREATE PROC USP_DeleteBillInfoByTableId
@id INT 
AS
	DELETE FROM dbo.BillInfo WHERE IDBill IN (SELECT b.ID FROM dbo.TableFood AS T JOIN dbo.Bill AS B ON B.IDTable = T.ID WHERE B.IDTable = @id)
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_DeleteBillByTableId')
	DROP PROCEDURE	USP_DeleteBillByTableId
GO

CREATE PROC USP_DeleteBillByTableId
@id INT 
AS
	DELETE FROM dbo.Bill WHERE IDTable = @id
GO


IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_DeleteTable')
	DROP PROCEDURE	USP_DeleteTable
GO

CREATE PROC USP_DeleteTable
@id INT
AS
	DELETE FROM dbo.TableFood WHERE	ID =@id
GO


IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetListAccount')
	DROP PROCEDURE	USP_GetListAccount
GO

CREATE PROC USP_GetListAccount
AS
	SELECT UserName, DisplayName, Type FROM dbo.Account
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_InsertAccount')
	DROP PROCEDURE	USP_InsertAccount
GO

CREATE PROC USP_InsertAccount
@userName NVARCHAR(100), @displayName NVARCHAR(100), @type INT
AS
	INSERT dbo.Account
	(
	    UserName,
	    DisplayName,
	    PassWord,
	    Type
	)
	VALUES
	(   @userName, -- UserName - nvarchar(100)
	    @displayName, -- DisplayName - nvarchar(100)
	    N'3244185981728979115075721453575112', -- PassWord - nvarchar(100)
	    @type    -- Type - int
	    )
GO	

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_UpdateAccount2')
	DROP PROCEDURE	USP_UpdateAccount2
GO

CREATE PROC USP_UpdateAccount2
@userName NVARCHAR(100), @displayName NVARCHAR(100), @type INT
AS
	UPDATE dbo.Account SET DisplayName = @displayName, Type = @type WHERE UserName = @userName
GO


IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_DeleteAccount')
	DROP PROCEDURE	USP_DeleteAccount
GO

CREATE PROC USP_DeleteAccount
@userName NVARCHAR(100)
AS
	DELETE FROM dbo.Account WHERE UserName = @userName
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_ResetAccountPassword')
	DROP PROCEDURE	USP_ResetAccountPassword
GO

CREATE PROC USP_ResetAccountPassword
@userName NVARCHAR(100)
AS
	UPDATE dbo.Account SET PassWord = N'3244185981728979115075721453575112' WHERE @userName = UserName
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetListBillByDateAndPage')
	DROP PROCEDURE	USP_GetListBillByDateAndPage
GO

CREATE PROC USP_GetListBillByDateAndPage
@dateCheckIN DATE, @dateCheckOut DATE, @page INT, @pageSize INT
AS 
BEGIN
	DECLARE @selectRows INT = @pageSize* @page
	DECLARE @exceptRows INT = @pageSize*(@page - 1)+1

	SELECT *
	FROM  (	SELECT ROW_NUMBER() OVER ( ORDER BY T.Name ) AS [STT],t.Name AS [Tên bàn], B.DateCheckIn AS [Ngày check in], B.DateCheckOut AS [Ngày check out],B.Discount AS [Giảm giá], totalPrice [Tổng tiền]
			FROM dbo.Bill AS B
				JOIN dbo.TableFood AS T ON T.ID = B.IDTable
			WHERE DateCheckIn >= @dateCheckIN AND DateCheckOut <= @dateCheckOut AND B.Status = 1) AS RowConstrainedResult
	WHERE RowConstrainedResult.[STT] BETWEEN @exceptRows AND @selectRows
END
GO

IF EXISTS	(SELECT	*
			FROM sys.objects
			WHERE name = 'USP_GetCountBillByDate')
	DROP PROCEDURE	USP_GetCountBillByDate
GO

CREATE PROC USP_GetCountBillByDate
@dateCheckIN DATE, @dateCheckOut DATE
AS BEGIN
	SELECT COUNT(*)
	FROM dbo.Bill AS B
		JOIN dbo.TableFood AS T ON T.ID = B.IDTable
	WHERE DateCheckIn >= @dateCheckIN AND DateCheckOut <= @dateCheckOut AND B.Status = 1
END
GO