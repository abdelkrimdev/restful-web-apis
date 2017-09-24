# Use the standard Microsoft .NET Core image
FROM microsoft/aspnetcore:2.0.0

# Copy the output folder to the '/app' folder in the container
WORKDIR /app
COPY out .

# Expose port 80 for the Web API traffic
ENV ASPNETCORE_URLS http://*:80
EXPOSE 80

# Run the application from within the container
ENTRYPOINT ["dotnet", "SupaTrupa.WebAPI.dll"]
