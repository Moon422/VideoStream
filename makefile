cc=dotnet

run:
	$(cc) run --project Presentation

watch:
	$(cc) watch run --project Presentation

db:
	$(cc) ef migrations add "$(msg)" --project Infrastructure --startup-project Presentation
	$(cc) ef database update --project Infrastructure --startup-project Presentation 
	git add Infrastructure/Migrations 
	git commit -m '$(msg)' 
	echo "migration complete and committed to git"
