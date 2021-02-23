﻿#Chrono
#2019-09: see C:\g\alex-pi\Src\AlexPiApi\ReadMe.md
#2021-02:
  Model gen-n:
  Instead of these 3:
      Install-Package Microsoft.EntityFrameworkCore
      Install-Package Microsoft.EntityFrameworkCore.SqlServer
      Install-Package Microsoft.EntityFrameworkCore.Tools
  ..installed all 3 straight from the Nuget manager.

  Scaffold-DbContext "Server=.\sqlexpress;Database=OneBase;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model
  Scaffold-DbContext "Server=.\sqlexpress;Database=RMS;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\RMS
  Scaffold-DbContext "Server=.\sqlexpress;Database=RMS;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\RMS -force
  Scaffold-DbContext "Server=.\sqlexpress;Database=BR;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\BR
  
  Scaffold-DbContext "Server=MTdevSQLDB;Database=RMS;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model
  