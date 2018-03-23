add-migration watchername -ProjectName BNE.Dashboard.Data -StartUpProjectName BNE.Dashboard.API

update-database -ProjectName BNE.Dashboard.Data -StartUpProjectName BNE.Dashboard.API -script
