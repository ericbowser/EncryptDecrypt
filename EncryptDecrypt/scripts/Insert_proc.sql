USE [KeyData]
GO
/****** Object:  StoredProcedure [dbo].[InsertKey]    Script Date: 6/2/2024 2:49:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    --"Modulus": "214Hi1pGFYFLB1+XtNgVRxHH/puH5QMLn8sCaVteuOizrYVPXL3/OQgtYMTxCEjSwbGsH4zK8sTdxSDmgVWG0BYzKdTxDbgBI+StCfEiS8OzuLDo34nVx9cKAQexwz2a5OOJ0rs4ZLWvebpmyF4Q535n/9X+t/guUkY/GxNK7vkZ8tdiyoEoZFlag3ewFrQIm0DgJKLodKLEneT7VPrpp8r9uZq7rwjIAlCOZ7rTe/8XJhGPQheGV/vzRd6RRCRYRyr5Kn5+xrY1eEQMRL5pbKEMvqaK6484RPsEJnmf9Xz1I1eOx64PgZscqMSOqlJmoMn1HcMXmXQ4j+cqDlPpZQ==",
    --"Exponent": "AQAB",
    --"P": null,
    --"Q": null,
    --"DP": null,
    --"DQ": null,
    --"InverseQ": null,
    --"D": null
ALTER   PROCEDURE [dbo].[InsertKey] 
   @User varchar(100) = [User], /* Input Parameter - optional default value */
   @JsonKey varchar(MAX) = [JsonKey],
   @Password varchar(100) = [Password],
   @output_parameter int OUTPUT     /* Output Parameter (if needed) */
AS
BEGIN
   -- Start of the SQL logic
   SET NOCOUNT ON;   -- This prevents the message about the number of rows affected by a T-SQL statement from being returned as part of the result set.

   INSERT INTO dbo.Credentials([user], password)
	VALUES(@User, @Password);

   -- Your SQL statements here
   DECLARE @CredentialId INT = NULL;
   SET @CredentialId = (SELECT id FROM [dbo].[Credentials] 
				WHERE [user] = @User);

   IF @CredentialId > 0
   BEGIN
	INSERT INTO [dbo].[KeyObject]
           (JsonKey
           ,[CredentialId])
     VALUES (
		   @JsonKey,
		   @CredentialId)
   END

   -- If using output parameters, assign a value to the output parameter
   SET @output_parameter = (SELECT COUNT(*) FROM [dbo].[Credentials] WHERE [user] = @user);
   PRINT @output_parameter;

   IF @output_parameter != null
   BEGIN
	   SET @output_parameter = (
		SELECT COUNT(*) FROM [dbo].[Credentials] 
			INNER JOIN dbo.KeyObject ON KeyObject.CredentialId = Credentials.id 
				WHERE [user] = @user);

	   -- End of the SQL logic
	   return @output_parameter;
   END
END;
