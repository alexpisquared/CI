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


  Installers  -  ClickOnce fails all over  -    For MSIX try these:
    https://www.youtube.com/watch?v=yhOnClQrvBk
    https://www.youtube.com/watch?v=4t2TI8ImwMY
    https://www.youtube.com/watch?v=4t2TI8ImwMY&feature=youtu.be&t=1177

  #2021-03
  *.deps.json solution: clean ef nuget entries:
    <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="5.0.3" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="5.0.3" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Proxies" Version="5.0.3" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="5.0.3" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="5.0.3" />
    <PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="3.1.0" />


    MSIX 'Microsoft.Data.SqlClient.SNI.runtime', issue
        fixed in 16.9.0 according to https://developercommunity2.visualstudio.com/t/ClickOnce-no-longer-works/1288425
       currently 16.8.6 is what is on now ==> so just sit tight and wait.