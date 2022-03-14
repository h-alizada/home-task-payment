FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["WebApi/Checkout.PaymentGateway.WebApi/Checkout.PaymentGateway.WebApi.csproj", "WebApi/Checkout.PaymentGateway.WebApi/"]
COPY ["Checkout.PaymentGateway.Infrastructure.Services/Checkout.PaymentGateway.Infrastructure.Services.csproj", "Checkout.PaymentGateway.Infrastructure.Services/"]
COPY ["Checkout.PaymentGateway.Infrastructure.Services.Core/Checkout.PaymentGateway.Infrastructure.Services.Core.csproj", "Checkout.PaymentGateway.Infrastructure.Services.Core/"]
COPY ["Checkout.PaymentGateway.Domain/Checkout.PaymentGateway.Domain.csproj", "Checkout.PaymentGateway.Domain/"]
COPY ["Checkout.PaymentGateway.Infrastructure.Core/Checkout.PaymentGateway.Infrastructure.Core.csproj", "Checkout.PaymentGateway.Infrastructure.Core/"]
COPY ["Infrastructure/Checkout.PaymentGateway.Infrastructure.Http/Checkout.PaymentGateway.Infrastructure.Http.csproj", "Infrastructure/Checkout.PaymentGateway.Infrastructure.Http/"]
COPY ["Infrastructure/Checkout.PaymentGateway.Infrastructure.Cryptography/Checkout.PaymentGateway.Infrastructure.Cryptography.csproj", "Infrastructure/Checkout.PaymentGateway.Infrastructure.Cryptography/"]
COPY ["Checkout.PaymentGateway.DomainServices/Checkout.PaymentGateway.DomainServices.csproj", "Checkout.PaymentGateway.DomainServices/"]
COPY ["Checkout.PaymentGateway.DomainServices.Core/Checkout.PaymentGateway.DomainServices.Core.csproj", "Checkout.PaymentGateway.DomainServices.Core/"]
COPY ["Infrastructure/Checkout.PaymentGateway.Infrastructure.Sql/Checkout.PaymentGateway.Infrastructure.Sql.csproj", "Infrastructure/Checkout.PaymentGateway.Infrastructure.Sql/"]
RUN dotnet restore "WebApi/Checkout.PaymentGateway.WebApi/Checkout.PaymentGateway.WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi/Checkout.PaymentGateway.WebApi"
RUN dotnet build "Checkout.PaymentGateway.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.PaymentGateway.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.PaymentGateway.WebApi.dll"]