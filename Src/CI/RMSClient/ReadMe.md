#Chrono
#2019-09: see C:\g\alex-pi\Src\AlexPiApi\ReadMe.md
#2021-02:
  Model gen-n:
  Instead of these 3:
      Install-Package Microsoft.EntityFrameworkCore
      Install-Package Microsoft.EntityFrameworkCore.SqlServer
      Install-Package Microsoft.EntityFrameworkCore.Tools
  ..installed all 3 straight from the Nuget manager.

  Scaffold-DbContext "Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\Inventory
  Scaffold-DbContext "Server=.\sqlexpress;Database=RMS;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\RMS
  Scaffold-DbContext "Server=.\sqlexpress;Database=RMS;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\RMS -force
  Scaffold-DbContext "Server=.\sqlexpress;Database=BR;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\BR
  
  Scaffold-DbContext "Server=MTdevSQLDB;Database=RMS;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model
  

  RMSClient_tempCertificate - has a password of RMSClient_tempCertificate


  Installers
  ClickOnce - Fails all over
  Try these:
  https://www.youtube.com/watch?v=yhOnClQrvBk
  https://www.youtube.com/watch?v=4t2TI8ImwMY
  https://www.youtube.com/watch?v=4t2TI8ImwMY&feature=youtu.be&t=1177