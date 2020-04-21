<h1>Database</h1>

<h3>Add Migration</h3>
<p>
In UsefulCMS dir...<br />
dotnet ef migrations add 00_name --project ../UsefulDatabase
</p>

<h3>Update Database</h3>
<p>
If production - $env:ASPNETCORE_ENVIRONMENT='Production'<br />
dotnet ef database update
</p>