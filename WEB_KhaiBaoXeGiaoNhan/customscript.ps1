//ef core SqlServer
Scaffold-DbContext "Server=192.168.100.64;Database=Web_BookingTrans_Test;User Id=hoangnm;Password=123;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir WebModels -Force
Scaffold-DbContext "Server=192.168.100.64;Database=Web_BookingTrans;User Id=hoangnm;Password=123;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir WebModels -Force
Scaffold-DbContext "Server=192.168.100.66;Database=Vas_4000;User Id=sa;Password=123;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir WebModelsPMC -Force
//ef core PostgreSQL
Scaffold-DbContext "Host=satao.db.elephantsql.com;Database=nszmbjnd;Username=nszmbjnd;Password=QbOXY6X3aNfr0OtbXHpCeBKksE6qoFT-" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir WebModels -Force