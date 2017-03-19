# bagombo

Simple blogging software using ASP.NET Core MVC and Entity Framework Core

### 0.2 Alpha is released
* This is basically useable as a blog now
* The home page is really the only spot where modifying code is needed, by updating /Views/Home/Index.html -- basically to update links to entries instead of taking what was left over
* You can run it by compiling from Visual Studio 2017 and publishing it to a server, then set it up in IIS
* There is a CreateDb.sql file under src, you have to create the database but can run this to setup the tables
* For the search to work you need to create a full-text index on the BlogPost table
* Change appsettings.json to seed the Db with an admin user besdies my default
* Use the user manager to make an author
* Hopefully the rest you can figure out

Features for 1.0:
* Multi-Author support
* Features allow for different topics to easily be created and viewed
* Full text search
* Edit posts in straight markdown/HTML
* Export posts for portability
* Themes
* Supports any database with EF Core Provider
* Multi-Platform
    * Initial release tested on Windows 2016 with SQL Server 2016 Express Edition
	* 1.1 tested on additional platforms

MIT license

## Roadmap:
### * Not finished at all
### * Working on 0.2 for alpha release - Estimated Release is April 1st
### * Working on getting a live instance running on AWS
### * I'll add instructions for getting it running
### * 0.3 - code cleanup and refactoring - June 1st
### * 0.4 - Core Feature enhancements - July 1st
### * 0.5 - Data Layer Stabilization - August 1st
### * 0.6 - Core HTML and SEO 
### * 0.7 - Front End Enhancement and Themes - Nov 1st
### * 0.8 - code cleanup and refactoring - Jan 1st
### * 0.9 - Blog data portability - Feb 1st
### * 1.0 - Bug fixes




