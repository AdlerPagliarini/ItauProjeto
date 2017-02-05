CREATE DATABASE itauBD;

USE itauBD;

CREATE TABLE tCliente(
	idCliente int IDENTITY(1,1) NOT NULL,
	Email nvarchar(50) NOT NULL,
	Senha nvarchar(50) NOT NULL,
	CONSTRAINT PK_tCliente PRIMARY KEY (idCliente)
)
CREATE TABLE tClienteDados(
	idClienteDados int NOT NULL,
	Nome nvarchar(50) NOT NULL,
	DataNascimento datetime NOT NULL,
	CEP nvarchar(9) NOT NULL,
	Estado nvarchar(2) NOT NULL,
	Cidade nvarchar(50) NOT NULL,
	Bairro nvarchar(50) NOT NULL,
	Endereco nvarchar(100) NOT NULL,
	Numero nvarchar(10) NOT NULL,
	ComprovanteEndereco nvarchar(50) NOT NULL,
	CONSTRAINT PK_tClienteDados PRIMARY KEY (idClienteDados)
)

ALTER TABLE tClienteDados ADD CONSTRAINT FK_tClienteDados_idCliente FOREIGN KEY (idClienteDados) REFERENCES tCliente (idCliente)
ON DELETE CASCADE
ON UPDATE CASCADE;

















/*CREATE TRIGGER autoInsertWhenRegisterUser ON tCliente AFTER INSERT AS
 BEGIN
	BEGIN TRAN
		BEGIN TRY
		  DECLARE @idInserido int 
		  SET @idInserido = (SELECT idCliente FROM INSERTED)
		  BEGIN
			INSERT INTO tClienteDados 
					([idClienteDados],[Nome],[DataNascimento],[CEP],[Estado],[Cidade],[Bairro],[Endereco],[Numero],[ComprovanteEndereco])
					VALUES 
					(@idInserido,'nome','2000-01-01','cep','uf','cidade','bairro','endereco','numero','comprovante')
		  END
		  COMMIT TRAN
		END TRY
		BEGIN CATCH
		  IF @@TRANCOUNT > 0
			BEGIN
				PRINT 'Falha na trigger - autoInsertWhenRegisterUser'
				ROLLBACK TRAN
			END
		END CATCH
 END


 --Valida trigger
insert into tCliente (Email, Senha) VALUES ('Teste', '123')
select * from tCliente;
select * from tClienteDados;*/