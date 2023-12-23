FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SavingsManagementSystem/SavingsManagementSystem.csproj", "SavingsManagementSystem/"]
COPY ["SavingsManagementSystem.Common/SavingsManagementSystem.Common.csproj", "SavingsManagementSystem.Common/"]
COPY ["SavingsManagementSystem.Data/SavingsManagementSystem.Data.csproj", "SavingsManagementSystem.Data/"]
COPY ["SavingsManagementSystem.Model/SavingsManagementSystem.Model.csproj", "SavingsManagementSystem.Model/"]
COPY ["SavingsManagementSystem.Repository/SavingsManagementSystem.Repository.csproj", "SavingsManagementSystem.Repository/"]
COPY ["SavingsManagementSystem.Service/SavingsManagementSystem.Service.csproj", "SavingsManagementSystem.Service/"]
RUN dotnet restore "SavingsManagementSystem/SavingsManagementSystem.csproj"
COPY . .
WORKDIR "/src/SavingsManagementSystem"
RUN dotnet build "SavingsManagementSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SavingsManagementSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish/src/SavingsManagementSystem\StaticFiles\Html\ConfirmEmail.html
COPY --from=publish/src/SavingsManagementSystem\StaticFiles\Html\ForgetPassword.html
COPY --from=publish/src/SavingsManagementSystem\StaticFiles\Html\MemberInvite.html

ENTRYPOINT ["dotnet", "SavingsManagementSystem.dll"]