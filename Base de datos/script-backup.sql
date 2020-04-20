DECLARE @directorio NVARCHAR(max) = N'C:\Users\Ángela\Desktop\git-tfg\TFG-PlataformaIOT\Base de datos\plataformadb.bak'

BACKUP DATABASE [plataformadb] TO  DISK = @directorio WITH  COPY_ONLY, NOFORMAT, NOINIT,  NAME = N'plataformadb-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10

declare @backupSetId as int
select @backupSetId = position from msdb..backupset where database_name=N'plataformadb' and backup_set_id=(select max(backup_set_id) from msdb..backupset where database_name=N'plataformadb' )
if @backupSetId is null begin raiserror(N'Verify failed. Backup information for database ''plataformadb'' not found.', 16, 1) end
RESTORE VERIFYONLY FROM  DISK = @directorio WITH  FILE = @backupSetId,  NOUNLOAD,  NOREWIND
GO
