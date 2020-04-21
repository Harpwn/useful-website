#ADD MIGRATION
##In usefulcms dir
dotnet ef migrations add 00_name --project ../UsefulDatabase

#UPDATE DATABASE 

##For Production
$env:ASPNETCORE_ENVIRONMENT='Production'

dotnet ef database update


#LIVE DEPLOYMENT

1)Update SQL Production Database
2)publish cms and api
3) profit???